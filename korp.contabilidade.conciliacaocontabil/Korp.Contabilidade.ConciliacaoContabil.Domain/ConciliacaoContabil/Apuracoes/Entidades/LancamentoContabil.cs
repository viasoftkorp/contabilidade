using Viasoft.Core.DDD.Entities;

// ReSharper disable once CheckNamespace
// trocado namespace para não conflitar com o .Entidades da Conciliacao
namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;

public class LancamentoContabil: Entity
{
    public int LegacyId { get; set; }
    public int LegacyCompanyId { get; set; }
    public int NumeroLancamento { get; set; }
    public string Historico { get; set; }
    public decimal ValorDebito { get; set; }
    public decimal ValorCredito { get; set; }
    public int CodigoConta { get; set; }
}