using System;
using System.Threading.Tasks;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ServicosExternos.TenantManagement
{
    public interface ITenantManagementService
    {
        public Task<Guid> GetEnvironmentId(Guid tenantId, string databaseName);
    }
}