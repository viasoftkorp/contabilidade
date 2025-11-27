using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;

public class PlanoContaService : IPlanoContaService, ITransientDependency
{
    private readonly IPlanoContaRepositorio _planoContaRepositorio;

    public PlanoContaService(IPlanoContaRepositorio planoContaRepositorio)
    {
        _planoContaRepositorio = planoContaRepositorio;
    }

    public Task<ListResultDto<PlanoConta>> BuscarTodosPlanosConta(PagedFilteredAndSortedRequestInput input)
    {
        return _planoContaRepositorio.BuscarTodosPlanosConta(input);
    }
}