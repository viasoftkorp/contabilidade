using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;

public class PlanoContaRepositorio : IPlanoContaRepositorio, ITransientDependency
{
    private readonly IRepository<PlanoConta> _repository;
    private readonly IRepository<CtParametros> _ctParametros;
    private readonly IRepository<TipoConciliacaoContabilConta> _conciliacaoContabilContas;
    private readonly IRepository<Banco> _bancos;
    private readonly IRepository<Estoque> _estoque;
    private readonly IRepository<Fornecedor> _fornecedores;
    private readonly IRepository<Cliente> _clientes;

    public PlanoContaRepositorio(IRepository<PlanoConta> repository, IRepository<CtParametros> ctParametros,
        IRepository<TipoConciliacaoContabilConta> conciliacaoContabilContas, IRepository<Banco> bancos, IRepository<Estoque> estoque,
        IRepository<Fornecedor> fornecedores, IRepository<Cliente> clientes)
    {
        _repository = repository;
        _ctParametros = ctParametros;
        _conciliacaoContabilContas = conciliacaoContabilContas;
        _bancos = bancos;
        _estoque = estoque;
        _fornecedores = fornecedores;
        _clientes = clientes;
    }

    public async Task<ListResultDto<PlanoConta>> BuscarTodosPlanosConta(PagedFilteredAndSortedRequestInput input)
    {
        var normalizedFilter = input.Filter is null ? "" : input.Filter.Trim().ToLower();
        var query = _repository.WhereIf(!string.IsNullOrEmpty(normalizedFilter),
                p => p.Descricao.ToLower().Contains(normalizedFilter))
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        if (string.IsNullOrEmpty(input.Sorting))
        {
            query = query.OrderBy(p => p.Codigo);
        }

        var queryPaged = query.PageBy(input.SkipCount, input.MaxResultCount);
        var contas = await queryPaged.AsNoTracking().ToListAsync();

        return new ListResultDto<PlanoConta>(contas);
    }

    public async Task<bool> IsContaVirtual(int codigoConta)
    {
        var parametros = await _ctParametros.FirstOrDefaultAsync();
        var codigoContaString = codigoConta.ToString();
        return parametros is not null
               && (
                   parametros.Banco == codigoContaString
                   || parametros.Estoque == codigoContaString
                   || parametros.Fornecedor == codigoContaString
                   || parametros.PlanoCliente == codigoContaString
                   || parametros.ContaContabilAdiantamentoFornecedor == codigoConta
                   || parametros.ContaContabilAdiantamentoCliente == codigoConta
               );
    }

    public async Task<List<int>> GetContasVinculadasIds(int codigoConta, int? idTipoConciliacaoContabil = null)
    {
        var parametros = await _ctParametros.FirstOrDefaultAsync();
        if (parametros is null)
        {
            return new List<int>();
        }

        var contasJaInseridas = new List<int>();
        if (idTipoConciliacaoContabil.HasValue)
        {
            contasJaInseridas = await _conciliacaoContabilContas
                .Where(rateio => rateio.IdTipoConciliacaoContabil == idTipoConciliacaoContabil.Value)
                .Select(rateio => rateio.CodigoConta)
                .Distinct()
                .ToListAsync();
        }

        if (parametros.Banco == codigoConta.ToString())
        {
            var contasVinculadas = await _bancos
                .Select(b => b.CtaCredito)
                .Union(_bancos.Select(b => b.CtaDebito))
                .Where(conta => conta != null)
                .Select(conta => conta.Value)
                .WhereIf(contasJaInseridas.Any(), conta => !contasJaInseridas.Contains(conta))
                .Distinct()
                .ToListAsync();
            return contasVinculadas;
        }

        if (parametros.Estoque == codigoConta.ToString())
        {
            var contasVinculadas = await _estoque
                .Select(estoque => estoque.CodigoConta)
                .Where(conta => conta != null)
                .Select(conta => conta.Value)
                .WhereIf(contasJaInseridas.Any(), conta => !contasJaInseridas.Contains(conta))
                .Distinct()
                .ToListAsync();
            return contasVinculadas;
        }

        if (parametros.Fornecedor == codigoConta.ToString())
        {
            var contasVinculadas = await _fornecedores
                .Select(fornecedor => fornecedor.CodigoConta)
                .Where(conta => conta != null)
                .Select(conta => conta.Value)
                .WhereIf(contasJaInseridas.Any(), conta => !contasJaInseridas.Contains(conta))
                .Distinct()
                .ToListAsync();
            return contasVinculadas;
        }

        if (parametros.PlanoCliente == codigoConta.ToString())
        {
            var contasVinculadas = await _clientes
                .Select(cliente => cliente.CodigoConta)
                .Where(conta => conta != null)
                .Select(conta => conta.Value)
                .WhereIf(contasJaInseridas.Any(), conta => !contasJaInseridas.Contains(conta))
                .Distinct()
                .ToListAsync();
            return contasVinculadas;
        }

        if (parametros.ContaContabilAdiantamentoFornecedor == codigoConta)
        {
            var contasVinculadas = await _fornecedores
                .Select(fornecedor => fornecedor.ContaContabilAdiantamento)
                .Where(conta => conta != null)
                .Select(conta => conta.Value)
                .WhereIf(contasJaInseridas.Any(), conta => !contasJaInseridas.Contains(conta))
                .Distinct()
                .ToListAsync();
            return contasVinculadas;
        }

        if (parametros.ContaContabilAdiantamentoCliente == codigoConta)
        {
            var contasVinculadas = await _clientes
                .Select(cliente => cliente.ContaContabilAdiantamento)
                .Where(conta => conta != null)
                .Select(conta => conta.Value)
                .WhereIf(contasJaInseridas.Any(), conta => !contasJaInseridas.Contains(conta))
                .Distinct()
                .ToListAsync();
            return contasVinculadas;
        }

        return new List<int>();
    }

    public async Task<List<PlanoConta>> GetContasVinculadas(int codigoConta, int? idTipoConciliacaoContabil = null)
    {
        var codigoContas = await GetContasVinculadasIds(codigoConta, idTipoConciliacaoContabil);
        if (!codigoContas.Any())
        {
            return new List<PlanoConta>();
        }

        return await _repository
            .Where(p => codigoContas.Contains(p.Codigo))
            .ToListAsync();
    }
}