using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Accounting.Domain.Entities;

public class LegacyFechamentoPeriodoContabil : FullAuditedEntity, IMustHaveTenantAndEnvironment
{
    public Guid TenantId { get; set; }
    public Guid EnvironmentId { get; set; }
    
    public string Data { get; set; }
}