using System;
using System.ComponentModel.DataAnnotations.Schema;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Accounting.Domain.Entities
{
    [Table("BookkeepingAccount")]
    public class BookkeepingAccount : FullAuditedEntity, IMustHaveTenant
    {
        public int Code { get; set; }
        public string Classification { get; set; }
        public string Name { get; set; }
        public string IsSynthetic { get; set; }
        public Guid TenantId { get; set; }
        public string Changed { get; set; }
        public string CenterCostCode { get; set; }
        public int Number { get; set; }
        public int? ParentCode { get; set; }
        public DateTime? Date { get; set; }
        public int? Model { get; set; }
        public int? Nature { get; set; }
        public int Level { get; set; }
        public bool? Required { get; set; }
        public string Type { get; set; }
        public string Summarised { get; set; }
        public int Order { get; set; }
        public int? BookkeepingAccountGroupLegacyId { get; set; }
        public string CtaCode { get; set; }
        public bool? FgRevenues { get; set; }
        public bool? CalculateCosts { get; set; }
    }
}
