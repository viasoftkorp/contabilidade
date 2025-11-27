using System.Linq;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;

public static class PeriodoContabilNotaExtensions
{
    public static IQueryable<T> FiltrarPeriodoContabil<T>(this IQueryable<T> registros, ConciliacaoContabil conciliacaoContabil) where T : IPeriodoContabilNota
    {
        var empresas = conciliacaoContabil.ListarEmpresas();
        return registros
            .Where(registro => empresas.Contains(registro.LegacyCompanyId))
            .Where(registro => registro.Data >= conciliacaoContabil.DataInicial && registro.Data <= conciliacaoContabil.DataFinal)
            .Where(registro => registro.Situacao != "CANCELADA");
    }
    
    public static IQueryable<T> FiltrarEmpresaContabil<T>(this IQueryable<T> registros, ConciliacaoContabil conciliacaoContabil) where T : IPeriodoContabilNota
    {
        var empresas = conciliacaoContabil.ListarEmpresas();
        return registros
            .Where(registro => empresas.Contains(registro.LegacyCompanyId));
    }
}