using System;
using Viasoft.Accounting.Domain.Enums;
using Viasoft.Core.DDD.Application.Dto.Entities;
using Viasoft.Data.Attributes;

namespace Viasoft.Accounting.Host.Controllers.Inputs
{
    public class AccountingOperationEntryRuleInput : EntityDto
    {
        [StrictRequired]
        public AccountingEntryType AccountingEntryType { get; set; }
        
        [StrictRequired]
        public Guid AccountingOperationId { get; set; }

        [StrictRequired]
        public Guid BookkeepingAccountId { get; set; }
        
        [StrictRequired]
        public string EntryVariable { get; set; }
        
        public  Guid HistoricId { get; set; }
        
        public CteComplement? FirstDisplayInfo { get; set; }
        public CteComplement? SecondDisplayInfo { get; set; }
        
        [StrictRequired]
        public int Order { get; set; }
    }
}