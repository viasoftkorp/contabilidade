using System;
using System.Threading.Tasks;

namespace Viasoft.Accounting.Host.Providers.Pessoas
{
    public interface IGetPersonProvider
    {
        Task<PersonOutput> Get(Guid id);
    }
}
