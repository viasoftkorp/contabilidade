using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

public class CtParametros
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LegacyId { get; set; }
    public string Banco { get; set; }
    public string Estoque { get; set; }
    public string Fornecedor { get; set; }
    public string PlanoCliente { get; set; }
    public int ContaContabilAdiantamentoFornecedor { get; set; }
    public int ContaContabilAdiantamentoCliente { get; set; }
}