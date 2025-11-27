using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class TipoLancamento
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LegacyId { get; set; }
    public string Codigo { get; set; }
    public string Descricao { get; set; }
}