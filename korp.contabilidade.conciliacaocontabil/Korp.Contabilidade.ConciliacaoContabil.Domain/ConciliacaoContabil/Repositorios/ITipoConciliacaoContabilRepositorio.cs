using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;

public interface ITipoConciliacaoContabilRepositorio
{
    Task<TipoConciliacaoContabil> BuscarConciliacaoContabilTipo(TipoApuracaoConciliacaoContabil tipoApuracaoConciliacao);
    Task AdicionarConta(TipoConciliacaoContabilConta conta);
    Task AdicionarContas(IEnumerable<TipoConciliacaoContabilConta> contas);
    Task<ListResultDto<TipoConciliacaoContabilConta>> BuscarTodasContasPorConciliacao(int legacyId, PagedFilteredAndSortedRequestInput input);
    Task<TipoConciliacaoContabilConta> GetConta(int legacyId, int id);
    Task DeletarConta(int legacyId, int id);
    Task DeletarContas(int legacyId, List<int> ids);
    Task<TipoConciliacaoContabil> BuscarTipoConciliacaoContabilPorId(int legacyId);
}