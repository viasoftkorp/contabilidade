using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Accounting.Host.Controllers.Dtos;
using Viasoft.Accounting.Host.Controllers.Inputs;
using Viasoft.Accounting.Host.Controllers.Outputs;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;

namespace Viasoft.Accounting.Host.Controllers
{
    [ApiVersion(2023.4), ApiVersion(2023.3), ApiVersion(2023.2), ApiVersion(2023.1), ApiVersion(2022.4)]
    public class ManagerialAccountController : BaseReadonlyForSelectController<ManagerialAccount, ManagerialAccountOutput, PagedFilteredAndSortedRequestInput, PagedFilteredAndSortedRequestInput, ManagerialAccountOutput, string>, IHaveAutocomplete
    {
        public ManagerialAccountController(IReadOnlyRepository<ManagerialAccount> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        protected override (Expression<Func<ManagerialAccount, string>>, bool) DefaultGetAllSorting()
        {
            return (e => e.Description, true);
        }

        protected override IQueryable<ManagerialAccount> ApplyCustomFilters(IQueryable<ManagerialAccount> query, PagedFilteredAndSortedRequestInput input)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter));  
        }

        protected override IQueryable<ManagerialAccount> ApplyCustomFiltersForSelect(IQueryable<ManagerialAccount> query, PagedFilteredAndSortedRequestInput input)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter));  

        }

        [HttpGet]
        public async Task<AutocompleteOutput> GetForAutocomplete([FromQuery] AutocompleteInput input)
        { 
            var result = Repository
                .WhereIf(input.ValueToFilter != null, e => e.Description.Contains(input.ValueToFilter))
                .Select(e => new AutocompleteOutputItems{
                        Option = new AutocompleteOutputValue{
                            Value = e.Id.ToString(),
                            Key = e.Description
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
    }
}