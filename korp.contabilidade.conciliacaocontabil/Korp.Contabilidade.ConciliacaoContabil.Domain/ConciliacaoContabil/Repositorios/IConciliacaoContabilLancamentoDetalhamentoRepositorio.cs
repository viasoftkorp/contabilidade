using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;

public interface IConciliacaoContabilLancamentoDetalhamentoRepositorio
{
    Task<ListResultDto<BuscarConciliacaoContabilLancamentoDetalhamentoOutput>> BuscarTodosLancamentosDetalhamentos(int idConciliacaoContabilLancamento, PagedFilteredAndSortedRequestInput input);
    Task<CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus> Create(ConciliacaoContabilLancamentoDetalhamentoInput input);
    Task<CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus> Update(ConciliacaoContabilLancamentoDetalhamentoInput input);
}