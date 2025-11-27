using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Viasoft.Accounting.Domain.Enums;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Accounting.Domain.Entities
{
    [Table("AccountingEntry")]
    public class AccountingEntry : FullAuditedEntity, IMustHaveTenant
    {
        public int Code { get; set; }        
        public string EntryType { get; set; }   
        
        [MaxLength(8)]
        public string EntryDateLegacy{ get => EntryDate.Value.ToLocalTime().ToString("yyyyMMdd");
            set {  }
        }

        public int? AccountingYear { get; set; }        
        public int? AccountingMonth { get; set; }
        
        [MaxLength(8)]
        public DateTime? CreationTimeLegacy
        {
            get => CreationTime;
            set { }
        }

        public string Notes { get; set; }       
        
        public string Series { get; set; }        
       
        public string Customer { get; set; }   
        
        public int CompanyCode { get; set; }
                
        public EntrySourceType? SourceType { get; set; }
        
        public Guid? SourceId { get; set; }
        
        public DateTime? EntryDate{ get; set; } 
        public Guid TenantId { get; set; }
        public string Status { get; set; }
        public string Usuario { get; set; }
        public string Fornecedor { get; set; }
        public string Entrada { get; set; }
        public int? LegacyIdContaReceber { get; set; }
    }
}