using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Financeiro;

public interface IContaFinanceiroRepositorio
{
    Task<IReadOnlyCollection<IContasFinanceiroDto>> ListarContas(ConciliacaoContabil conciliacaoContabil);
}