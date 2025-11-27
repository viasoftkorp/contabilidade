using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Accounting.Domain.Entities;

public class LegacyRegraValorLancamento : FullAuditedEntity, IMustHaveTenantAndEnvironment
{
    public Guid TenantId { get; set; }
    public Guid EnvironmentId { get; set; }
    public int Codigo { get; set; }
    public string Campo { get; set; }
}