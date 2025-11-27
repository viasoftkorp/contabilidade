using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Host.Controllers;

public class TipoLancamentoController : BaseController
{
    private readonly ITipoLancamentoService _tipoLancamentoService;

    public TipoLancamentoController(ITipoLancamentoService tipoLancamentoService)
    {
        _tipoLancamentoService = tipoLancamentoService;
    }

    [HttpGet("/contabilidade/conciliacao-contabil/tipo-lancamentos")]
    public async Task<PagedResultDto<TipoLancamentoDto>> GetAllTipoLancamentos([FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var tipoLancamentos = await _tipoLancamentoService.GetAllTipoLancamentos(input);
        return tipoLancamentos;
    }
}