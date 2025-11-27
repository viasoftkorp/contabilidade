using System.Linq;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;

public static class PeriodoContabilExtensions
{
    public static IQueryable<T> FiltrarPeriodoContabil<T>(this IQueryable<T> registros, ConciliacaoContabil conciliacaoContabil) where T : IPeriodoContabil
    {
        var empresas = conciliacaoContabil.ListarEmpresas();
        return registros
            .Where(registro => empresas.Contains(registro.LegacyCompanyId))
            .Where(registro => registro.Data >= conciliacaoContabil.DataInicial && registro.Data <= conciliacaoContabil.DataFinal);
    }
    
    public static IQueryable<T> FiltrarEmpresaContabil<T>(this IQueryable<T> registros, ConciliacaoContabil conciliacaoContabil) where T : IPeriodoContabil
    {
        var empresas = conciliacaoContabil.ListarEmpresas();
        return registros
            .Where(registro => empresas.Contains(registro.LegacyCompanyId));
    }
}