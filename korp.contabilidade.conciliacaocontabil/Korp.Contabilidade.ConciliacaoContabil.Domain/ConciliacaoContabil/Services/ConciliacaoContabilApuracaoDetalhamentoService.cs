using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;

public class ConciliacaoContabilApuracaoDetalhamentoService: IConciliacaoContabilApuracaoDetalhamentoService, ITransientDependency
{
    private readonly IConciliacaoContabilApuracaoDetalhamentoRepositorio _conciliacaoContabilApuracaoDetalhamentoRepositorio;

    public ConciliacaoContabilApuracaoDetalhamentoService(IConciliacaoContabilApuracaoDetalhamentoRepositorio conciliacaoContabilApuracaoDetalhamentoRepositorio)
    {
        _conciliacaoContabilApuracaoDetalhamentoRepositorio = conciliacaoContabilApuracaoDetalhamentoRepositorio;
    }

    public async Task<ListResultDto<BuscarConciliacaoContabilApuracaoDetalhamentoOutput>> BuscarTodasApuracoesDetalhamentos(int idConciliacaoContabilApuracao, PagedFilteredAndSortedRequestInput input)
    {
        return await _conciliacaoContabilApuracaoDetalhamentoRepositorio.BuscarTodasApuracoesDetalhamentos(idConciliacaoContabilApuracao, input);
    }

    public async Task<CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus> Create(ConciliacaoContabilApuracaoDetalhamentoInput input)
    {
        return await _conciliacaoContabilApuracaoDetalhamentoRepositorio.Create(input);
    }

    public async Task<CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus> Update(ConciliacaoContabilApuracaoDetalhamentoInput input)
    {
        return await _conciliacaoContabilApuracaoDetalhamentoRepositorio.Update(input);
    }
}