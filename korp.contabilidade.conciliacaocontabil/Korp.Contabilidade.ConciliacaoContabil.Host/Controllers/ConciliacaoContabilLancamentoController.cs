using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Host.Controllers;

public class ConciliacaoContabilLancamentoController : BaseController, ITransientDependency
{
    private readonly IConciliacaoContabilLancamentoService _conciliacaoContabilLancamentoService;

    public ConciliacaoContabilLancamentoController(IConciliacaoContabilLancamentoService conciliacaoContabilLancamentoService)
    {
        _conciliacaoContabilLancamentoService = conciliacaoContabilLancamentoService;
    }
    
    [HttpGet("/contabilidade/conciliacao-contabil/lancamento/{legacyId}")]
    public async Task<IActionResult> BuscarTodosLancamentosPorConciliacao([FromRoute] int legacyId, [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var lancamentos = await _conciliacaoContabilLancamentoService.BuscarTodosLancamentosPorConciliacao(legacyId, input);
        return Ok(lancamentos);
    }
}