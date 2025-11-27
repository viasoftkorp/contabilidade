using System;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Data.Attributes;

namespace Viasoft.Accounting.Host.Controllers.Inputs.Gets
{
    public class GetAllAccountingOperationEntryRulesInput : PagedFilteredAndSortedRequestInput
    {
        [StrictRequired]
        public Guid AccountingOperationId { get; set; }
    }
}