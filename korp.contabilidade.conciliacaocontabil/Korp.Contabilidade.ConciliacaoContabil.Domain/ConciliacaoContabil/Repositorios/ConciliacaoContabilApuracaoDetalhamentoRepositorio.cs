using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;

public class ConciliacaoContabilApuracaoDetalhamentoRepositorio : IConciliacaoContabilApuracaoDetalhamentoRepositorio, ITransientDependency
{
    private readonly IRepository<ConciliacaoContabilApuracaoDetalhamento> _repository;
    private readonly IRepository<PlanoConta> _planoContas;
    private readonly IRepository<TipoLancamento> _tipoLancamento;
    private readonly IRepository<Empresa> _empresas;
    private readonly IRepository<ConciliacaoContabilApuracao> _apuracoes;
    private readonly IFornecedorClienteService _fornecedorClienteService;

    public ConciliacaoContabilApuracaoDetalhamentoRepositorio(
        IRepository<ConciliacaoContabilApuracaoDetalhamento> repository, IRepository<PlanoConta> planoContas,
        IRepository<TipoLancamento> tipoLancamento, IRepository<Empresa> empresas, IRepository<ConciliacaoContabilApuracao> apuracoes,
        IFornecedorClienteService fornecedorClienteService)
    {
        _repository = repository;
        _planoContas = planoContas;
        _tipoLancamento = tipoLancamento;
        _empresas = empresas;
        _apuracoes = apuracoes;
        _fornecedorClienteService = fornecedorClienteService;
    }

    public async Task<ListResultDto<BuscarConciliacaoContabilApuracaoDetalhamentoOutput>>
        BuscarTodasApuracoesDetalhamentos(int idConciliacaoContabilApuracao, PagedFilteredAndSortedRequestInput input)
    {
        var query = _repository.Where(x => x.IdConciliacaoContabilApuracao == idConciliacaoContabilApuracao);

        if (string.IsNullOrEmpty(input.Sorting))
        {
            query = query.OrderByDescending(l => l.Data).ThenBy(l => l.NumeroLancamento);
        }

        var queryOutput = query
            .Join(
                _planoContas,
                apuracaoDetalhamento => apuracaoDetalhamento.CodigoConta,
                planoConta => planoConta.Codigo,
                (apuracaoDetalhamento, planoConta) => new {apuracaoDetalhamento, planoConta})
            .Join(
                _tipoLancamento,
                x => x.apuracaoDetalhamento.IdTipoLancamento, tipoLancamento => tipoLancamento.LegacyId,
                (x, tipoLancamento) => new {x.apuracaoDetalhamento, x.planoConta, tipoLancamento})
            .Join(
                _empresas,
                x => x.apuracaoDetalhamento.LegacyCompanyId,
                empresa => empresa.LegacyId,
                (x, empresa) => new
                {
                    x.apuracaoDetalhamento.LegacyId,
                    x.apuracaoDetalhamento.LegacyCompanyId,
                    CompanyName = empresa.Nome,
                    x.apuracaoDetalhamento.Data,
                    x.apuracaoDetalhamento.NumeroLancamento,
                    x.apuracaoDetalhamento.Historico,
                    x.apuracaoDetalhamento.Valor,
                    x.apuracaoDetalhamento.CodigoConta,
                    NomeConta = x.planoConta.Descricao,
                    x.apuracaoDetalhamento.CodigoFornecedorCliente,
                    x.apuracaoDetalhamento.IdConciliacaoContabilApuracao,
                    x.apuracaoDetalhamento.IdTipoLancamento,
                    CodigoTipoLancamento = x.tipoLancamento.Codigo,
                    DescricaoTipoLancamento = x.tipoLancamento.Descricao
                })
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var queryPaged = queryOutput.PageBy(input.SkipCount, input.MaxResultCount);
        var detalhamentos = await queryPaged.AsNoTracking().ToListAsync();
        var detalhamentosOutput = detalhamentos
            .Select(a => new BuscarConciliacaoContabilApuracaoDetalhamentoOutput
            {
                LegacyId = a.LegacyId,
                LegacyCompanyId = a.LegacyCompanyId,
                CompanyName = a.CompanyName,
                Data = a.Data,
                NumeroLancamento = a.NumeroLancamento,
                Historico = a.Historico,
                Valor = a.Valor,
                CodigoConta = a.CodigoConta,
                NomeConta = a.NomeConta,
                CodigoFornecedorCliente = a.CodigoFornecedorCliente,
                IdConciliacaoContabilApuracao = a.IdConciliacaoContabilApuracao,
                IdTipoLancamento = a.IdTipoLancamento,
                CodigoTipoLancamento = a.CodigoTipoLancamento,
                DescricaoTipoLancamento = a.DescricaoTipoLancamento
            }).ToList();

        return new ListResultDto<BuscarConciliacaoContabilApuracaoDetalhamentoOutput>(detalhamentosOutput);
    }

    public async Task<CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus> Create(ConciliacaoContabilApuracaoDetalhamentoInput input)
    {
        var detalhamento = new ConciliacaoContabilApuracaoDetalhamento(input);

        var hasValidIdApuracao = await _apuracoes.AnyAsync(l => l.LegacyId == input.IdConciliacaoContabilApuracao);
        if (!hasValidIdApuracao)
        {
            return CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.IdApuracaoInvalido;
        }

        var hasCodigoFornecedorClienteValid = await _fornecedorClienteService.IsCodigoValid(input.CodigoFornecedorCliente);
        if (!hasCodigoFornecedorClienteValid)
        {
            return CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.CodigoFornecedorClienteInvalido;
        }

        var empresa = await _empresas.FirstOrDefaultAsync(empresa => empresa.LegacyId == input.LegacyCompanyId);
        if (empresa is null)
        {
            return CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.EmpresaInvalida;
        }

        var planoConta = await _planoContas.FirstOrDefaultAsync(item => item.Codigo == input.CodigoConta);
        if (planoConta is null)
        {
            return CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.PlanoContaInvalido;
        }

        var tipoLancamento = await _tipoLancamento.FirstOrDefaultAsync(item => item.LegacyId == input.IdTipoLancamento);
        if (tipoLancamento is null)
        {
            return CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.TipoLancamentoInvalido;
        }

        var result = await _repository.InsertAsync(detalhamento, true);
        return CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.Ok;
    }

    public async Task<CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus> Update(ConciliacaoContabilApuracaoDetalhamentoInput input)
    {
        var detalhamento = await _repository.FirstOrDefaultAsync(d => d.LegacyId == input.LegacyId);
        if (detalhamento is null)
        {
            return CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.DetalhamentoNaoEncontrado;
        }

        var hasCodigoFornecedorClienteValid = await _fornecedorClienteService.IsCodigoValid(input.CodigoFornecedorCliente);
        if (!hasCodigoFornecedorClienteValid)
        {
            return CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.CodigoFornecedorClienteInvalido;
        }

        var empresa = await _empresas.FirstOrDefaultAsync(empresa => empresa.LegacyId == input.LegacyCompanyId);
        if (empresa is null)
        {
            return CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.EmpresaInvalida;
        }

        var planoConta = await _planoContas.FirstOrDefaultAsync(item => item.Codigo == input.CodigoConta);
        if (planoConta is null)
        {
            return CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.PlanoContaInvalido;
        }

        var tipoLancamento = await _tipoLancamento.FirstOrDefaultAsync(item => item.LegacyId == input.IdTipoLancamento);
        if (tipoLancamento is null)
        {
            return CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.TipoLancamentoInvalido;
        }

        detalhamento.Data = input.Data;
        detalhamento.NumeroLancamento = input.NumeroLancamento;
        detalhamento.Historico = input.Historico;
        detalhamento.Valor = input.Valor;
        detalhamento.CodigoConta = input.CodigoConta;
        detalhamento.CodigoFornecedorCliente = input.CodigoFornecedorCliente;
        detalhamento.IdTipoLancamento = input.IdTipoLancamento;

        var result = await _repository.UpdateAsync(detalhamento, true);
        return CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.Ok;
    }
}