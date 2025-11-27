using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Faturamento;

public interface INotaFaturamentoRepositorio
{
    Task<IReadOnlyCollection<IImpostoDto>> ListarNotasFiscais(ConciliacaoContabil conciliacaoContabil);
}