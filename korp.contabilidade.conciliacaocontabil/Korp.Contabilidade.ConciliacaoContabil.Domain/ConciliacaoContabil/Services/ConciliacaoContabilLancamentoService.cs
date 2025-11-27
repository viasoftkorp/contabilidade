using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;

public class ConciliacaoContabilLancamentoService : IConciliacaoContabilLancamentoService, ITransientDependency
{
    private readonly IConciliacaoContabilLancamentoRepositorio _conciliacaoContabilLancamentoRepositorio;

    public ConciliacaoContabilLancamentoService(IConciliacaoContabilLancamentoRepositorio conciliacaoContabilLancamentoRepositorio)
    {
        _conciliacaoContabilLancamentoRepositorio = conciliacaoContabilLancamentoRepositorio;
    }

    public async Task<ListResultDto<BuscarConciliacaoContabilLancamentoOutput>> BuscarTodosLancamentosPorConciliacao(int legacyId, PagedFilteredAndSortedRequestInput input)
    {
        return await _conciliacaoContabilLancamentoRepositorio.BuscarTodosLancamentosPorConciliacao(legacyId, input);
    }
}