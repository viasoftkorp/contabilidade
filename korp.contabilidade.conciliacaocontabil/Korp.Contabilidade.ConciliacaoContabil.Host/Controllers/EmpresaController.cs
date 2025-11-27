using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Host.Controllers;

public class EmpresaController : BaseController
{
    private readonly IEmpresaService _empresasService;

    public EmpresaController(IEmpresaService empresaService)
    {
        _empresasService = empresaService;
    }

    [HttpGet("/contabilidade/conciliacao-contabil/empresas")]
    public async Task<PagedResultDto<EmpresaDto>> GetAllEmpresas([FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var empresas = await _empresasService.GetAllEmpresas(input);
        return empresas;
    }
}