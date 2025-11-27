using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Host.Controllers;

public class ConciliacaoContabilLancamentoDetalhamentoController : BaseController
{
    private readonly IConciliacaoContabilLancamentoDetalhamentoService _conciliacaoContabilLancamentoDetalhamentoService;

    public ConciliacaoContabilLancamentoDetalhamentoController(
        IConciliacaoContabilLancamentoDetalhamentoService conciliacaoContabilLancamentoDetalhamentoService)
    {
        _conciliacaoContabilLancamentoDetalhamentoService = conciliacaoContabilLancamentoDetalhamentoService;
    }

    [HttpGet("/contabilidade/conciliacao-contabil/lancamento-detalhamento/{idConciliacaoContabilLancamento}")]
    public async Task<IActionResult> BuscarTodosLancamentoDetalhamentos([FromRoute] int idConciliacaoContabilLancamento,
        [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var detalhamentos =
            await _conciliacaoContabilLancamentoDetalhamentoService.BuscarTodosLancamentosDetalhamentos(
                idConciliacaoContabilLancamento, input);
        return Ok(detalhamentos);
    }

    [HttpPost("/contabilidade/conciliacao-contabil/lancamento-detalhamento")]
    public async Task<CreateOrEditConciliacaoContabilLancamentoDetalhamentoOutput> CreateLancamentoDetalhamentos([FromBody] ConciliacaoContabilLancamentoDetalhamentoInput input)
    {
        var detalhamentoStatus = await _conciliacaoContabilLancamentoDetalhamentoService.Create(input);
        return new CreateOrEditConciliacaoContabilLancamentoDetalhamentoOutput
        {
            Status = detalhamentoStatus
        };
    }

    [HttpPut("/contabilidade/conciliacao-contabil/lancamento-detalhamento/{id:int}")]
    public async Task<CreateOrEditConciliacaoContabilLancamentoDetalhamentoOutput> UpdateLancamentoDetalhamentos([FromRoute] int id, [FromBody] ConciliacaoContabilLancamentoDetalhamentoInput input)
    {
        input.LegacyId = id;
        var detalhamentoStatus = await _conciliacaoContabilLancamentoDetalhamentoService.Update(input);
        return new CreateOrEditConciliacaoContabilLancamentoDetalhamentoOutput
        {
            Status = detalhamentoStatus
        };
    }
}