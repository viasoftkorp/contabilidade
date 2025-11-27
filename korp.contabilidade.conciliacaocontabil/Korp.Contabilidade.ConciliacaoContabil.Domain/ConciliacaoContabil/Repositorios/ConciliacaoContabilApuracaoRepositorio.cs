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

public class ConciliacaoContabilApuracaoRepositorio : IConciliacaoContabilApuracaoRepositorio, ITransientDependency
{
    private readonly IRepository<ConciliacaoContabilApuracao> _repository;
    private readonly IRepository<Empresa> _empresas;

    public ConciliacaoContabilApuracaoRepositorio(IRepository<ConciliacaoContabilApuracao> repository,
        IRepository<Empresa> empresas)
    {
        _repository = repository;
        _empresas = empresas;
    }

    public async Task<ListResultDto<BuscarConciliacaoContabilApuracaoOutput>> BuscarTodasApuracoesPorConciliacao(
        int legacyId, PagedFilteredAndSortedRequestInput input)
    {
        var query = _repository.Where(x => x.IdConciliacaoContabil == legacyId);

        if (string.IsNullOrEmpty(input.Sorting))
        {
            query = query.OrderByDescending(l => l.Data).ThenBy(a => a.Documento).ThenBy(a => a.Parcela);
        }

        var queryOutput = query
            .Join(_empresas,
                apuracao => apuracao.LegacyCompanyId,
                empresa => empresa.LegacyId,
                (apuracao, empresa) => new
                {
                    apuracao.LegacyId,
                    apuracao.LegacyCompanyId,
                    CompanyName = empresa.Nome,
                    apuracao.Data,
                    apuracao.Documento,
                    apuracao.Parcela,
                    apuracao.CodigoFornecedorCliente,
                    apuracao.RazaoSocialFornecedorCliente,
                    apuracao.Valor,
                    apuracao.Conciliado,
                    apuracao.Chave,
                    apuracao.IdConciliacaoContabil,
                    apuracao.DescricaoTipoValorApuracao,
                    apuracao.TipoValorApuracao
                    
                })
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var queryPaged = queryOutput.PageBy(input.SkipCount, input.MaxResultCount);
        var apuracaos = await queryPaged.AsNoTracking().ToListAsync();
        var apuracaosOutput = apuracaos.Select(a => new BuscarConciliacaoContabilApuracaoOutput
        {
            LegacyId = a.LegacyId,
            LegacyCompanyId = a.LegacyCompanyId,
            CompanyName = a.CompanyName,
            Data = a.Data,
            Documento = a.Documento,
            Parcela = a.Parcela,
            CodigoFornecedorCliente = a.CodigoFornecedorCliente,
            RazaoSocialFornecedorCliente = a.RazaoSocialFornecedorCliente,
            Valor = a.Valor,
            Conciliado = a.Conciliado,
            Chave = a.Chave,
            IdConciliacaoContabil = a.IdConciliacaoContabil,
            DescricaoTipoValorApuracao = a.DescricaoTipoValorApuracao,
            TipoValorApuracao = a.TipoValorApuracao
        }).ToList();

        return new ListResultDto<BuscarConciliacaoContabilApuracaoOutput>(apuracaosOutput);
    }
}