using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;

public class FornecedorClienteService : IFornecedorClienteService, ITransientDependency
{
    private IRepository<Fornecedor> _fornecedores;
    private IRepository<Cliente> _clientes;

    public FornecedorClienteService(IRepository<Fornecedor> fornecedores, IRepository<Cliente> clientes)
    {
        _fornecedores = fornecedores;
        _clientes = clientes;
    }

    public async Task<PagedResultDto<FornecedorClienteCodigoOutput>> GetAllCodigos(GetAllFornecedorClienteCodigoInput input)
    {
        var query = _fornecedores
            .WhereIf(!string.IsNullOrEmpty(input.Filter), f => f.Codigo.Contains(input.Filter))
            .Select(f => new {f.Codigo, f.RazaoSocial})
            .Union(
                _clientes
                    .WhereIf(!string.IsNullOrEmpty(input.Filter), f => f.Codigo.Contains(input.Filter))
                    .Select(c => new {c.Codigo, c.RazaoSocial})
            )
            .Select(item => new FornecedorClienteCodigoOutput
            {
                RazaoSocial = item.RazaoSocial,
                Codigo = item.Codigo
            });
        var totalCount = await query.CountAsync();
        var items = await query.PageBy(input.SkipCount, input.MaxResultCount).ToListAsync();
        return new PagedResultDto<FornecedorClienteCodigoOutput>(totalCount, items);
    }

    public async Task<bool> IsCodigoValid(string codigo)
    {
        return await _fornecedores
            .Where(f => f.Codigo == codigo).Select(c => c.Codigo)
            .Union(_clientes.Where(f => f.Codigo == codigo).Select(c => c.Codigo))
            .AnyAsync();
    }
}