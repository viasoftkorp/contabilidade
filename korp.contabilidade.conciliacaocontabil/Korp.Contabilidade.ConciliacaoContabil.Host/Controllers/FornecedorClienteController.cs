using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Host.Controllers;

public class FornecedorClienteController : BaseController
{
    private readonly IFornecedorClienteService _fornecedorClienteService;

    public FornecedorClienteController(IFornecedorClienteService fornecedorClienteService)
    {
        _fornecedorClienteService = fornecedorClienteService;
    }

    [HttpGet("/contabilidade/conciliacao-contabil/fornecedor-cliente/codigos")]
    public async Task<PagedResultDto<FornecedorClienteCodigoOutput>> GetAllCodigos([FromQuery] GetAllFornecedorClienteCodigoInput input)
    {
        return await _fornecedorClienteService.GetAllCodigos(input);
    }
}