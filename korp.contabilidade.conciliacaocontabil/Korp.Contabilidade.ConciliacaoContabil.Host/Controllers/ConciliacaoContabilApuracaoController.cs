using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Host.Controllers;

public class ConciliacaoContabilApuracaoController : BaseController
{
    private readonly IConciliacaoContabilApuracaoService _conciliacaoContabilApuracaoService;

    public ConciliacaoContabilApuracaoController(IConciliacaoContabilApuracaoService conciliacaoContabilApuracaoService)
    {
        _conciliacaoContabilApuracaoService = conciliacaoContabilApuracaoService;
    }

    [HttpGet("/contabilidade/conciliacao-contabil/apuracao/{legacyId}")]
    public async Task<IActionResult> BuscarTodasApuracoesPorConciliacao([FromRoute] int legacyId,
        [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var apuracoes = await _conciliacaoContabilApuracaoService.BuscarTodasApuracoesPorConciliacao(legacyId, input);
        return Ok(apuracoes);
    }
}