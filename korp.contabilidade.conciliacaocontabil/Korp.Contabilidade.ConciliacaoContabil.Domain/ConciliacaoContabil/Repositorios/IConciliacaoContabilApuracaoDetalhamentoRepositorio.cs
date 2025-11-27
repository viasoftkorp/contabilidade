using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;

public interface IConciliacaoContabilApuracaoDetalhamentoRepositorio
{
    Task<ListResultDto<BuscarConciliacaoContabilApuracaoDetalhamentoOutput>> BuscarTodasApuracoesDetalhamentos(int idConciliacaoContabilApuracao, PagedFilteredAndSortedRequestInput input);
    Task<CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus> Create(ConciliacaoContabilApuracaoDetalhamentoInput input);
    Task<CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus> Update(ConciliacaoContabilApuracaoDetalhamentoInput input);
}