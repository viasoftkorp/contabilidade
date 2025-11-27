using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Accounting.Domain.Entities
{
    public class ManagerialAccount : FullAuditedEntity, IMustHaveTenant
    {
        public string Code { get; set; }      
        public string Description { get; set; }

        public Guid TenantId { get; set; }
    }
}