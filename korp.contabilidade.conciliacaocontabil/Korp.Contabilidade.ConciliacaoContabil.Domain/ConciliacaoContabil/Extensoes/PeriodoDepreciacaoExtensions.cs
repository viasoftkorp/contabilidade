using System.Linq;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;

public static class PeriodoDepreciacaoExtensions
{
    public static IQueryable<T> FiltrarPeriodoDepreciacao<T>(this IQueryable<T> registros, ConciliacaoContabil conciliacaoContabil) where T : IPeriodoDepreciacao
    {
        return registros
            .Where(registro => registro.Data >= conciliacaoContabil.DataInicial && registro.Data <= conciliacaoContabil.DataFinal);
    }
}