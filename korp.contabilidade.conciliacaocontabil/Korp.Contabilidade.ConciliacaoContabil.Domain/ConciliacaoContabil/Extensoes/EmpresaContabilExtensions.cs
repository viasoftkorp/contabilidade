using System.Linq;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;

public static class EmpresaContabilExtensions
{
    public static IQueryable<T> FiltrarEmpresaContabil<T>(this IQueryable<T> registros, ConciliacaoContabil conciliacaoContabil) where T : IEmpresaContabil
    {
        var empresas = conciliacaoContabil.ListarEmpresas();
        return registros
            .Where(registro => empresas.Contains(registro.LegacyCompanyId));
    }
}