using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Accounting.Domain.Dtos;
using Viasoft.Accounting.Domain.Services;
using Viasoft.Core.AspNetCore.Controller;

namespace Viasoft.Accounting.Host.LancamentosContabeis;

[ApiVersion(2023.4), ApiVersion(2023.3), ApiVersion(2023.2), ApiVersion(2023.1), ApiVersion(2022.4)]
[Route("lancamentos-contabeis")]
public class LancamentosContabeisController : BaseController
{
    private readonly IAccountingEntriesService _accountingEntriesService;

    public LancamentosContabeisController(IAccountingEntriesService accountingEntriesService)
    {
        _accountingEntriesService = accountingEntriesService;
    }

    [HttpDelete("{idOrigem}")]
    public async Task<IActionResult> Estornar(Guid idOrigem)
    {
        var result = await _accountingEntriesService.Estornar(idOrigem);
        if (result == MotivoEstornarLancamentoContabilResult.Ok)
        {
            return Ok(result);
        }

        return UnprocessableEntity(result);
    }
}