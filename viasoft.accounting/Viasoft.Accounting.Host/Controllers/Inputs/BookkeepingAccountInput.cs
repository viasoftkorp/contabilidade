using System;

namespace Viasoft.Accounting.Host.Controllers.Inputs;

public class BookkeepingAccountInput
{
    public Guid SalesAccountId { get; set; }
    public Guid ParentBookkeepingAccountId { get; set; }

    public BookkeepingAccountInput() { }
}
