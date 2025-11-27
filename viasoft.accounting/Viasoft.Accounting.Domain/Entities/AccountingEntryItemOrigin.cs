using System;
using Viasoft.Accounting.Domain.Enums;
using Viasoft.Core.DDD.Entities;

namespace Viasoft.Accounting.Domain.Entities
{
    public class AccountingEntryItemOrigin : Entity
    {
        public int LegacyId { get; set; }
        public AccountingEntryItem AccountingEntryItem { get; set; }
        public int AccountingEntryItemLegacyId { get; set; }
        public Guid? IdOrigem { get; set; }
        public int LegacyIdOrigem { get; set; }
        public decimal DebitValue { get; set; }      
        public decimal CreditValue { get; set; }          
        public AccountingEntryItemOriginType OriginType { get; set; }  
        public DateTime CreationDate{ get; set; } 
    }
}