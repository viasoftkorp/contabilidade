using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Viasoft.Accounting.Domain.Dtos;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Accounting.Domain.Enums;
using Viasoft.Accounting.Domain.Services.External;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Core.Identity.Abstractions;
using Viasoft.Core.Identity.Abstractions.Model;
using Viasoft.Core.Identity.Abstractions.Store;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.PushNotifications.Abstractions.Contracts;
using Viasoft.PushNotifications.Abstractions.Notification;

namespace Viasoft.Accounting.Domain.Services
{
    public class AccountingEntriesService : IAccountingEntriesService, ITransientDependency
    {
        private readonly IRepository<AccountingEntry> _accountingEntries;
        private readonly IRepository<AccountingEntryItem> _accountingEntryItems;
        private readonly IRepository<AccountingEntryItemOrigin> _accountingEntryItemOrigins;
        private readonly IRepository<LegacyFechamentoPeriodoContabil> _legacyFechamentoPeriodoContabils;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountingEntriesService> _logger;
        private readonly IPushNotification _pushNotification;
        private readonly ICurrentUser _currentUser;
        private readonly IUserStore _userStore;
        private readonly IGetCodeAccountEntryService _getCodeAccountEntryService;
        private readonly IServiceBus _serviceBus;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AccountingEntriesService(IRepository<AccountingEntryItem> accountingEntryItems, IRepository<AccountingEntry> accountingEntries, IMapper mapper, IUnitOfWork unitOfWork, IRepository<LegacyFechamentoPeriodoContabil> legacyFechamentoPeriodoContabils, ILogger<AccountingEntriesService> logger, IPushNotification pushNotification, ICurrentUser currentUser, IUserStore userStore, IGetCodeAccountEntryService getCodeAccountEntryService, IServiceBus serviceBus, IDateTimeProvider dateTimeProvider, IRepository<AccountingEntryItemOrigin> accountingEntryItemOrigins)
        {
            _accountingEntryItems = accountingEntryItems;
            _accountingEntries = accountingEntries;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _legacyFechamentoPeriodoContabils = legacyFechamentoPeriodoContabils;
            _logger = logger;
            _pushNotification = pushNotification;
            _currentUser = currentUser;
            _userStore = userStore;
            _getCodeAccountEntryService = getCodeAccountEntryService;
            _serviceBus = serviceBus;
            _dateTimeProvider = dateTimeProvider;
            _accountingEntryItemOrigins = accountingEntryItemOrigins;
        }

        public async Task AddAccountingEntry(AccountingEntryDto entry, List<AccountingEntryItemDto> items)
        {
            var userPreferences = await _userStore.GetUserPreferencesAsync(_currentUser.Id);
            var newEntry = _mapper.Map<AccountingEntry>(entry);
            var userDetails = await _userStore.GetUserDetailsAsync(_currentUser.Id);
            newEntry.Usuario = userDetails.Login;

            var currentEntryCode = await _getCodeAccountEntryService.GetEntryCode();          

            newEntry.Code = currentEntryCode.Value;

            foreach (var item in items)
            {
                item.EntryCode = currentEntryCode.Value;
                item.AccountingEntryId = newEntry.Id;
            }
            var entryItemsWithOrigins = GetEntryItemsForCreation(items, userDetails.Login);
            try
            {
                await InsertItemsAsync(newEntry, entryItemsWithOrigins, userPreferences);
                    }
            catch (DbUpdateException e)
            {
                if (e.InnerException is not SqlException sqlException)
                {
                    throw e;
                }

                var procedureDaKorp = sqlException.Procedure == "KORP_EXCECAO_CONTA_SINTETICA";
                if (procedureDaKorp)
                {
                    _logger.LogError(e, "Falha ao inserir lançamento contabeis");
                    await _pushNotification.SendAsync(new Payload
                    {
                        Header = $"Atenção! Falha na criação dos lançamentos contabeis do documento {entry.Notes}",
                        Body = sqlException.Message
                    });
                    return;
                }

                var keyDuplicada = sqlException.Number == 2627;
                if (keyDuplicada)
                {
                    var retries = 0;
                    do
                    {
                        currentEntryCode = await _getCodeAccountEntryService.GetEntryCode();
                        newEntry.Code = currentEntryCode.Value;
                                foreach (var entryItemWithOrigins in entryItemsWithOrigins)
                                {
                                    entryItemWithOrigins.EntryItem.EntryCode = newEntry.Code;
                        }

                        try
                                    {
                            await InsertItemsAsync(newEntry, entryItemsWithOrigins, userPreferences);

                            break;
                        }
                        catch (Exception)
                        {
                            retries++;
                        }

                    } while (retries < 5);
                }

            }
        }
            
        private async Task InsertItemsAsync(AccountingEntry entry, List<AccountingEntryItemWithOriginsDto> entryItems, UserPreferences userPreferences)
        {
            using (_unitOfWork.Begin())
            {
                await _accountingEntries.InsertAsync(entry);

                foreach (var entryItemWithOrigins in entryItems)
                {
                    var entryItemInserted = await _accountingEntryItems.InsertAsync(entryItemWithOrigins.EntryItem);

                    if (entryItemWithOrigins.Origins == null)
                        continue;

                    foreach (var origin in entryItemWithOrigins.Origins)
                    {
                        origin.AccountingEntryItem = entryItemInserted;
                        origin.CreationDate = _dateTimeProvider.UtcNow().ToLocal(userPreferences.DefaultUserTimeZone);

                        await _accountingEntryItemOrigins.InsertAsync(origin);
                    }
                }   
                await _unitOfWork.CompleteAsync();
            }
        }
        
        private List<AccountingEntryItemWithOriginsDto> GetEntryItemsForCreation(List<AccountingEntryItemDto> items, string usuario)
        {
            var orderedItems = items.OrderBy(item => item.Order);

            var output = new List<AccountingEntryItemWithOriginsDto>();
            foreach (var item in orderedItems)
            {
                var outputItem = new AccountingEntryItemWithOriginsDto
                {
                    EntryItem = _mapper.Map<AccountingEntryItem>(item),
                    Origins = new List<AccountingEntryItemOrigin>()
                };
            
                output.Add(outputItem);

                outputItem.EntryItem.Usuario = usuario;

                if (item.AccountingEntryItemOrigins == null) 
                    continue;
            
                foreach (var itemOrigin in item.AccountingEntryItemOrigins)
                {
                    if (itemOrigin.OriginType == AccountingEntryItemOriginType.tolDesconhecido)
                        continue;
            
                    outputItem.Origins.Add(new AccountingEntryItemOrigin
                    {
                        IdOrigem = itemOrigin.IdOrigin,
                        LegacyIdOrigem = itemOrigin.LegacyIdOrigin,
                        DebitValue = itemOrigin.DebitValue.GetValueOrDefault(),
                        CreditValue = itemOrigin.CreditValue.GetValueOrDefault(),
                        OriginType = itemOrigin.OriginType,
                    });
        }
            }
      
            return output;
        }
      
        public async Task RemoveAccountingEntries(Guid sourceId)
        {
            var accountingEntriesIds = _accountingEntries.Where(e => e.SourceId.Value == sourceId).Select(e => e.Id );
            var accountingEntryItemsLegacyIds = _accountingEntryItems.Where(e => accountingEntriesIds.Contains(e.AccountingEntryId.Value)).Select(e => e.LegacyId);
            using (_unitOfWork.Begin(op => op.LazyTransactionInitiation = false))
            {
                await _accountingEntryItemOrigins.BatchHardDeleteAsync(e => accountingEntryItemsLegacyIds.Contains(e.AccountingEntryItemLegacyId));
                await _accountingEntryItems.BatchHardDeleteAsync(e => accountingEntriesIds.Contains(e.AccountingEntryId.Value));
                await _accountingEntries.BatchHardDeleteAsync(e => e.SourceId.Value == sourceId);
                await _unitOfWork.CompleteAsync();
            }

        }      
        public async Task<MotivoEstornarLancamentoContabilResult> Estornar(Guid sourceId)
        {
            var lancamentoParaEstornar = await _accountingEntries.FirstOrDefaultAsync(lancamento => lancamento.SourceId == sourceId);
            if (lancamentoParaEstornar is null)
            {
                return MotivoEstornarLancamentoContabilResult.Ok;
            }
            
            var contemPeriodoContabilFechadoContentoLancamento = await _legacyFechamentoPeriodoContabils
                .AnyAsync(fechamento => string.Compare(fechamento.Data, lancamentoParaEstornar.EntryDateLegacy) >= 0);
            if (contemPeriodoContabilFechadoContentoLancamento)
            {
                return MotivoEstornarLancamentoContabilResult.PeriodoContabilFechado;
            }
            
            var accountingEntryItemsLegacyIds = _accountingEntryItems.Where(itemLancamento => itemLancamento.AccountingEntryId == lancamentoParaEstornar.Id).Select(e => e.LegacyId);

            using (_unitOfWork.Begin(op => op.LazyTransactionInitiation = false))
            {
                await _accountingEntryItemOrigins.BatchHardDeleteAsync(e => accountingEntryItemsLegacyIds.Contains(e.AccountingEntryItemLegacyId));
                await _accountingEntryItems.BatchHardDeleteAsync(itemLancamento => itemLancamento.AccountingEntryId == lancamentoParaEstornar.Id);
                await _accountingEntries.BatchHardDeleteAsync(lancamento => lancamento.Id == lancamentoParaEstornar.Id);
                await _unitOfWork.CompleteAsync();
            }

            return MotivoEstornarLancamentoContabilResult.Ok;
        }
    }
}