using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class FiscalItemNotaSaida : Entity
{
    public decimal? ValorIcms {get; set;}
    public decimal? ValorIcmsSt {get; set;}
    public decimal? ValorPis {get; set;}
    public decimal? ValorCofins {get; set;}
    public decimal? ValorIpi {get; set;}
    public decimal? ValorIss {get; set;}
    public int IdNotaSaida { get; set; }
}