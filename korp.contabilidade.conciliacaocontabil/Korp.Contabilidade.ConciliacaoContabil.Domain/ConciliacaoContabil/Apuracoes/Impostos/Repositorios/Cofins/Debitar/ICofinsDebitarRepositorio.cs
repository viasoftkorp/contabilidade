using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Cofins.Debitar;

public interface ICofinsDebitarRepositorio
{
    Task<IReadOnlyCollection<IImpostoDto>> ListarImpostoDebitar(ConciliacaoContabil conciliacaoContabil);
}