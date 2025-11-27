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
using Viasoft.Accounting.Host.Controllers.Inputs;
using Viasoft.Accounting.Host.Controllers.Inputs.Gets;
using Viasoft.Accounting.Host.Controllers.Outputs;
using Viasoft.Accounting.Host.Services;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;

namespace Viasoft.Accounting.Host.Controllers;

[ApiVersion(2023.4), ApiVersion(2023.3), ApiVersion(2023.2), ApiVersion(2023.1), ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
public class BookkeepingController : BaseReadonlyForSelectController<BookkeepingAccount, BookkeepingAccountOutput, GetAllBokkepingAccountsInput, GetAllBokkepingAccountsInput, BookkeepingAccountOutput, int>
{
    private readonly IBookkeepingAccountService _bookkeepingAccountService;

    public BookkeepingController(IReadOnlyRepository<BookkeepingAccount> repository, IMapper mapper, IBookkeepingAccountService bookkeepingAccountService) : base(repository, mapper)
    {
        _bookkeepingAccountService = bookkeepingAccountService;
    }

    protected override (Expression<Func<BookkeepingAccount, int>>, bool) DefaultGetAllSorting()
    {
        return (e => e.Code, true);
    }

    protected override IQueryable<BookkeepingAccount> ApplyCustomFilters(IQueryable<BookkeepingAccount> query, GetAllBokkepingAccountsInput input)
    {
        return query
            .WhereIf(!input.IgnoreSyntheticFilter, e => input.Synthetic ? e.IsSynthetic == "S" : e.IsSynthetic == "N")
            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.Name.Contains(input.Filter)
            || e.Code.ToString().Contains(input.Filter) || e.Classification.Contains(input.Filter));
    }

    [HttpPost]
    public async Task Create(BookkeepingAccountInput input)
    {
        await _bookkeepingAccountService.Create(input);
    }

    [HttpGet]
    public async Task<BookkeepingAccountOutput> GetByCode(int code)
    {
        var representativeMapping = await Repository.Where(e => e.Code == code)
            .Select(conta => new BookkeepingAccountOutput(conta))
            .FirstOrDefaultAsync();

        return representativeMapping;
    }

    [HttpPost]
    public async Task<Dictionary<int, NameValueDto>> GetBookkeepingNamesByCodes(List<int> codes)
    {
        var representativeMapping = await Repository.Where(e => codes.Contains(e.Code))
            .ToDictionaryAsync(e => e.Code, e => new NameValueDto
            {
                Id = e.Id,
                Name = e.Name
            });

        return representativeMapping;
    }

    [HttpGet]
    public async Task<IdCodeDto> GetBookkeepingCodeById([FromQuery] Guid id)
    {
        var representative = await Repository.Select(e => new IdCodeDto
        {
            Id = e.Id,
            Code = e.Code
        }).FirstOrDefaultAsync(e => e.Id == id);

        return representative;
    }
}
