using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Accounting.Domain.Entities
{
    [Table("AccountingOperation")]
    public class AccountingOperation : FullAuditedEntity, IMustHaveTenant
    {
        public string Code { get; set; }
        
        public string Description { get; set; }
        
        public string Cfop { get; set; }  
        
        public string EvaluatedSocialContribution { get; set; }  
        
        public string DoesntGenerateUnitaryCost { get; set; }      
        
        public string ShouldGenerateEntries { get; set; }     
        
        public string CstIcms { get; set; }   
        
        public string CstPis { get; set; }   
        
        public string CstCofins { get; set; }
        public bool CteModule { get; set; }
        public Guid TenantId { get; set; }
        public bool? IssueInvoice { get; set; }
        public bool? UsedInWarehouseRequisitionModule { get; set; }
    }
}