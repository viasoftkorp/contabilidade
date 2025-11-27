using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Accounting.Host.Contracts
{
    [Endpoint("AccountingOperationDeleted")]
    public class AccountingOperationDeleted : IEvent
    {
        public Guid AccountingOperationId { get; set; }
    }
}