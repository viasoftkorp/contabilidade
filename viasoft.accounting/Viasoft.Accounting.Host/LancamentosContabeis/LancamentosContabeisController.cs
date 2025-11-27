using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Accounting.Domain.Dtos;
using Viasoft.Accounting.Domain.Services;
using Viasoft.Core.AspNetCore.Controller;

namespace Viasoft.Accounting.Host.LancamentosContabeis;

    [ApiVersion(2023.4), ApiVersion(2023.3), ApiVersion(2023.2), ApiVersion(2023.1), ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
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