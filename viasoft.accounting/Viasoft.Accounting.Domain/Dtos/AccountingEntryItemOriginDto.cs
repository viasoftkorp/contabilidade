using System;
using Viasoft.Accounting.Domain.Enums;

namespace Viasoft.Accounting.Domain.Dtos
{
    public class AccountingEntryItemOriginDto
    {      
        public decimal? DebitValue { get; set; }          
        public decimal? CreditValue { get; set; }         
        public AccountingEntryItemOriginType OriginType { get; set; }         
        public Guid? IdOrigin { get; set; }
        public int LegacyIdOrigin { get; set; }
    }
}