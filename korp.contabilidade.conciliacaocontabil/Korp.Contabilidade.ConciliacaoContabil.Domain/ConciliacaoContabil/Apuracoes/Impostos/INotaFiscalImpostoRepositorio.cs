using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos;

public interface INotaFiscalImpostoRepositorio
{
    Task<IReadOnlyCollection<IImpostoDto>> ListarNotasFiscais(ConciliacaoContabil conciliacaoContabil);
}