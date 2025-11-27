using System;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ServicosExternos.TenantManagement;
using Microsoft.AspNetCore.Http;
using Viasoft.Core.AmbientData;
using Viasoft.Core.MultiTenancy.Abstractions.Environment.Resolver.Contributors;
using Viasoft.Core.ServiceBus.Extensions;

namespace Korp.Contabilidade.ConciliacaoContabil.Host.Contributors
{
    public class EnvironmentContributor: IEnvironmentResolveContributor
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITenantManagementService _tenantManagementService;
        private readonly IAmbientData _ambientData;
        public int? Order { get; set; }

        public EnvironmentContributor(IHttpContextAccessor contextAccessor, ITenantManagementService tenantManagementService, IAmbientData ambientData)
        {
            _contextAccessor = contextAccessor;
            _tenantManagementService = tenantManagementService;
            _ambientData = ambientData;
        }
        
        public async ValueTask<AmbientDataResolveResult> TryResolveEnvironment()
        {
            if (!MessageContextExtensions.IsHandlingMessage() &&
                _contextAccessor?.HttpContext?.Request != null &&
                _contextAccessor.HttpContext.Request.Headers.TryGetValue("TenantId", out var tenantId)
                && _contextAccessor.HttpContext.Request.Headers.TryGetValue("DatabaseName", out var databaseName))
            {
                var environmentId = await _tenantManagementService.GetEnvironmentId(Guid.Parse(tenantId), databaseName);
                return await ValueTask.FromResult(AmbientDataResolveResult<Guid>.Handle(environmentId));
            }
            
            return await ValueTask.FromResult(AmbientDataResolveResult.NotHandled);
        }
    }
}