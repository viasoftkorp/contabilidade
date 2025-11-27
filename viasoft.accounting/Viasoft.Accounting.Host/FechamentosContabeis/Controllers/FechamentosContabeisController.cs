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
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;

namespace Viasoft.Accounting.Host.FechamentosContabeis.Controllers;
[ApiVersion(2023.4), ApiVersion(2023.3), ApiVersion(2023.2), ApiVersion(2023.1), ApiVersion(2022.4)]
[Route("fechamentos-contabeis")]
public class FechamentosContabeisController : BaseController
{
    private readonly IRepository<LegacyFechamentoPeriodoContabil> _legacyFechamentoPeriodoContabils;

    public FechamentosContabeisController(IRepository<LegacyFechamentoPeriodoContabil> legacyFechamentoPeriodoContabils)
    {
        _legacyFechamentoPeriodoContabils = legacyFechamentoPeriodoContabils;
    }

    [HttpGet]
    public async Task<PagedResultDto<FechamentoContabilModel>> GetAll([FromQuery] GetAllFechamentoContabilInput input)
    {
        var query = _legacyFechamentoPeriodoContabils
            .AsNoTracking()
            .WhereIf(input.DataLancamento.HasValue,
                fechamento => string.Compare(fechamento.Data, input.DataLancamento.Value.ToString("yyyyMMdd")) >=
                              0)
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var totalCount = await query.CountAsync();
        var fechamentos = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .Select(fechamento => new FechamentoContabilModel(fechamento))
            .ToListAsync();
        return new PagedResultDto<FechamentoContabilModel>(totalCount, fechamentos);
    }
}