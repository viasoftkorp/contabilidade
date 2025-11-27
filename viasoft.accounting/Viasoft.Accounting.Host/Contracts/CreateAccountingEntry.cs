using System;
using System.Collections.Generic;
using Viasoft.Accounting.Domain.Dtos;
using Viasoft.Core.ServiceBus.Abstractions;
using ICommand = Viasoft.Core.ServiceBus.Abstractions.ICommand;

namespace Viasoft.Accounting.Host.Contracts
{
    [Endpoint("CreateAccountingEntry","Viasoft.Accounting")]
    public class CreateAccountingEntry : ICommand
    {
        public Guid SourceId { get; set; }
        
        public Guid AccountingOperationId { get; set; }
        
        public AccountingEntryDto AccountingEntry { get; set; }
        
        public List<AccountingEntryItemDto> AccountingEntryItems { get; set; }

    }
} 