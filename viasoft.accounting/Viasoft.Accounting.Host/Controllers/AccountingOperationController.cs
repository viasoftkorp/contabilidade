using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Accounting.Host.Controllers.Dtos;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Accounting.Host.Controllers.Inputs;
using Viasoft.Accounting.Host.Controllers.Outputs;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;

namespace Viasoft.Accounting.Host.Controllers
{
        [ApiVersion(2023.4), ApiVersion(2023.3), ApiVersion(2023.2), ApiVersion(2023.1), ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    public class AccountingOperationController: BaseReadonlyForSelectController<AccountingOperation, AccountingOperationOutput, AccountOperationGetAllInput, AccountOperationGetAllInput, AccountingOperationOutput, string>, IHaveAutocomplete
    {
        private readonly IRepository<AccountingOperation> _accountingOperations;

        public AccountingOperationController(IRepository<AccountingOperation> accountingOperations, IMapper mapper) : base(accountingOperations, mapper)
        {
            _accountingOperations = accountingOperations;
        }

        protected override (Expression<Func<AccountingOperation, string>>, bool) DefaultGetAllSorting()
        {
            return (operation => operation.Description, true);
        }

        protected override IQueryable<AccountingOperation> ApplyCustomFiltersForSelect(IQueryable<AccountingOperation> query, AccountOperationGetAllInput input)
        {
            return query
                .WhereIf(input.IssueInvoice.HasValue, operation => operation.IssueInvoice.Value)
                .WhereIf(input.Filter != null, e => e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter) );
        }

        protected override IQueryable<AccountingOperation> ApplyCustomFilters(IQueryable<AccountingOperation> query, AccountOperationGetAllInput input)
        {
            return query
                .OrderBy(accountingOperation => accountingOperation.Code)
                .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting)
                .WhereIf(input.IssueInvoice.HasValue, operation => operation.IssueInvoice.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), accountingOperation =>
                    accountingOperation.Code.Contains(input.Filter)
                    || accountingOperation.Description.Contains(input.Filter));
        }

        [HttpGet("{code}")]
        public async Task<AccountingOperationOutput> GetByCode(string code)
        {
            return await Repository.Where(e => e.Code == code)
                .Select(e => new AccountingOperationOutput(e))
                .FirstOrDefaultAsync();
        }
        [HttpGet("/accounting/AccountingOperation/GetAll")]
        [HttpGet("/accounting/operacoes-contabeis")]
        public override async Task<PagedResultDto<AccountingOperationOutput>> GetAll([FromQuery] AccountOperationGetAllInput input)
        {
            return await base.GetAll(input);
        }
        [HttpGet("/accounting/operacoes-contabeis/{id}")]
        public async Task<AccountingOperationOutput> GetByIdV2(Guid id)
        {
            return await base.GetById(id);
        }

        [HttpGet]
        public async Task<AutocompleteOutput> GetForAutocomplete([FromQuery] AutocompleteInput input)
        {
            var result = _accountingOperations
                .WhereIf(input.ValueToFilter != null, e => e.Description.Contains(input.ValueToFilter) || e.Code.Contains(input.ValueToFilter))
                .Select(e => new AutocompleteOutputItems{
                    Option = new AutocompleteOutputValue{
                        Value = e.Id.ToString(),
                        Key = e.Code + " - " + e.Description
                    }
                });

            var totalCount = await result.CountAsync();
            var items = await result
                .PageBy(input.SkipCount, input.MaxDropSize)
                .ToListAsync();
            var output = new AutocompleteOutput{TotalCount = totalCount, Items = items};

            return output;
        }

        [HttpGet]
        public async Task<List<NameValueDto>> GetAutocompleteOptions([FromQuery] List<Guid> ids)
        {
            var userOutput = await Repository
                .Where(e => ids.Contains(e.Id))
                .Select(e => new NameValueDto
                {
                    Id = e.Id,
                    Name = e.Description
                }).ToListAsync();

            return userOutput;
        }

        [HttpGet]
        public async Task<List<NameValueDto>> GetAccountingOperationsDescriptions([FromQuery] List<Guid> ids)
        {
            var accountingOperations = await _accountingOperations
                .Where(e => ids.Contains(e.Id))
                .Select(e => new NameValueDto
                {
                    Id = e.Id,
                    Name = e.Code + " - " + e.Description
                }).ToListAsync();

            return accountingOperations;
        }
    }
}
