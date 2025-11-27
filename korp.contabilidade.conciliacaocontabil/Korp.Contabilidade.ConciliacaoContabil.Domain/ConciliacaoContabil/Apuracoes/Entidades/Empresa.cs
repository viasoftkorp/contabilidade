using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class Empresa: Entity
{
    public int LegacyId { get; set; }
    public string Nome { get; set; }
}