using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Host.Controllers;

public class ConciliacaoContabilApuracaoDetalhamentoController : BaseController
{
    private readonly IConciliacaoContabilApuracaoDetalhamentoService _conciliacaoContabilApuracaoDetalhamentoService;

    public ConciliacaoContabilApuracaoDetalhamentoController(
        IConciliacaoContabilApuracaoDetalhamentoService conciliacaoContabilApuracaoDetalhamentoService)
    {
        _conciliacaoContabilApuracaoDetalhamentoService = conciliacaoContabilApuracaoDetalhamentoService;
    }

    [HttpGet("/contabilidade/conciliacao-contabil/apuracao-detalhamento/{idConciliacaoContabilApuracao}")]
    public async Task<IActionResult> BuscarTodasApuracaoDetalhamentos([FromRoute] int idConciliacaoContabilApuracao,
        [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var detalhamentos =
            await _conciliacaoContabilApuracaoDetalhamentoService.BuscarTodasApuracoesDetalhamentos(
                idConciliacaoContabilApuracao, input);
        return Ok(detalhamentos);
    }

    [HttpPost("/contabilidade/conciliacao-contabil/apuracao-detalhamento")]
    public async Task<CreateOrEditConciliacaoContabilApuracaoDetalhamentoOutput> CreateLancamentoDetalhamentos([FromBody] ConciliacaoContabilApuracaoDetalhamentoInput input)
    {
        var detalhamentoStatus = await _conciliacaoContabilApuracaoDetalhamentoService.Create(input);
        return new CreateOrEditConciliacaoContabilApuracaoDetalhamentoOutput
        {
            Status = detalhamentoStatus
        };
    }

    [HttpPut("/contabilidade/conciliacao-contabil/apuracao-detalhamento/{id:int}")]
    public async Task<CreateOrEditConciliacaoContabilApuracaoDetalhamentoOutput> UpdateLancamentoDetalhamentos([FromRoute] int id, [FromBody] ConciliacaoContabilApuracaoDetalhamentoInput input)
    {
        input.LegacyId = id;
        var detalhamentoStatus = await _conciliacaoContabilApuracaoDetalhamentoService.Update(input);
        return new CreateOrEditConciliacaoContabilApuracaoDetalhamentoOutput
        {
            Status = detalhamentoStatus
        };
    }
}