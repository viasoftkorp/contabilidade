using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Accounting.Host.Configuracoes.Dtos;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Repositories;
using System.Linq;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;


namespace Viasoft.Accounting.Host.Configuracoes.Controllers;

[ApiVersion(2023.4), ApiVersion(2023.3), ApiVersion(2023.2), ApiVersion(2023.1), ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("configuracoes")]
public class ConfiguracaoController : BaseController
{
    private readonly IRepository<Configuracao> _configuracaos;
    private readonly IRepository<AccountingOperation> _accountingOperations;
    private readonly IRepository<BookkeepingAccount> _bookkeepingAccounts;

    public ConfiguracaoController(
        IRepository<Configuracao> configuracaos,
        IRepository<AccountingOperation> accountingOperations,
        IRepository<BookkeepingAccount> bookkeepingAccounts)
    {
        _configuracaos = configuracaos;
        _accountingOperations = accountingOperations;
        _bookkeepingAccounts = bookkeepingAccounts;
    }

    [HttpGet]
    public async Task<ConfiguracaoDto> Get()
    {
        var configuracao = await (from config in _configuracaos.AsNoTracking()
            join operacaoContabilAdiantamento in _accountingOperations.AsNoTracking()
                on config.CodigoOperacaoContabilAdiantamento equals operacaoContabilAdiantamento.Code
            join bookkeepingAccount in _bookkeepingAccounts.AsNoTracking()
                on config.CodigoContaContabilPai equals bookkeepingAccount.Code
                into bookkepingAccounts
            from bookkeepingAccount in bookkepingAccounts.DefaultIfEmpty()
            select new ConfiguracaoDto
            {
                Id = config.Id,
                IdOperacaoContabilAdiantamento = operacaoContabilAdiantamento.Id,
                IdContaContabilPai = bookkeepingAccount != null ? bookkeepingAccount.Id : null
            }).FirstAsync();

        return configuracao;
    }
}
