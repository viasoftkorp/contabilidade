using System;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

public interface IPeriodoPatrimonial
{
    public DateOnly? DataEntrada { get; set; }
    public DateOnly? DataSaida { get; set; }
    public int LegacyCompanyId { get; set; }
}