using Viasoft.Data.Attributes;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public class AdicionarContaInput
{
    [StrictRequired]
    public int CodigoConta { get; set; }
    public string Descricao { get; set; }
    public bool? ShouldAddLinkedAccounts { get; set; }
}