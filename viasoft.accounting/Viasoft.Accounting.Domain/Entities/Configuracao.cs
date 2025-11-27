using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Accounting.Domain.Entities;

public class Configuracao : FullAuditedEntity, IMustHaveTenantAndEnvironment
{
    public Guid TenantId { get; set; }
    public string CodigoOperacaoContabilAdiantamento { get; set; }
    public int? CodigoContaContabilPai { get; set; }
    public string CodigoContaReferencia { get; set; }
    public Guid EnvironmentId { get; set; }
}
