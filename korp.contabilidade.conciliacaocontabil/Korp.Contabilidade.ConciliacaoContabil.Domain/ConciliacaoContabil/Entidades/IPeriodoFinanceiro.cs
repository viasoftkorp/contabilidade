using System;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

public interface IPeriodoFinanceiro
{
    public DateOnly DataEntrada { get; set; }
    public DateOnly? DataPagamento { get; set; }
    public int LegacyCompanyId { get; set; }
}