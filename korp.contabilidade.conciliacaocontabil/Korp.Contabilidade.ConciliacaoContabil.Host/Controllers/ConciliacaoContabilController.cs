using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApplicationService;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;

namespace Korp.Contabilidade.ConciliacaoContabil.Host.Controllers;

[Route("")]
public class ConciliacaoContabilController: BaseController
{
    private readonly IConciliacaoContabilApplicationService _applicationService;
    
    public ConciliacaoContabilController(IConciliacaoContabilApplicationService applicationService)
    {
        _applicationService = applicationService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CriarConciliacaoContabil(CriarConciliacaoContabilInput input)
    {
        ConciliacaoContabilOutput output = await _applicationService.CriarConciliacaoContabil(input);
        
        return Accepted($"/contabilidade/conciliacao-contabil/", output);
    }
    
    [HttpPut]
    public async Task<IActionResult> AtualizarConciliacaoContabil(AtualizarConciliacaoContabilInput input)
    {
        await _applicationService.AtualizarConciliacaoContabil(input);
        
        return Accepted($"/contabilidade/conciliacao-contabil/", null);
    }
    
    [HttpDelete("{legacyId}")]
    public async Task<IActionResult> DeletarConciliacaoContabil([FromRoute] int legacyId)
    {
        await _applicationService.DeletarConciliacaoContabil(legacyId);
        
        return Accepted($"/contabilidade/conciliacao-contabil/", null);
    }
    
    [HttpGet("{legacyId}")]
    public async Task<IActionResult> BuscarConciliacaoContabil([FromRoute] int legacyId)
    {
        var conciliacao = await _applicationService.BuscarConciliacaoContabilPorLegacyId(legacyId);
        if (conciliacao is null)
        { 
            return NotFound(new ConciliacaoContabilErro { Codigo = ConciliacaoContabilErro.RecursoNaoEncontrado, Mensagem = "Conciliação contábil não encontrada " });
        }
        
        return Ok(conciliacao);
    }
    
    [HttpGet]
    public async Task<IActionResult> BuscarTodasConciliacoesContabeis([FromQuery] BuscarTodasConciliacoesContabeisInput input)
    {
        var conciliacoes = await _applicationService.BuscarTodasConciliacoesContabeis(input);
        return Ok(conciliacoes);
    }
}