using System;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

public interface IPeriodoContabil
{
    public DateOnly Data { get; set; }
    public int LegacyCompanyId { get; set; }
}