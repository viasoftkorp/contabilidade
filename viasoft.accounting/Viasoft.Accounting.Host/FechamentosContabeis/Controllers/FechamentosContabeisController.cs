using System;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Accounting.Host.PeriodosContabeis.Controllers.Dtos;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;

namespace Viasoft.Accounting.Host.FechamentosContabeis.Controllers;
    [ApiVersion(2023.4), ApiVersion(2023.3), ApiVersion(2023.2), ApiVersion(2023.1), ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("fechamentos-contabeis")]
public class FechamentosContabeisController : BaseController
{
    private readonly IRepository<LegacyFechamentoPeriodoContabil> _legacyFechamentoPeriodoContabils;
    private readonly ICurrentCompany _currentCompany;

    public FechamentosContabeisController(IRepository<LegacyFechamentoPeriodoContabil> legacyFechamentoPeriodoContabils,
        ICurrentCompany currentCompany)
    {
        _legacyFechamentoPeriodoContabils = legacyFechamentoPeriodoContabils;
        _currentCompany = currentCompany;
    }

    [HttpGet]
    public async Task<PagedResultDto<FechamentoContabilModel>> GetAll([FromQuery] GetAllFechamentoContabilInput input)
    {
        var legacyIdEmpresa = _currentCompany.LegacyId;
        var query = _legacyFechamentoPeriodoContabils
            .AsNoTracking()
            .Where(fechamento => fechamento.LegacyIdEmpresa == legacyIdEmpresa)
            .WhereIf(input.DataLancamento.HasValue,
                fechamento => fechamento.Data >= input.DataLancamento.Value)
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var totalCount = await query.CountAsync();
        var fechamentos = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .Select(fechamento => new FechamentoContabilModel(fechamento))
            .ToListAsync();
        return new PagedResultDto<FechamentoContabilModel>(totalCount, fechamentos);
    }
}