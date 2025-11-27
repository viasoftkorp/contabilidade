using System;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

public interface IPeriodoContabilOutros
{
    public DateOnly DataLancamento { get; set; }
}