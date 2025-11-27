using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class CabecalhoOutrosLancamentosFiscais: Entity,IEmpresaContabil
{
    public int LegacyId { get; set; }
    public int Codigo { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int LegacyCompanyId { get; set; }
}