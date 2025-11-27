using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;

public class ConciliacaoContabilLancamentoDetalhamentoService: IConciliacaoContabilLancamentoDetalhamentoService, ITransientDependency
{
    private readonly IConciliacaoContabilLancamentoDetalhamentoRepositorio _conciliacaoContabilLancamentoDetalhamentoRepositorio;

    public ConciliacaoContabilLancamentoDetalhamentoService(IConciliacaoContabilLancamentoDetalhamentoRepositorio conciliacaoContabilLancamentoDetalhamentoRepositorio)
    {
        _conciliacaoContabilLancamentoDetalhamentoRepositorio = conciliacaoContabilLancamentoDetalhamentoRepositorio;
    }

    public async Task<ListResultDto<BuscarConciliacaoContabilLancamentoDetalhamentoOutput>> BuscarTodosLancamentosDetalhamentos(int idConciliacaoContabilLancamento, PagedFilteredAndSortedRequestInput input)
    {
        return await _conciliacaoContabilLancamentoDetalhamentoRepositorio.BuscarTodosLancamentosDetalhamentos(idConciliacaoContabilLancamento, input);
    }

    public async Task<CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus> Create(ConciliacaoContabilLancamentoDetalhamentoInput input)
    {
        return await _conciliacaoContabilLancamentoDetalhamentoRepositorio.Create(input);
    }

    public async Task<CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus> Update(ConciliacaoContabilLancamentoDetalhamentoInput input)
    {
        return await _conciliacaoContabilLancamentoDetalhamentoRepositorio.Update(input);
    }
}