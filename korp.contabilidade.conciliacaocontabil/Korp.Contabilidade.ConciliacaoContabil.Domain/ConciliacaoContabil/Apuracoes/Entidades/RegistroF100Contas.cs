using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class RegistroF100Contas: Entity
{
    public int CodigoConta { get; set; }
    public int LegacyRegistroF100 { get; set; }
    public string Operacao { get; set; }
}