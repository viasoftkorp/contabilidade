using System;
using Viasoft.Accounting.Domain.Enums;
using Viasoft.Core.DDD.Application.Dto.Entities;

namespace Viasoft.Accounting.Host.Controllers.Outputs
{
    public class AccountingOperationEntryRuleOutput : FullAuditedEntityDto
    {
        public AccountingEntryType AccountingEntryType { get; set; }
        public string AccountingEntryTypeDescription => AccountingEntryType.ToString();
        public Guid AccountingOperationId { get; set; }

        public Guid BookkeepingAccountId { get; set; }
        public int BookkeepingAccountCode { get; set; }
        public string BookkeepingAccountDescription { get; set; }
        public string BookkeepingAccountClassification { get; set; }

        public string EntryVariable { get; set; }
        
        public  Guid HistoricId { get; set; }
        public  string HistoricDescription { get; set; }
        
        public CteComplement? FirstDisplayInfo { get; set; }
        public string FirstDisplayInfoDescription => FirstDisplayInfo.ToString();
        public string SecondDisplayInfoDescription => SecondDisplayInfo.ToString();
        public CteComplement? SecondDisplayInfo { get; set; }
        
        public int Order { get; set; }
    }
}