using System;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;

public class ConciliacaoContabilLancamentoDetalhamentoRepositorio : IConciliacaoContabilLancamentoDetalhamentoRepositorio, ITransientDependency
{
    private readonly IRepository<ConciliacaoContabilLancamentoDetalhamento> _repository;
    private readonly IRepository<Empresa> _empresas;
    private readonly IRepository<ConciliacaoContabilLancamento> _lancamentos;

    public ConciliacaoContabilLancamentoDetalhamentoRepositorio(
        IRepository<ConciliacaoContabilLancamentoDetalhamento> repository, IRepository<Empresa> empresas, IRepository<ConciliacaoContabilLancamento> lancamentos)
    {
        _repository = repository;
        _empresas = empresas;
        _lancamentos = lancamentos;
    }

    public async Task<ListResultDto<BuscarConciliacaoContabilLancamentoDetalhamentoOutput>>
        BuscarTodosLancamentosDetalhamentos(int idConciliacaoContabilLancamento, PagedFilteredAndSortedRequestInput input)
    {
        var query = _repository.Where(x => x.IdConciliacaoContabilLancamento == idConciliacaoContabilLancamento);

        if (string.IsNullOrEmpty(input.Sorting))
        {
            query = query.OrderByDescending(l => l.Data).ThenBy(l => l.Documento).ThenBy(l => l.Parcela);
        }

        var queryOutput = query
            .Join(
                _empresas,
                lancamentoDetalhamento => lancamentoDetalhamento.LegacyCompanyId, empresa => empresa.LegacyId,
                (lancamentoDetalhamento, empresa) => new
                {
                    lancamentoDetalhamento.LegacyId,
                    lancamentoDetalhamento.LegacyCompanyId,
                    CompanyName = empresa.Nome,
                    lancamentoDetalhamento.Data,
                    lancamentoDetalhamento.Documento,
                    lancamentoDetalhamento.Parcela,
                    lancamentoDetalhamento.CodigoFornecedorCliente,
                    lancamentoDetalhamento.RazaoSocialFornecedorCliente,
                    lancamentoDetalhamento.Valor,
                    lancamentoDetalhamento.IdConciliacaoContabilLancamento,
                    lancamentoDetalhamento.TipoValorApuracao,
                    lancamentoDetalhamento.DescricaoTipoValorApuracao
                })
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var queryPaged = queryOutput.PageBy(input.SkipCount, input.MaxResultCount);
        var detalhamentos = await queryPaged.AsNoTracking().ToListAsync();
        var detalhamentosOutput = detalhamentos
            .Select(x => new BuscarConciliacaoContabilLancamentoDetalhamentoOutput
            {
                LegacyId = x.LegacyId,
                LegacyCompanyId = x.LegacyCompanyId,
                CompanyName = x.CompanyName,
                Data = x.Data,
                Documento = x.Documento,
                Parcela = x.Parcela,
                CodigoFornecedorCliente = x.CodigoFornecedorCliente,
                RazaoSocialFornecedorCliente = x.RazaoSocialFornecedorCliente,
                Valor = x.Valor,
                IdConciliacaoContabilLancamento = x.IdConciliacaoContabilLancamento,
                TipoValorApuracao = x.TipoValorApuracao,
                DescricaoTipoValorApuracao = x.DescricaoTipoValorApuracao
            }).ToList();

        return new ListResultDto<BuscarConciliacaoContabilLancamentoDetalhamentoOutput>(detalhamentosOutput);
    }

    public async Task<CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus> Create(ConciliacaoContabilLancamentoDetalhamentoInput input)
    {
        var detalhamento = new ConciliacaoContabilLancamentoDetalhamento(input);

        var hasValidTipoValorApuracao = Enum.IsDefined(typeof(TipoValorApuracaoConciliacaoContabil), detalhamento.TipoValorApuracao);
        if (!hasValidTipoValorApuracao)
        {
            return CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus.TipoApuracaoInvalido;
        }
        
        var empresa = await _empresas.FirstOrDefaultAsync(empresa => empresa.LegacyId == detalhamento.LegacyCompanyId);
        if (empresa is null)
        {
            return CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus.EmpresaInvalida;
        }

        var hasValidIdLancamento = await _lancamentos.AnyAsync(l => l.LegacyId == detalhamento.IdConciliacaoContabilLancamento);
        if (!hasValidIdLancamento)
        {
            return CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus.IdLancamentoInvalido;
        }

        var result = await _repository.InsertAsync(detalhamento, true);
        return CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus.Ok;
    }

    public async Task<CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus> Update(ConciliacaoContabilLancamentoDetalhamentoInput input)
    {
        var detalhamento = await _repository.FirstOrDefaultAsync(d => d.LegacyId == input.LegacyId);
        if (detalhamento is null)
        {
            return CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus.DetalhamentoNaoEncontrado;
        }

        var hasValidTipoValorApuracao = Enum.IsDefined(typeof(TipoValorApuracaoConciliacaoContabil), detalhamento.TipoValorApuracao);
        if (!hasValidTipoValorApuracao)
        {
            return CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus.TipoApuracaoInvalido;
        }

        var empresa = await _empresas.FirstOrDefaultAsync(empresa => empresa.LegacyId == input.LegacyCompanyId);
        if (empresa is null)
        {
            return CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus.EmpresaInvalida;
        }

        var hasValidIdLancamento = await _lancamentos.AnyAsync(l => l.LegacyId == detalhamento.IdConciliacaoContabilLancamento);
        if (!hasValidIdLancamento)
        {
            return CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus.IdLancamentoInvalido;
        }

        detalhamento.Data = input.Data;
        detalhamento.Documento = input.Documento;
        detalhamento.Parcela = input.Parcela;
        detalhamento.Valor = input.Valor;
        detalhamento.TipoValorApuracao = input.TipoValorApuracao;
        detalhamento.DescricaoTipoValorApuracao = input.DescricaoTipoValorApuracao;
        var result = await _repository.UpdateAsync(detalhamento, true);
        return CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus.Ok;
    }
}