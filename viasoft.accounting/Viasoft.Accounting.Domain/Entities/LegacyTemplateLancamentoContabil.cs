using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Accounting.Domain.Entities;

public class LegacyTemplateLancamentoContabil : FullAuditedEntity, IMustHaveTenantAndEnvironment
{
    public Guid TenantId { get; set; }
    public Guid EnvironmentId { get; set; }
    public string CodigoOperacaoContabil { get; set; }

    public string TipoLancamento { get; set; }
    public int CodigoContaContabil { get; set; }
    public string CodigoHistorico { get; set; }
    public int CodigoCampo { get; set; }
    public int? CodigoComplemento1 { get; set; }
    public int? CodigoComplemento2 { get; set; }
    public int? CodigoComplemento3 { get; set; }
    public int? CodigoComplemento4 { get; set; }
    public int? CodigoComplemento5 { get; set; }
}