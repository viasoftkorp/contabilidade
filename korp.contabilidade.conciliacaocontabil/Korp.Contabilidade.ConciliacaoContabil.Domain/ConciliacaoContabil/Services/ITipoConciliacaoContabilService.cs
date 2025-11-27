using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;

public interface ITipoConciliacaoContabilService
{
    Task<AdicionarContaResponseEnum> AdicionarConta(int legacyId, AdicionarContaInput input);
    Task<ListResultDto<TipoConciliacaoContabilConta>> BuscarTodasContasPorConciliacao(int legacyId, PagedFilteredAndSortedRequestInput input);
    Task<RemoverContaResponseEnum> DeletarConta(int legacyId, int id, bool? shouldRemoveLinkedAccounts);
}