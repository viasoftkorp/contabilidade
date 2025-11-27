using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class DepreciacaoConfiguracao: Entity
{
    public int LegacyIdBem { get; set; }
    public string TipoContabilidade { get; set; }
}