using System.Collections.Generic;
using Viasoft.Accounting.Domain.Entities;

namespace Viasoft.Accounting.Domain.Dtos
{
    public class AccountingEntryItemWithOriginsDto
    {
        public AccountingEntryItem EntryItem { get; set; }
        public List<AccountingEntryItemOrigin> Origins { get; set; }
    }
}