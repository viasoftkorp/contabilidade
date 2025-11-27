using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes;

public interface IApuracao
{
    Task<List<ConciliacaoContabilApuracao>> Apurar(ConciliacaoContabil conciliacaoContabil);
}