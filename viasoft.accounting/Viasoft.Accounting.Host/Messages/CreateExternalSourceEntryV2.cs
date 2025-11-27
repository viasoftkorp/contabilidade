using System;
using System.Collections.Generic;
using Viasoft.Accounting.Domain.Dtos;
using Viasoft.Accounting.Host.Contracts;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Accounting.Host.Messages
{
    [Endpoint("CreateAccountingEntryV2")]
    public class CreateExternalSourceEntryV2 : IMessage
    {
        public Guid SourceId { get; set; }
        
        public Guid AccountingOperationId { get; set; }
        
        public AccountingEntryDto AccountingEntry { get; set; }
        
        public List<AccountingEntryItemDto> AccountingEntryItems { get; set; }
        
    }
}