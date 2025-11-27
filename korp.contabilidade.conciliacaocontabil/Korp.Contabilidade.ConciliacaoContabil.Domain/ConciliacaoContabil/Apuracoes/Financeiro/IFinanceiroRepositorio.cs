using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Financeiro;

public interface IFinanceiroRepositorio
{
    Task<IReadOnlyCollection<IContasFinanceiroDto>> ListarContaPagamento(ConciliacaoContabil conciliacaoContabil);
    Task<IReadOnlyCollection<IContasFinanceiroDto>> ListarContaRecebimento(ConciliacaoContabil conciliacaoContabil);
}