using System;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class OutrosLancamentosFiscais: Entity,IPeriodoContabilOutros
{
    public int LegacyId { get; set; }
    public string Imposto { get; set; }
    public string Historico { get; set; }
    public DateOnly DataLancamento { get; set; }
    public decimal Valor { get; set; }
    public string CreditoDebito { get; set; }
    public int LegacyIdCabecalhoOutros { get; set; }
}