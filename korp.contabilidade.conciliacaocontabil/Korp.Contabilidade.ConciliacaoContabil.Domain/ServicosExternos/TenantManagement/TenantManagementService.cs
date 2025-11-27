using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Viasoft.Core.API.TenantManagement;
using Viasoft.Core.API.TenantManagement.Model;
using Viasoft.Core.IoC.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ServicosExternos.TenantManagement
{
    public class TenantManagementService: ITenantManagementService, ITransientDependency
    {
        private readonly ITenantManagementApi _tenantManagementApi;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<TenantManagementService> _logger;

        public TenantManagementService(ITenantManagementApi tenantManagementApi, IMemoryCache memoryCache, ILogger<TenantManagementService> logger)
        {
            _tenantManagementApi = tenantManagementApi;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<Guid> GetEnvironmentId(Guid tenantId, string databaseName)
        {
            var key = tenantId + "_" + databaseName.ToLower();
            if (_memoryCache.TryGetValue(key, out var value))
            {
                return (Guid)value;
            }
            
            var callResponse = await _tenantManagementApi.GetEnvironmentsAsync(new TenantManagementApiGetEnvironments
            {
                MaxResultCount = 250,
                TenantIds = new List<Guid> { tenantId }
            });

            var environments = await callResponse.GetResponse();
            foreach (var environment in environments.Items)
            {
                if (string.Equals(environment.DatabaseName, databaseName, StringComparison.CurrentCultureIgnoreCase))
                {
                    _memoryCache.Set(key, environment.Id, TimeSpan.FromHours(8));
                    return environment.Id;
                }
            }

            _logger.LogError("Não foi possível encontrar um environment para o database {0}, tenant {1}", databaseName, tenantId);
            return Guid.Empty;
        }
    }
}