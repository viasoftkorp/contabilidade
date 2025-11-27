using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Viasoft.Accounting.Host.Controllers.Inputs.Gets;

public class GetAllBokkepingAccountsInput : PagedFilteredAndSortedRequestInput
{
    public bool Synthetic { get; set; }

    public GetAllBokkepingAccountsInput() { }
}
