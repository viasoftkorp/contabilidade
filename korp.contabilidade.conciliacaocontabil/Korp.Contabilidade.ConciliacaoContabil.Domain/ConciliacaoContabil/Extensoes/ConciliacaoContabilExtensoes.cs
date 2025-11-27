using System.Collections.Generic;
using System.Linq;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;

public static class ConciliacaoContabilExtensoes
{
    public static List<int> ListarEmpresas(this ConciliacaoContabil conciliacaoContabil)
    {
        return conciliacaoContabil.Empresas
            .Select(e => e.LegacyCompanyId)
            .ToList();
    }

    public static List<int> ListarCodigoContasContabeis(this ConciliacaoContabil conciliacaoContabil)
    {
        return conciliacaoContabil.TipoConciliacaoContabil.ConciliacaoContabilContas
            .Select(l => l.CodigoConta)
            .ToList();
    }
}