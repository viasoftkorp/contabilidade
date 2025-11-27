using Viasoft.Core.DDD.Application.Dto.Entities;

namespace Viasoft.Accounting.Host.Contracts
{
    public class dsadasAccountingEntryItemDto : FullAuditedEntityDto
    {
        public int EntryCode { get; set; }        
        public decimal? DebitValue { get; set; }          
        public decimal? CreditValue { get; set; }        
        public string EntryHistoricCode { get; set; }        
        public string Notes { get; set; }        
        public string AccountingOperation { get; set; }        
        public int? AccountCode { get; set; }        
        public int CompanyCode { get; set; }
        
        public int Order { get; set; }

    }
}