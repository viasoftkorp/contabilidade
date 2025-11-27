using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;

public class ConciliacaoContabilApuracaoService: IConciliacaoContabilApuracaoService, ITransientDependency
{
    private readonly IConciliacaoContabilApuracaoRepositorio _apuracaoRepositorio;

    public ConciliacaoContabilApuracaoService(IConciliacaoContabilApuracaoRepositorio apuracaoRepositorio)
    {
        _apuracaoRepositorio = apuracaoRepositorio;
    }

    public async Task<ListResultDto<BuscarConciliacaoContabilApuracaoOutput>> BuscarTodasApuracoesPorConciliacao(int legacyId, PagedFilteredAndSortedRequestInput input)
    {
        return await _apuracaoRepositorio.BuscarTodasApuracoesPorConciliacao(legacyId, input);
    }
}