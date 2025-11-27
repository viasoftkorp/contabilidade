using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class CreditoCofins: Entity
{
    public int LegacyIdBem { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public decimal Valor { get; set; }
}