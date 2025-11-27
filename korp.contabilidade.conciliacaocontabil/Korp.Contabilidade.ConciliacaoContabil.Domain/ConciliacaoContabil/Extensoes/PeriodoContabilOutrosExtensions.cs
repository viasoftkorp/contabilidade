using System.Linq;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;

public static class PeriodoContabilOutrosExtensions
{
    public static IQueryable<T> FiltrarPeriodoEntradaFinanceiro<T>(this IQueryable<T> registros, ConciliacaoContabil conciliacaoContabil) where T : IPeriodoContabilOutros
    {
        return registros
            .Where(registro => registro.DataLancamento >= conciliacaoContabil.DataInicial && registro.DataLancamento <= conciliacaoContabil.DataFinal);
    }
}