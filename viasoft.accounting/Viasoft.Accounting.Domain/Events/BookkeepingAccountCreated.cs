using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Accounting.Domain.Events;

[Endpoint("Viasoft.Accounting.BookkeepingAccountCreated")]
public class BookkeepingAccountCreated : IEvent
{
    public Guid BookkeepingAccountId { get; set; }
    public Guid SalesAccountId { get; set; }

    public BookkeepingAccountCreated() { }

    public BookkeepingAccountCreated(Guid bookkeepingAccountId, Guid salesAccountId)
    {
        BookkeepingAccountId = bookkeepingAccountId;
        SalesAccountId = salesAccountId;
    }
}
