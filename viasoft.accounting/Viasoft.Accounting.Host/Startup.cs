using System;
using System.Collections.Generic;
using Korp.EntidadesLegadas.ACL.Core.AmbientData;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Viasoft.Accounting.Infrastructure.EntityFrameworkCore;
using Viasoft.Accounting.Infrastructure.Seeders;
using Viasoft.Core.API.Administration.Extensions;
using Viasoft.Core.API.Authentication.Extensions;
using Viasoft.Core.API.Authorization.Extensions;
using Viasoft.Core.API.EmailTemplate.Extensions;
using Viasoft.Core.API.LicensingManagement.Extensions;
using Viasoft.Core.API.Reporting.Extensions;
using Viasoft.Core.API.TenantManagement.Extensions;
using Viasoft.Core.API.UserProfile.Extensions;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.AspNetCore.ApiVersioning;
using Viasoft.Core.AspNetCore.Extensions;
using Viasoft.Core.Service;
using Viasoft.Core.AspNetCore.Provisioning;
using Viasoft.Core.AspNetCore.UnitOfWork;
using Viasoft.Core.Authorization.AspNetCore.Extensions;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Core.EntityFrameworkCore.SQLServer.Extensions;
using Viasoft.Core.Identity.AspNetCore.Extensions;
using Viasoft.Core.IoC.Extensions;
using Viasoft.Core.Mapper.Extensions;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Core.MultiTenancy.AspNetCore.Extensions;
using Viasoft.Core.MultiTenancy.Extensions;
using Viasoft.Core.MultiTenancy.Options;
using Viasoft.Core.ServiceBus.AspNetCore.Extensions;
using Viasoft.Core.ServiceBus.SQLServer.Extensions;
using Viasoft.Core.ServiceDiscovery.Extensions;
using Viasoft.Core.Storage.Extensions;
using Viasoft.Data.Extensions;
using Viasoft.Data.Seeder.Extensions;
using Viasoft.PushNotifications.AspNetCore.Extensions;


namespace Viasoft.Accounting.Host
{
    public class Startup
    {
        public static IServiceConfiguration ServiceConfiguration => new ServiceConfiguration
        {
            ServiceName = "Viasoft.Accounting",
            Domain = "Accounting"
        };
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAdministrationApi()
                .AddAuthorizationApi()
                .AddAuthenticationApi()
                .AddEmailTemplateApi()
                .AddLicensingManagementApi()
                .AddReportingApi()
                .AddTenantManagementApi()
                .AddUserProfileApi()
                .AddTransient<ITenantPropertiesVisitor, TenantTimezonePropertiesVisitor>()
                .AddPersistence(_configuration)
                .AddVersioning(_configuration)
                .AddEfCore(options => { options.EnableLegacyMultiTenancyDbContextIntegration = true; }, new List<Type> {typeof(AccountingDbContext), typeof(LegacyAccountingDbContext)})
                .AddEfCoreSqlServer()
                .AddServiceBus(options => { options.SagasOptions.Enabled = true; }, ServiceConfiguration, _configuration)
                .AddServiceBusSqlServerProvider()
                .AddMultiTenancy(MultiTenancyOptions.Default().CompanyNotRequired())
                .AddServiceMesh()
                .AddDomainDrivenDesign()
                .AddAutoMapper()
                .AddApiClient(_configuration,options => options.DefaultTimeoutPeriod = ApiServiceCallTimeout.Long)
                .AddSeeders(new[]
                {
                    typeof(StandardHistoricSeeder),
                })
                .RegisterDependenciesByConvention()
                .AddNotification()
                .AddUserIdentity()
                .AddLegacyMultiTenancySupport()
                .AddAuthorizations(_configuration)
                .AddFilterDataFilter<LegacyFilterDataFilter>()
                .AspNetCoreDefaultConfiguration(ServiceConfiguration, _configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.AspNetCoreDefaultAppConfiguration()
                .UseProvisioning()
                .UseUnitOfWork()
                .UseEndpoints();
        }
    }
}
