using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Accounting.Domain;
using Viasoft.Accounting.Domain.Contracts;
using Viasoft.Accounting.Domain.Contracts.Ctes;
using Viasoft.Accounting.Domain.Dtos;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Accounting.Domain.Enums;
using Viasoft.Accounting.Host.Contracts;
using Viasoft.Accounting.Host.Messages;
using Viasoft.Core.AmbientData;
using Viasoft.Core.AmbientData.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.Identity.Abstractions.Store;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Accounting.Host.Handlers
{
    public class CteIssuedHandler : IHandleMessages<CteIssued>
    {
        private readonly IReadOnlyRepository<AccountingOperation> _accountingOperations;
        private readonly IReadOnlyRepository<AccountingOperationEntryRule> _accountingOperationEntryRules;
        private readonly IReadOnlyRepository<BookkeepingAccount> _bookkeepingAccounts;
        private readonly IReadOnlyRepository<AccountingEntryHistory> _entryHistories;
        private readonly IServiceBus _bus;
        private readonly ICurrentCompany _company;
        private readonly IUserStore _userStore;
        private readonly IAmbientData _ambientData;


        public CteIssuedHandler(
            IReadOnlyRepository<AccountingOperationEntryRule> accountingOperationEntryRules,
            IReadOnlyRepository<BookkeepingAccount> bookkeepingAccounts,
            IReadOnlyRepository<AccountingOperation> accountingOperations,
            IReadOnlyRepository<AccountingEntryHistory> entryHistories,
            IServiceBus bus, ICurrentCompany company, IUserStore userStore, IAmbientData ambientData)
        {
            _accountingOperationEntryRules = accountingOperationEntryRules;
            _bookkeepingAccounts = bookkeepingAccounts;
            _accountingOperations = accountingOperations;
            _entryHistories = entryHistories;
            _bus = bus;
            _company = company;
            _userStore = userStore;
            _ambientData = ambientData;
        }

        public async Task Handle(CteIssued cteIssued)
        {
            var command = new CreateAccountingEntry
            {
                SourceId = cteIssued.Id, 
                AccountingOperationId = cteIssued.AccountingOperationId
            };
            
            var accountingOperationCode = _accountingOperations.Where(e => e.Id == cteIssued.AccountingOperationId)
                .Select(e => e.Code).First();
            var accountingOperationEntryRules = _accountingOperationEntryRules
                .Where(e => e.AccountingOperationId == cteIssued.AccountingOperationId)
                .ToList();

            if (accountingOperationEntryRules.Count <= 0)
            {
                return;
            }

            var borrowerCode = cteIssued.BorrowerCode;

            var bookkeepingAccountIds = accountingOperationEntryRules.Select(e => e.BookkeepingAccountId).ToList();
            var bookkeepingAccounts = _bookkeepingAccounts.Where(e => bookkeepingAccountIds.Contains(e.Id))
                .ToDictionary(b => b.Id, b => b.Code);

            var historicIds = accountingOperationEntryRules
                .Where(e => e.AccountingOperationId == cteIssued.AccountingOperationId)
                .Select(e => e.HistoricId).ToList();
            var historicCodes = _entryHistories.Where(e => historicIds.Contains(e.Id))
                .ToDictionary(h => h.Id, h => h.Code);
            
            
            var userPreferences = await _userStore.GetUserPreferencesAsync(_ambientData.GetUserId());
            
            var accountingEntry = new AccountingEntryDto
            {
                Id = Guid.NewGuid(),
                AccountingMonth = cteIssued.EmissionDate.ToLocal(userPreferences.DefaultUserTimeZone).Month,
                AccountingYear = cteIssued.EmissionDate.ToLocal(userPreferences.DefaultUserTimeZone).Year,
                Customer = borrowerCode,
                SourceId = cteIssued.Id,
                EntryDate = cteIssued.EmissionDate.ToLocal(userPreferences.DefaultUserTimeZone),
                Series = cteIssued.SeriesNumber.ToString(),
                Notes = cteIssued.CteNumber.ToString(),
                CompanyCode = _company.LegacyId,
                EntryType = "016",
                SourceType = EntrySourceType.Cte
            };

            command.AccountingEntry = accountingEntry;
            
            command.AccountingEntryItems = new List<AccountingEntryItemDto>();
            foreach (var entryRule in accountingOperationEntryRules)
            {
                var value = cteIssued.Variables.ContainsKey(entryRule.EntryVariable) ? cteIssued.Variables[entryRule.EntryVariable] : 0;
                if (value <= 0)
                {
                    continue;
                }

                var notes =
                    entryRule.FirstDisplayInfo.HasValue &&
                    cteIssued.Complements.ContainsKey((CteComplement) entryRule.FirstDisplayInfo)
                        ? cteIssued.Complements[(CteComplement) entryRule.FirstDisplayInfo]
                        : "";
                notes += entryRule.SecondDisplayInfo.HasValue &&
                         cteIssued.Complements.ContainsKey((CteComplement) entryRule.SecondDisplayInfo)
                    ? " " + cteIssued.Complements[(CteComplement) entryRule.SecondDisplayInfo]
                    : "";

                var accountEntryItem = new AccountingEntryItemDto
                {
                    Id = Guid.NewGuid(),
                    Notes = notes,
                    AccountCode = bookkeepingAccounts[entryRule.BookkeepingAccountId],
                    CompanyCode = _company.LegacyId,
                    AccountingOperation = accountingOperationCode,
                    CreditValue = entryRule.AccountingEntryType == AccountingEntryType.Credit ? value : decimal.Zero,
                    DebitValue = entryRule.AccountingEntryType == AccountingEntryType.Debit ? value : decimal.Zero,
                    EntryHistoricCode = historicCodes[entryRule.HistoricId],
                    Order = entryRule.Order
                };
                
                accountEntryItem.AccountingEntryItemOrigins = new List<AccountingEntryItemOriginDto>();
                var itemOrigin = new AccountingEntryItemOriginDto
                {
                    DebitValue = accountEntryItem.DebitValue,
                    CreditValue = accountEntryItem.CreditValue,
                    OriginType = AccountingEntryItemOriginType.tolCteEmissao,
                    IdOrigin = cteIssued.Id
                };
                
                accountEntryItem.AccountingEntryItemOrigins.Add(itemOrigin);
                
                command.AccountingEntryItems.Add(accountEntryItem);
            }

            if (command.AccountingEntryItems.Count <= 0)
            {
                return;
            }

             await _bus.SendLocal(command);
        }

    }
}