using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ServicosExternos.LegacyAdministration;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AmbientData.Abstractions;
using Viasoft.Core.AspNetCore.Controller;

namespace Korp.Contabilidade.ConciliacaoContabil.Host.Controllers;

[Route("administracao")]
public class LegacyAdministrationController : BaseController
{
    private readonly ILegacyAdministrationService _legacyAdministrationService;
    protected readonly IAmbientDataCallOptionsResolver AmbientDataCallOptionsResolver;

    public LegacyAdministrationController(ILegacyAdministrationService legacyAdministrationService, IAmbientDataCallOptionsResolver ambientDataCallOptionsResolver)
    {
        _legacyAdministrationService = legacyAdministrationService;
        AmbientDataCallOptionsResolver = ambientDataCallOptionsResolver;
    }
    
    [HttpGet("empresas")]
    public async Task<IActionResult> GetCompaniesByUser([FromQuery] string usuario)
    {
        var companies = await _legacyAdministrationService.GetCompaniesByUser(usuario, AmbientDataCallOptionsResolver.GetOptions());
        return Ok(companies);
    }
}