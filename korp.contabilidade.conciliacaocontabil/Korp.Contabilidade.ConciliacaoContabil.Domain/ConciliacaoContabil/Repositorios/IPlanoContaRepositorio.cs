using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;

public interface IPlanoContaRepositorio
{
    Task<ListResultDto<PlanoConta>> BuscarTodosPlanosConta(PagedFilteredAndSortedRequestInput input);
    Task<bool> IsContaVirtual(int codigoConta);
    Task<List<int>> GetContasVinculadasIds(int codigoConta, int? idTipoConciliacaoContabil = null);
    Task<List<PlanoConta>> GetContasVinculadas(int codigoConta, int? idTipoConciliacaoContabil = null);
}