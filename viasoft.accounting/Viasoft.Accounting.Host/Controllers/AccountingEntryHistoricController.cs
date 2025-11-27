using System;
using System.Linq;
using System.Linq.Expressions;
using Asp.Versioning;
using AutoMapper;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Accounting.Host.Controllers.Outputs;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Repositories;

namespace Viasoft.Accounting.Host.Controllers
{
        [ApiVersion(2023.4), ApiVersion(2023.3), ApiVersion(2023.2), ApiVersion(2023.1), ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    public class AccountingEntryHistoricController : BaseReadonlyController<AccountingEntryHistory, AccountingEntryHistoricOutput, PagedFilteredAndSortedRequestInput, string>
    {
        protected override (Expression<Func<AccountingEntryHistory, string>>, bool) DefaultGetAllSorting()
        {
            return (e => e.Description, true);
        }

        protected override IQueryable<AccountingEntryHistory> ApplyCustomFilters(IQueryable<AccountingEntryHistory> query, PagedFilteredAndSortedRequestInput input)
        {
            return query
                .Where(e => e.Module == "CTE")
                .OrderBy(e => e.Code)
                .Select(e => new AccountingEntryHistory
            {
                Id = e.Id,
                Code = e.Code,
                Description = e.Description
            });
        }

        public AccountingEntryHistoricController(IReadOnlyRepository<AccountingEntryHistory> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}