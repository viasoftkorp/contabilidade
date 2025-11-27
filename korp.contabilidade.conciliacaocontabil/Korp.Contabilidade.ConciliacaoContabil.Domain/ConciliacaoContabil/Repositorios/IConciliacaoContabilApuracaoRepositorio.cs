using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;

public interface IConciliacaoContabilApuracaoRepositorio
{
    Task<ListResultDto<BuscarConciliacaoContabilApuracaoOutput>> BuscarTodasApuracoesPorConciliacao(int legacyId, PagedFilteredAndSortedRequestInput input);
}