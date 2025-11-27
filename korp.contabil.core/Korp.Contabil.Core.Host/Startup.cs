using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.API.Administration.Extensions;
using Viasoft.Core.API.Authentication.Extensions;
using Viasoft.Core.API.LicensingManagement.Extensions;
using Viasoft.Core.API.TenantManagement.Extensions;
using Viasoft.Core.API.UserProfile.Extensions;
using Viasoft.Core.AspNetCore.Extensions;
using Viasoft.Core.AspNetCore.Provisioning;
using Viasoft.Core.AspNetCore.UnitOfWork;
using Viasoft.Core.Authorization.AspNetCore.Extensions;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Core.Identity.AspNetCore.Extensions;
using Viasoft.Core.IoC.Extensions;
using Viasoft.Core.MultiTenancy.AspNetCore.Extensions;
using Viasoft.Core.Service;
using Viasoft.Core.ServiceBus.AspNetCore.Extensions;
using Viasoft.Core.ServiceDiscovery.Extensions;
using Viasoft.Core.Storage.Extensions;
using Korp.Contabil.Core.Infrastructure.EntityFrameworkCore;
using Viasoft.Core.API.Authorization.Extensions;
using Viasoft.Core.AspNetCore.ApiVersioning;
using Viasoft.Core.EntityFrameworkCore.SQLServer.Extensions;
using Viasoft.Core.MultiTenancy.Extensions;
using Viasoft.Core.MultiTenancy.Options;
using Viasoft.Core.ServiceBus.SQLServer.Extensions;

namespace Korp.Contabil.Core.Host;

public class Startup
{
    private readonly IConfiguration _configuration;

    public static IServiceConfiguration ServiceConfiguration => new ServiceConfiguration
    {
        ServiceName = "Korp.Contabil.Core",
        Domain = "contabil",
        Area = "core"
    };

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AspNetCoreDefaultConfiguration(options => { options.UseNewSerializer = true; },
            ServiceConfiguration, _configuration);

        services
            .AddPersistence(_configuration)
            .AddEfCore<LegacyContabilDbContext>(op => op.EnableLegacyMultiTenancyDbContextIntegration = true)
            .AddEfCoreSqlServer()
            .AddServiceBus(options => { options.CompressionOptions.Enabled = true; }, ServiceConfiguration,
                _configuration)
            .AddServiceBusSqlServerProvider()
            .AddServiceMesh()
            .AddApiClient(_configuration)
            .AddMultiTenancy(MultiTenancyOptions.Default().CompanyNotRequired().EnvironmentNotRequired().TenantNotRequired())
            .AddVersioning(_configuration)
            .AddDomainDrivenDesign()
            .RegisterDependenciesByConvention()
            .AddUserIdentity()
            .AddAuthorizations(_configuration, options => options.AcceptsOpenIdScope = false)
            .AddLegacyMultiTenancySupport()
            .AddAdministrationApi()
            .AddAuthorizationApi()
            .AddAuthenticationApi()
            .AddLicensingManagementApi()
            .AddTenantManagementApi()
            .AddUserProfileApi();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.AspNetCoreDefaultAppConfiguration()
            .UseProvisioning()
            .UseUnitOfWork()
            .UseEndpoints();
    }
}
