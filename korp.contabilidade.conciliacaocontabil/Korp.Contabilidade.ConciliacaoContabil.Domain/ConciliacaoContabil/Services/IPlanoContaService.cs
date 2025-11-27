using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;

public interface IPlanoContaService
{
    Task<ListResultDto<PlanoConta>> BuscarTodosPlanosConta(PagedFilteredAndSortedRequestInput input);
}