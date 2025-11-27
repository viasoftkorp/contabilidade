using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Cofins.Creditar;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Cofins.Debitar;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Cofins;

public class CofinsRepositorio: IImpostoRepositorio
{
    private readonly IEnumerable<ICofinsCreditarRepositorio> _repositoriosCreditar;
    private readonly IEnumerable<ICofinsDebitarRepositorio> _debitarRepositorios;

    public CofinsRepositorio(IEnumerable<ICofinsCreditarRepositorio> repositoriosCreditar, IEnumerable<ICofinsDebitarRepositorio> debitarRepositorios)
    {
        _repositoriosCreditar = repositoriosCreditar;
        _debitarRepositorios = debitarRepositorios;
    }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarNotaEntrada(ConciliacaoContabil conciliacaoContabil)
    {
        var resultadoImpostos = new List<IImpostoDto>();
        foreach (var cofinsCreditar in _repositoriosCreditar)
        {
            var impostos = await cofinsCreditar.ListarImpostoCreditar(conciliacaoContabil);
            resultadoImpostos.AddRange(impostos);
        }

        return resultadoImpostos;
   }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarNotaSaida(ConciliacaoContabil conciliacaoContabil)
    {
        var resultadoImpostos = new List<IImpostoDto>();
        foreach (var cofinsCreditar in _debitarRepositorios)
        {
            var impostos = await cofinsCreditar.ListarImpostoDebitar(conciliacaoContabil);
            resultadoImpostos.AddRange(impostos);
        }

        return resultadoImpostos;
    }
}