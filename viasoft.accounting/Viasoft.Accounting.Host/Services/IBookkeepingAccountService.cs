using System.Threading.Tasks;
using Viasoft.Accounting.Host.Controllers.Inputs;

namespace Viasoft.Accounting.Host.Services;

public interface IBookkeepingAccountService
{
    Task Create(BookkeepingAccountInput input);
}
