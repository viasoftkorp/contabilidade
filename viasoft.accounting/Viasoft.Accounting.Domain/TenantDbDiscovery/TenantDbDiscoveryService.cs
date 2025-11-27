using System.Threading.Tasks;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant.Store;

namespace Viasoft.Accounting.Domain.TenantDbDiscovery
{
    public class TenantDbDiscoveryService : ITenantDbDiscoveryService, ITransientDependency
    {
        private readonly ITenancyStore _tenantInfrastructureConfigurationProvider;
        private readonly IEnvironmentStore _environmentService;
        private readonly ICurrentTenant _currentTenant;
        private readonly ICurrentEnvironment _currentEnvironment;

        private string _dbName;
        private string _gatewayAddress;

        public TenantDbDiscoveryService(ITenancyStore tenantInfrastructureConfigurationProvider, ICurrentTenant currentTenant, ICurrentEnvironment currentEnvironment, IEnvironmentStore environmentService)
        {
            _tenantInfrastructureConfigurationProvider = tenantInfrastructureConfigurationProvider;
            _currentTenant = currentTenant;
            _currentEnvironment = currentEnvironment;
            _environmentService = environmentService;
        }


        public async Task<string> DbName()
        {
            if (string.IsNullOrWhiteSpace(_dbName))
            {
                var configuration = await _tenantInfrastructureConfigurationProvider.GetInfrastructureConfigurationAsync(_currentTenant.Id);
                var environmentDetails = await _environmentService.GetEnvironmentAsync(_currentEnvironment.Id.Value);
                _dbName = environmentDetails.DatabaseName;
                _gatewayAddress = configuration.GatewayAddress; 
            }
            return _dbName;
        }
        public async Task<string> ServerIp()
        {
            if (string.IsNullOrWhiteSpace(_gatewayAddress))
            {
                var configuration = await _tenantInfrastructureConfigurationProvider.GetInfrastructureConfigurationAsync(_currentTenant.Id);
                var environmentDetails = await _environmentService.GetEnvironmentAsync(_currentEnvironment.Id.Value);
                _dbName = environmentDetails.DatabaseName;
                _gatewayAddress = configuration.GatewayAddress; 
            }
            return _gatewayAddress;
        }
    }
}