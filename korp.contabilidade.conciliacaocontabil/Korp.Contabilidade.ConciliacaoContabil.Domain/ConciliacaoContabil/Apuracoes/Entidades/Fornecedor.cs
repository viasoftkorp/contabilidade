using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class Fornecedor: Entity
{
    public int LegacyId { get; set; }
    public string Codigo { get; set; }
    public string RazaoSocial { get; set; }
    public int? CodigoConta { get; set; }
    public int? ContaContabilAdiantamento { get; set; }
}