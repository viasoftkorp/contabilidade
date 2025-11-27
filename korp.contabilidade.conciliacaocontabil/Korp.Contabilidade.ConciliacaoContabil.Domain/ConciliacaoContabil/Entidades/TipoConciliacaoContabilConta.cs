using System.ComponentModel.DataAnnotations;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

public class TipoConciliacaoContabilConta
{
    [Key]
    public int LegacyId { get; set; }
    public int CodigoConta { get; set; }
    public string Descricao { get; set; }
    public int IdTipoConciliacaoContabil { get; set; }
}