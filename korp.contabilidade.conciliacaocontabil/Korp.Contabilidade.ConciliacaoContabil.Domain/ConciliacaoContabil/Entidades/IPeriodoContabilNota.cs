using System;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

public interface IPeriodoContabilNota
{
    public DateOnly Data { get; set; }
    public int LegacyCompanyId { get; set; }
    public string Situacao { get; set; }
}