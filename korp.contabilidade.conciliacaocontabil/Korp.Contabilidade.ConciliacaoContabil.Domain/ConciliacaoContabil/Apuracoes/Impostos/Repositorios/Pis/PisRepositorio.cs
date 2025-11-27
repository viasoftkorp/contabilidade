using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Pis.Creditar;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Pis.Debitar;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Pis;

public class PisRepositorio: IImpostoRepositorio
{
    private readonly IEnumerable<IPisCreditarRepositorio> _repositoriosCreditar;
    private readonly IEnumerable<IPisDebitarRepositorio> _debitarRepositorios;
    public PisRepositorio(IEnumerable<IPisCreditarRepositorio> repositoriosCreditar, IEnumerable<IPisDebitarRepositorio> debitarRepositorios)
    {
        _repositoriosCreditar = repositoriosCreditar;
        _debitarRepositorios = debitarRepositorios;
    }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarNotaEntrada(ConciliacaoContabil conciliacaoContabil)
    {
        var resultadoImpostos = new List<IImpostoDto>();
        foreach (var pisCreditar in _repositoriosCreditar)
        {
            var impostos = await pisCreditar.ListarImpostoCreditar(conciliacaoContabil);
            resultadoImpostos.AddRange(impostos);
        }

        return resultadoImpostos;
    }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarNotaSaida(ConciliacaoContabil conciliacaoContabil)
    {
        var resultadoImpostos = new List<IImpostoDto>();
        foreach (var pisCreditar in _debitarRepositorios)
        {
            var impostos = await pisCreditar.ListarImpostoDebitar(conciliacaoContabil);
            resultadoImpostos.AddRange(impostos);
        }

        return resultadoImpostos;
    }
}