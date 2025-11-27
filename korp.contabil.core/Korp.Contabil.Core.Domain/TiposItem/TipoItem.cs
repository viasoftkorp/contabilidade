using System.ComponentModel.DataAnnotations.Schema;
using Viasoft.Core.DDD.Entities;

namespace Korp.Contabil.Core.Domain.TiposItem;

[Table("CT_TIPO_ITEM")]
public class TipoItem : Entity
{
    [Column("CODIGO")]
    public string Codigo { get; set; }
    [Column("DESC_TIPO_ITEM")]
    public string Descricao { get; set; }
}
