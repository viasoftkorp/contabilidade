using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class Cliente: Entity
{
    public string Codigo { get; set; }
    public string RazaoSocial { get; set; }
    public int? CodigoConta { get; set; }
    public int? ContaContabilAdiantamento { get; set; }
}