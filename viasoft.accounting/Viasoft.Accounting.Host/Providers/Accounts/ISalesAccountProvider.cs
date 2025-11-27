using System;
using System.Threading.Tasks;
using Viasoft.Accounting.Host.Providers.Accounts.Dtos;

namespace Viasoft.Accounting.Host.Providers.Accounts;

public interface ISalesAccountProvider
{
    Task<SalesAccountOutput> Get(Guid id);
}
