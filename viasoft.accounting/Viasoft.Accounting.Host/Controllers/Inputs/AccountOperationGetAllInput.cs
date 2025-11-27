using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Viasoft.Accounting.Host.Controllers.Inputs;

public class AccountOperationGetAllInput : PagedFilteredAndSortedRequestInput
{
    public bool? IssueInvoice { get; set; }
}