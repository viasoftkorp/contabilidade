using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.LancamentoContabil.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.LancamentoContabil.Repositorio;

public interface ILancamentoContabilRepositorio
{
    Task<List<LancamentoContabilOutput>> ListarLancamentosContabeis(ConciliacaoContabil conciliacaoContabil);
}