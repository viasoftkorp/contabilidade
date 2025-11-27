using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Host.Controllers;

public class PlanoContaController : BaseController
{
    private readonly IPlanoContaService _planoContaService;

    public PlanoContaController(IPlanoContaService planoContaService)
    {
        _planoContaService = planoContaService;
    }

    [HttpGet("/contabilidade/conciliacao-contabil/plano-conta")]
    public async Task<IActionResult> GetAll([FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var contas = await _planoContaService.BuscarTodosPlanosConta(input);
        return Ok(contas);
    }
}