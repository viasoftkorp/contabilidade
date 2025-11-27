using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Viasoft.Accounting.Domain.Enums;
using Viasoft.Core.DDD.Entities.Auditing;

namespace Viasoft.Accounting.Domain.Entities
{
    [Table("AccountingOperationEntryRule")]
    public class AccountingOperationEntryRule : FullAuditedEntity
    {
        public Guid AccountingOperationId { get; set; }
        
        public AccountingEntryType AccountingEntryType { get; set; }
        
        public Guid BookkeepingAccountId { get; set; }
        
        [Required]
        public string EntryVariable { get; set; }
        
        public  Guid HistoricId { get; set; }
        
        public int? FirstDisplayInfo { get; set; }
        
        public int? SecondDisplayInfo { get; set; }
        
        public int Order { get; set; }
    }
}