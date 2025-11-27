using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Accounting.Domain.Entities
{
    [Table("AccountingEntryItem")]
    public class AccountingEntryItem : FullAuditedEntity, IMustHaveTenant
    {
        public int EntryCode { get; set; }        
        public string CostCenter { get; set; }        
        public decimal? DebitValue { get; set; }          
        public decimal? CreditValue { get; set; }        
        public string EntryHistoricCode { get; set; }        
        public string Notes { get; set; }        
      
        public string AccountingOperation { get; set; }        
        public int? AccountCode { get; set; }        
        public int CompanyCode { get; set; }
        
        public Guid? AccountingEntryId { get; set; }

        public Guid TenantId { get; set; }
        public string Usuario { get; set; }
        
        public int LegacyId { get; set; }
        
        public string Origem { get; set; }
        public int LegacyIdOrigem { get; set; }
    }
}