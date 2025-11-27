using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;

public class ConciliacaoContabilLancamentoRepositorio : IConciliacaoContabilLancamentoRepositorio, ITransientDependency
{
    private readonly IRepository<ConciliacaoContabilLancamento> _repository;
    private readonly IRepository<PlanoConta> _planoContas;
    private readonly IRepository<TipoLancamento> _tipoLancamento;
    private readonly IRepository<Empresa> _empresas;
    private readonly IRepository<Fornecedor> _fornecedor;

    public ConciliacaoContabilLancamentoRepositorio(IRepository<ConciliacaoContabilLancamento> repository,
        IRepository<PlanoConta> planoContas, IRepository<TipoLancamento> tipoLancamento, IRepository<Empresa> empresas, IRepository<Fornecedor> fornecedor)
    {
        _repository = repository;
        _planoContas = planoContas;
        _tipoLancamento = tipoLancamento;
        _empresas = empresas;
        _fornecedor = fornecedor;
    }

    public async Task<ListResultDto<BuscarConciliacaoContabilLancamentoOutput>> BuscarTodosLancamentosPorConciliacao(
        int legacyId,
        PagedFilteredAndSortedRequestInput input)
    {
        var query = _repository.Where(x => x.IdConciliacaoContabil == legacyId);

        if (string.IsNullOrEmpty(input.Sorting))
        {
            query = query.OrderByDescending(l => l.Data).ThenBy(l => l.NumeroLancamento);
        }
        
        var queryOutput = query
            .GroupJoin(_planoContas,
                lancamento => lancamento.CodigoConta,
                planoConta => planoConta.Codigo,
                (lancamento, planoContas) => new { lancamento, planoContas })
            .SelectMany(
                x => x.planoContas.DefaultIfEmpty(),
                (x, planoConta) => new { x.lancamento, planoConta })
            .GroupJoin(_tipoLancamento,
                x => x.lancamento.IdTipoLancamento,
                tipoLancamento => tipoLancamento.LegacyId,
                (x, tipoLancamentos) => new { x.lancamento, x.planoConta, tipoLancamentos })
            .SelectMany(
                x => x.tipoLancamentos.DefaultIfEmpty(),
                (x, tipoLancamento) => new { x.lancamento, x.planoConta, tipoLancamento })
            .GroupJoin(_fornecedor,
                x => x.lancamento.CodigoFornecedorCliente,
                fornecedor => fornecedor.Codigo,
                (x, fornecedores) => new { x.lancamento, x.planoConta, x.tipoLancamento, fornecedores })
            .SelectMany(
                x => x.fornecedores.DefaultIfEmpty(),
                (x, fornecedor) => new { x.lancamento, x.planoConta, x.tipoLancamento, fornecedor })
            .GroupJoin(_empresas,
                x => x.lancamento.LegacyCompanyId,
                empresa => empresa.LegacyId,
                (x, empresas) => new { x.lancamento, x.planoConta, x.tipoLancamento, x.fornecedor, empresas })
            .SelectMany(
                x => x.empresas.DefaultIfEmpty(),
                (x, empresa) => new
                {
                    x.lancamento.LegacyId,
                    x.lancamento.LegacyCompanyId,
                    CompanyName = empresa != null ? empresa.Nome : null,
                    x.lancamento.Data,
                    x.lancamento.NumeroLancamento,
                    x.lancamento.Historico,
                    x.lancamento.Valor,
                    x.lancamento.CodigoConta,
                    NomeConta = x.planoConta != null ? x.planoConta.Descricao : null,
                    x.lancamento.Conciliado,
                    x.lancamento.Chave,
                    x.lancamento.IdConciliacaoContabil,
                    x.lancamento.CodigoFornecedorCliente,
                    RazaoSocial = x.fornecedor != null ? x.fornecedor.RazaoSocial : null,
                    x.lancamento.IdTipoLancamento,
                    CodigoTipoLancamento = x.tipoLancamento != null ? x.tipoLancamento.Codigo : null,
                    DescricaoTipoLancamento = x.tipoLancamento != null ? x.tipoLancamento.Descricao : null
                })
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var queryPaged = queryOutput.PageBy(input.SkipCount, input.MaxResultCount);
        var lancamentos = await queryPaged.AsNoTracking().ToListAsync();
        var lancamentosOutput = lancamentos
            .Select(l => new BuscarConciliacaoContabilLancamentoOutput
            {
                LegacyId = l.LegacyId,
                LegacyCompanyId = l.LegacyCompanyId,
                CompanyName = l.CompanyName,
                Data = l.Data,
                NumeroLancamento = l.NumeroLancamento,
                Historico = l.Historico,
                Valor = l.Valor,
                CodigoConta = l.CodigoConta,
                NomeConta = l.NomeConta,
                Conciliado = l.Conciliado,
                Chave = l.Chave,
                IdConciliacaoContabil = l.IdConciliacaoContabil,
                CodigoFornecedorCliente = l.CodigoFornecedorCliente,
                RazaoSocialFornecedorCliente = l.RazaoSocial,
                IdTipoLancamento = l.IdTipoLancamento,
                CodigoTipoLancamento = l.CodigoTipoLancamento,
                DescricaoTipoLancamento = l.DescricaoTipoLancamento
            }).ToList();
        
        return new ListResultDto<BuscarConciliacaoContabilLancamentoOutput>(lancamentosOutput);
    }
}