using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Cofins.Creditar;

public interface ICofinsCreditarRepositorio
{
    Task<IReadOnlyCollection<IImpostoDto>> ListarImpostoCreditar(ConciliacaoContabil conciliacaoContabil);
}