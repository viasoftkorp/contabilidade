using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Accounting.Host.Controllers.Inputs;
using Viasoft.Accounting.Host.Controllers.Inputs.Gets;
using Viasoft.Accounting.Host.Controllers.Outputs;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;

namespace Viasoft.Accounting.Host.Controllers
{
    [ApiVersion(2023.4), ApiVersion(2023.3), ApiVersion(2023.2), ApiVersion(2023.1), ApiVersion(2022.4)]
    public class AccountingOperationEntryRulesController : BaseCrudController<AccountingOperationEntryRule,
        AccountingOperationEntryRuleOutput, AccountingOperationEntryRuleInput, AccountingOperationEntryRuleInput,
        AccountingOperationEntryRuleOutput, GetAllAccountingOperationEntryRulesInput, string>
    {
        private readonly IReadOnlyRepository<BookkeepingAccount> _bookkeepingAccounts;
        private readonly IReadOnlyRepository<AccountingEntryHistory> _entryHistories;
        private readonly IUnitOfWork _unitOfWork;

        public AccountingOperationEntryRulesController(IMapper mapper,
            IRepository<AccountingOperationEntryRule> repository,
            IReadOnlyRepository<BookkeepingAccount> bookkeepingAccounts,
            IReadOnlyRepository<AccountingEntryHistory> entryHistories, IUnitOfWork unitOfWork) : base(mapper, repository)
        {
            _bookkeepingAccounts = bookkeepingAccounts;
            _entryHistories = entryHistories;
            _unitOfWork = unitOfWork;
        }

        protected override (Expression<Func<AccountingOperationEntryRule, string>>, bool) DefaultGetAllSorting()
        {
            return (e => e.EntryVariable, true);
        }

        [HttpGet]
        public override async Task<PagedResultDto<AccountingOperationEntryRuleOutput>> GetAll(
            [FromQuery] GetAllAccountingOperationEntryRulesInput input)
        {
            var result = await base.GetAll(input);

            var bookkeepingIds = result.Items.Select(e => e.BookkeepingAccountId);

            var bookkeepingAccounts = await _bookkeepingAccounts.Where(e => bookkeepingIds.Contains(e.Id)).Select(e => new
            {
                e.Id,
                e.Code,
                e.Classification,
                e.Name
            }).ToListAsync();

            var historyIds = result.Items.Select(e => e.HistoricId);

            var histories = _entryHistories.Where(e => historyIds.Contains(e.Id)).Select(e => new
            {
                e.Id,
                e.Description,
            });

            foreach (var resultItem in result.Items)
            { 
                resultItem.BookkeepingAccountCode =
                    bookkeepingAccounts.First(e => e.Id == resultItem.BookkeepingAccountId).Code;
                resultItem.BookkeepingAccountClassification =
                    bookkeepingAccounts.First(e => e.Id == resultItem.BookkeepingAccountId).Classification;
                resultItem.BookkeepingAccountDescription =
                    bookkeepingAccounts.First(e => e.Id == resultItem.BookkeepingAccountId).Name;

                resultItem.HistoricDescription = (await histories.FirstAsync(e => e.Id == resultItem.HistoricId)).Description;
            }

            return result;
        }

        protected override IQueryable<AccountingOperationEntryRule> ApplyCustomFilters(
            IQueryable<AccountingOperationEntryRule> query, GetAllAccountingOperationEntryRulesInput input)
        {
            return query.Where(e => e.AccountingOperationId == input.AccountingOperationId).OrderBy(e => e.Order);
        }

        [HttpPost]
        public override Task<AccountingOperationEntryRuleOutput> Create(AccountingOperationEntryRuleInput input)
        {
            var accountingOperationEntryRules =
                Repository.Where(e => e.AccountingOperationId == input.AccountingOperationId);
            var order = 0;

            if (accountingOperationEntryRules.Any())
            {
                order = accountingOperationEntryRules.Max(e => e.Order);
            }

            input.Order = ++order;
            return base.Create(input);
        }

        [HttpDelete]
        public override async Task Delete(Guid id)
        {
            var accountingOperationId = (await Repository.FindAsync(id)).AccountingOperationId;

            var ruleToDelete = await Repository.FindAsync(id);
            var rules = Repository
                .Where(e => e.AccountingOperationId == accountingOperationId && e.Id != id)
                .OrderBy(e => e.Order);

            var order = 1;
            foreach (var rule in rules)
            {
                rule.Order = order++;
            }
            using (_unitOfWork.Begin())
            {
                foreach (var rule in rules)
                {
                    await Repository.UpdateAsync(rule);
                }
                await Repository.DeleteAsync(ruleToDelete);
                await _unitOfWork.CompleteAsync();
            }



        }

        [HttpPost]
        public async Task MoveOrderDown(Guid id)
        {
            var accountingOperationEntryRule = await Repository.FindAsync(id);

            var previousTaxRules = await Repository.FirstOrDefaultAsync(e =>
                e.AccountingOperationId == accountingOperationEntryRule.AccountingOperationId &&
                e.Order == accountingOperationEntryRule.Order - 1);

            if (previousTaxRules != null)
            {
                accountingOperationEntryRule.Order = previousTaxRules.Order;
                previousTaxRules.Order = accountingOperationEntryRule.Order + 1;
            }
            using (_unitOfWork.Begin())
            {
                await Repository.UpdateAsync(accountingOperationEntryRule);
                await Repository.UpdateAsync(previousTaxRules);
                await _unitOfWork.CompleteAsync();
            }

        }

        [HttpPost]
        public async Task MoveOrderUp(Guid id)
        {
            var accountingOperationEntryRule = await Repository.FindAsync(id);

            var nextRule = await Repository.FirstOrDefaultAsync(e =>
                e.AccountingOperationId == accountingOperationEntryRule.AccountingOperationId &&
                e.Order == accountingOperationEntryRule.Order + 1);

            if (nextRule != null)
            {
                accountingOperationEntryRule.Order = nextRule.Order;
                nextRule.Order = accountingOperationEntryRule.Order - 1;
            }
            using (_unitOfWork.Begin())
            {
                await Repository.UpdateAsync(accountingOperationEntryRule);
                await Repository.UpdateAsync(nextRule);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}