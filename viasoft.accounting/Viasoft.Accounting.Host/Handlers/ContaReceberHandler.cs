using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;
using Viasoft.Accounting.Domain;
using Viasoft.Accounting.Domain.Contracts.PropostasComerciais;
using Viasoft.Accounting.Domain.Dtos;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Accounting.Domain.Enums;
using Viasoft.Accounting.Host.Contracts;
using Viasoft.Accounting.Host.Providers.Pessoas;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.Identity.Abstractions;
using Viasoft.Core.Identity.Abstractions.Store;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.ServiceBus.Abstractions;
using AccountingEntryDto = Viasoft.Accounting.Domain.Dtos.AccountingEntryDto;

namespace Viasoft.Accounting.Host.Handlers;

public class ContaReceberHandler : IHandleMessages<ContaAReceberCriada>
{
    private readonly IReadOnlyRepository<AccountingOperation> _accountingOperations;
    private readonly IReadOnlyRepository<LegacyTemplateLancamentoContabil> _templateLancamentoContabils;
    private readonly IReadOnlyRepository<BookkeepingAccount> _bookkeepingAccounts;
    private readonly IReadOnlyRepository<AccountingEntryHistory> _entryHistories;
    private readonly IReadOnlyRepository<LegacyComplemento> _complementos;
    private readonly IReadOnlyRepository<LegacyRegraValorLancamento> _regraValor;
    private readonly IServiceBus _bus;
    private readonly ICurrentCompany _company;
    private readonly IGetPersonProvider _getPersonProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUser _currentUser;
    private readonly IUserStore _userStore;
    private readonly ILogger<ContaReceberHandler> _logger;

    public ContaReceberHandler(IReadOnlyRepository<AccountingOperation> accountingOperations, IReadOnlyRepository<BookkeepingAccount> bookkeepingAccounts, IReadOnlyRepository<AccountingEntryHistory> entryHistories, IServiceBus bus, ICurrentCompany company, IGetPersonProvider getPersonProvider, IDateTimeProvider dateTimeProvider, ICurrentUser currentUser, IUserStore userStore, IReadOnlyRepository<LegacyTemplateLancamentoContabil> templateLancamentoContabils, IReadOnlyRepository<LegacyComplemento> complementos, IReadOnlyRepository<LegacyRegraValorLancamento> regraValor, ILogger<ContaReceberHandler> logger)
    {
        _accountingOperations = accountingOperations;
        _bookkeepingAccounts = bookkeepingAccounts;
        _entryHistories = entryHistories;
        _bus = bus;
        _company = company;
        _getPersonProvider = getPersonProvider;
        _dateTimeProvider = dateTimeProvider;
        _currentUser = currentUser;
        _userStore = userStore;
        _templateLancamentoContabils = templateLancamentoContabils;
        _complementos = complementos;
        _regraValor = regraValor;
        _logger = logger;
    }

    public async Task Handle(ContaAReceberCriada message)
    {
        var command = new CreateAccountingEntry
            {
                SourceId = message.IdContaReceber, 
                AccountingOperationId = message.IdOperacaoContabil
            };
            
            var codigoOperacaoContabil = await _accountingOperations.Where(e => e.Id == message.IdOperacaoContabil)
                .Select(e => e.Code).FirstAsync();
            var templatesLancamento = await _templateLancamentoContabils
                .Where(e => e.CodigoOperacaoContabil == codigoOperacaoContabil)
                .ToListAsync();

            if (templatesLancamento.Count <= 0)
            {
                return;
            }

            var cliente = await _getPersonProvider.Get(message.IdCliente);

            var codigoComplementos = templatesLancamento.SelectMany(template => new List<int?>
            {
                template.CodigoComplemento1,
                template.CodigoComplemento2,
                template.CodigoComplemento3,
                template.CodigoComplemento4,
                template.CodigoComplemento5,
            }).Distinct().ToList();
            var complementos = await _complementos
                .AsNoTracking()
                .Where(complemento => codigoComplementos.Contains(complemento.Codigo))
                .ToDictionaryAsync(complemento => complemento.Codigo, complemento => complemento);

            var regraValores = await _regraValor.AsNoTracking().Where(regra =>
                    templatesLancamento.Select(template => template.CodigoCampo).Contains(regra.Codigo))
                .ToDictionaryAsync(r => r.Codigo, r => r.Campo);
            var date = _dateTimeProvider.UtcNow();
            var userPreferences = await _userStore.GetUserPreferencesAsync(_currentUser.Id);
            var userDateTime = date.ToLocal(userPreferences.DefaultUserTimeZone);
            var accountingEntry = new AccountingEntryDto
            {
                Id = Guid.NewGuid(),
                EntryType = "004",
               // EntryDateLegacy = null,
               // CreationTimeLegacy = null,
                AccountingMonth = userDateTime.Month,
                AccountingYear = userDateTime.Year,
                Customer = cliente.Code,
                SourceId = message.IdContaReceber,
                EntryDate = message.DataEmissao,
                Series = null,
                SourceType = EntrySourceType.ContasReceber,
                Notes = message.Documento,
                CompanyCode = _company.LegacyId,
                Status = "A",
                Fornecedor = cliente.Code,
                Entrada = "E",
                LegacyIdContaReceber = message.LegacyIdContaReceber,

            };

            command.AccountingEntry = accountingEntry;
            
            command.AccountingEntryItems = new List<AccountingEntryItemDto>();
            var ordem = 1;
            foreach (var entryRule in templatesLancamento)
            {
                var value = GetValorContaReceber(message, regraValores[entryRule.CodigoCampo]);
                if (value <= 0)
                {
                    continue;
                }

                var notes =
                    entryRule.CodigoComplemento1.HasValue
                        ? GetDescricaoComplemento(complementos[entryRule.CodigoComplemento1.Value], cliente)
                        : "";
                notes += entryRule.CodigoComplemento2.HasValue
                    ? " " + GetDescricaoComplemento(complementos[entryRule.CodigoComplemento2.Value], cliente)
                    : "";       
                notes += entryRule.CodigoComplemento3.HasValue
                    ? " " + GetDescricaoComplemento(complementos[entryRule.CodigoComplemento3.Value], cliente)
                    : ""; 
                notes += entryRule.CodigoComplemento4.HasValue
                    ? " " + GetDescricaoComplemento(complementos[entryRule.CodigoComplemento4.Value], cliente)
                    : "";    
                notes += entryRule.CodigoComplemento5.HasValue
                    ? " " + GetDescricaoComplemento(complementos[entryRule.CodigoComplemento5.Value], cliente)
                    : "";

                var accountEntryItem = new AccountingEntryItemDto
                {
                    Id = Guid.NewGuid(),
                    Notes = notes,
                    AccountCode = entryRule.CodigoContaContabil,
                    CompanyCode = _company.LegacyId,
                    AccountingOperation = codigoOperacaoContabil,
                    CreditValue = entryRule.TipoLancamento == "C" ? value : decimal.Zero,
                    DebitValue = entryRule.TipoLancamento == "D" ? value : decimal.Zero,
                    EntryHistoricCode = entryRule.CodigoHistorico,
                    Order = ordem++,
                    Origem = "R",
                    LegacyIdOrigem = message.LegacyIdContaReceber
                };
                
                accountEntryItem.AccountingEntryItemOrigins = new List<AccountingEntryItemOriginDto>();
                var itemOrigin = new AccountingEntryItemOriginDto
                {
                    DebitValue = accountEntryItem.DebitValue,
                    CreditValue = accountEntryItem.CreditValue,
                    OriginType = AccountingEntryItemOriginType.tolContasAReceber,
                    LegacyIdOrigin = message.LegacyIdContaReceber
                };
                if (message.Adiantamento)
                {
                    itemOrigin.OriginType = AccountingEntryItemOriginType.tolContasAReceberAdiantamento;
                }
                
                accountEntryItem.AccountingEntryItemOrigins.Add(itemOrigin);

                command.AccountingEntryItems.Add(accountEntryItem);
            }

            if (command.AccountingEntryItems.Count <= 0)
            {
                return;
            }

            await _bus.SendLocal(command);
    }

    private string GetDescricaoComplemento(LegacyComplemento complemento, PersonOutput cliente)
    {
        switch (complemento.Tabela)
        {
            case "CLIENTES":
                return cliente.CompanyName;
            default:
                return "";
        }
    }

    private decimal GetValorContaReceber(ContaAReceberCriada message, string campo)
    {
        switch (campo)
        {
            case "JUROS":
                return message.Juros;       
            case "VALOR":
                return message.Valor;   
            case "VALORLIQUIDO":
                return message.ValorLiquido;
            default:
                _logger.LogError($"Busca de valor nao mapeado em contas a receber para o lancamento contabil, valor buscado foi {campo}");
                return 0;
        }
    }
}