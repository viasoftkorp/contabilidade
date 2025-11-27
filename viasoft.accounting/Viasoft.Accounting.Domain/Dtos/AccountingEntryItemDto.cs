using System;
using System.Collections.Generic;
using Viasoft.Core.DDD.Application.Dto.Entities;

namespace Viasoft.Accounting.Domain.Dtos
{
    public class AccountingEntryItemDto : FullAuditedEntityDto
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
        public int Order { get; set; }
        
        public string Origem { get; set; }
        public int LegacyIdOrigem { get; set; }
        public List<AccountingEntryItemOriginDto> AccountingEntryItemOrigins { get; set; }
    }
}