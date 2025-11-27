using System.Threading.Tasks;

namespace Viasoft.Accounting.Domain.TenantDbDiscovery
{
    public interface ITenantDbDiscoveryService
    {
        Task<string> DbName();
        Task<string> ServerIp();
    }
}