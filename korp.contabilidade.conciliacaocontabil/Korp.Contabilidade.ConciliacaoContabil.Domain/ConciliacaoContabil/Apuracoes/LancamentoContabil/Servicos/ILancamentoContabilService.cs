using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.LancamentoContabil.Servicos;

public interface ILancamentoContabilService
{
    Task<List<ConciliacaoContabilLancamento>> ApurarLancamentoContabil(ConciliacaoContabil conciliacaoContabil);
}