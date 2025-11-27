using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos;

public interface IImpostoRepositorio
{
    Task<IReadOnlyCollection<IImpostoDto>> ListarNotaEntrada(ConciliacaoContabil conciliacaoContabil);
    Task<IReadOnlyCollection<IImpostoDto>> ListarNotaSaida(ConciliacaoContabil conciliacaoContabil);
}