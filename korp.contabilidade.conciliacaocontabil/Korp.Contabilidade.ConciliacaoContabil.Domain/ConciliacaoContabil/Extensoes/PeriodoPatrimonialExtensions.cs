using System.Linq;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;

public static class PeriodoPatrimonialExtensions
{
    public static IQueryable<T> FiltrarPeriodoEntradaPatrimonial<T>(this IQueryable<T> registros, ConciliacaoContabil conciliacaoContabil) where T : IPeriodoPatrimonial
    {
        var empresas = conciliacaoContabil.ListarEmpresas();
        return registros
            .Where(registro => empresas.Contains(registro.LegacyCompanyId))
            .Where(registro => registro.DataEntrada >= conciliacaoContabil.DataInicial && registro.DataEntrada <= conciliacaoContabil.DataFinal);
    }
    
    public static IQueryable<T> FiltrarPeriodoSaidaPatrimonial<T>(this IQueryable<T> registros, ConciliacaoContabil conciliacaoContabil) where T : IPeriodoPatrimonial
    {
        var empresas = conciliacaoContabil.ListarEmpresas();
        return registros
            .Where(registro => empresas.Contains(registro.LegacyCompanyId))
            .Where(registro => registro.DataSaida >= conciliacaoContabil.DataInicial && registro.DataSaida <= conciliacaoContabil.DataFinal);
    }
    
    public static IQueryable<T> FiltrarEmpresaPatrimonial<T>(this IQueryable<T> registros, ConciliacaoContabil conciliacaoContabil) where T : IPeriodoPatrimonial
    {
        var empresas = conciliacaoContabil.ListarEmpresas();
        return registros
            .Where(registro => empresas.Contains(registro.LegacyCompanyId));
    }
    
    public static IQueryable<T> FiltrarDataPatrimonial<T>(this IQueryable<T> registros, ConciliacaoContabil conciliacaoContabil) where T : IPeriodoPatrimonialItens
    {
        return registros
            .Where(registro => registro.Data >= conciliacaoContabil.DataInicial && registro.Data <= conciliacaoContabil.DataFinal);
    }
}