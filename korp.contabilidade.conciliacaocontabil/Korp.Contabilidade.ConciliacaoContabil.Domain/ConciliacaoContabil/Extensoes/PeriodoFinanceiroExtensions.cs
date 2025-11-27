using System.Linq;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;

public static class PeriodoFinanceiroExtensions
{
    public static IQueryable<T> FiltrarPeriodoEntradaFinanceiro<T>(this IQueryable<T> registros, ConciliacaoContabil conciliacaoContabil) where T : IPeriodoFinanceiro
    {
        var empresas = conciliacaoContabil.ListarEmpresas();
        return registros
            .Where(registro => empresas.Contains(registro.LegacyCompanyId))
            .Where(registro => registro.DataEntrada >= conciliacaoContabil.DataInicial && registro.DataEntrada <= conciliacaoContabil.DataFinal);
    }
    
    public static IQueryable<T> FiltrarPeriodoPagamentoFinanceiro<T>(this IQueryable<T> registros, ConciliacaoContabil conciliacaoContabil) where T : IPeriodoFinanceiro
    {
        var empresas = conciliacaoContabil.ListarEmpresas();
        return registros
            .Where(registro => empresas.Contains(registro.LegacyCompanyId))
            .Where(registro => registro.DataPagamento >= conciliacaoContabil.DataInicial && registro.DataPagamento <= conciliacaoContabil.DataFinal);
    }
}