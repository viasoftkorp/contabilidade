using System;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

public interface IPeriodoDepreciacao
{
    public DateOnly Data { get; set; }
}