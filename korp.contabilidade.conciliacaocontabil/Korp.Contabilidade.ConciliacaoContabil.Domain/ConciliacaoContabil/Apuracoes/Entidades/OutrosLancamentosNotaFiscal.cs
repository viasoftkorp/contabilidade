using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class OutrosLancamentosNotaFiscal: Entity
{
    public int LegacyOutrosLancamentosId { get; set; }
    public int LegacyNotaEntradaId { get; set; }
    public int LegacyNotaSaidaId { get; set; }
}