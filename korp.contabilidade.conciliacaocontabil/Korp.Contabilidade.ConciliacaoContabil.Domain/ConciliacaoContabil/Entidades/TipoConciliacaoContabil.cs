using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

public class TipoConciliacaoContabil
{
    [Key]
    public int LegacyId { get; set; }
    public string Descricao { get; set; }
    public List<TipoConciliacaoContabilConta> ConciliacaoContabilContas { get; set; } = new();
    public TipoApuracaoConciliacaoContabil TipoApuracao { get; set; }
    public bool Ativo { get; set; }
}