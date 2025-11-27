using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;
using Korp.Contabilidade.ConciliacaoContabil.Host.Contributors;
using Korp.Contabilidade.ConciliacaoContabil.Host.Sharing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.API.Administration.Extensions;
using Viasoft.Core.API.Authentication.Extensions;
using Viasoft.Core.API.EmailTemplate.Extensions;
using Viasoft.Core.API.LicensingManagement.Extensions;
using Viasoft.Core.API.Reporting.Extensions;
using Viasoft.Core.API.TenantManagement.Extensions;
using Viasoft.Core.API.UserProfile.Extensions;
using Viasoft.Core.AspNetCore.Extensions;
using Viasoft.Core.AspNetCore.Provisioning;
using Viasoft.Core.AspNetCore.UnitOfWork;
using Viasoft.Core.Authorization.AspNetCore.Extensions;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Core.EntityFrameworkCore.SQLServer.Extensions;
using Viasoft.Core.Identity.AspNetCore.Extensions;
using Viasoft.Core.IoC.Extensions;
using Viasoft.Core.MultiTenancy.AspNetCore.Extensions;
using Viasoft.Core.Service;
using Viasoft.Core.ServiceBus.AspNetCore.Extensions;
using Viasoft.Core.ServiceBus.SQLServer.Extensions;
using Viasoft.Core.ServiceDiscovery.Extensions;
using Viasoft.Core.Storage.Extensions;
using Korp.Contabilidade.ConciliacaoContabil.Infrastructure.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Viasoft.Core.API.Authorization.Extensions;
using Viasoft.Core.API.Emailing.Extensions;
using Viasoft.Core.Authentication.Proxy.Extensions;
using Viasoft.Core.Authorization.Proxy.Extensions;
using Viasoft.Core.Identity.Abstractions.AmbientData.Resolver.Contributor;
using Viasoft.Core.Mapper.Extensions;
using Viasoft.Core.MultiTenancy.Abstractions.Environment.Resolver.Contributors;
using Viasoft.Core.MultiTenancy.Options;
using Viasoft.PushNotifications.AspNetCore.Extensions;

namespace Korp.Contabilidade.ConciliacaoContabil.Host
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public static IServiceConfiguration ServiceConfiguration => new ServiceConfiguration
        {
            ServiceName = "Korp.Contabilidade.ConciliacaoContabil",
            Domain = "contabilidade",
            Area = "conciliacao-contabil",
            App = "",
            AppIdentifier = ""
        };
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AdicionarRepositoriosImpostos();
            services.AddMemoryCache();
            services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IEnvironmentResolveContributor), typeof(EnvironmentContributor)));
            services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IUserResolveContributor), typeof(UserContributor)));
            
			BuildExpressionOptions.Default.AddNullCheckExpression = false;
			BuildExpressionOptions.Default.CaseInsensitiveStringComparision = false;
            services.AddTransient<IApuracaoFactory, ApuracaoFactory>();
            services.AspNetCoreDefaultConfiguration(options =>
                {
                    options.UseNewSerializer = true;
                }, ServiceConfiguration, _configuration);
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                });
            services
                .AddPersistence(_configuration)
                .AddEfCore<ConciliacaoContabilDbContext>()
                .AddEfCoreSqlServer()
                .AddServiceBus(options =>
                {
                    options.CompressionOptions.Enabled = true;
                    options.JsonOptions.UseNewSerializer = true;
                    options.TimeoutOptions.Enabled = true;
                }, ServiceConfiguration, _configuration)
                .AddServiceBusSqlServerProvider()
                .AddServiceMesh()
                .AddApiClient(_configuration)
                .AddMultiTenancy(MultiTenancyOptions.Default().CompanyNotRequired())
                .AddDomainDrivenDesign()
                .RegisterDependenciesByConvention()
                .AddUserIdentity()
                .AddAuthorizations(_configuration, options => options.AcceptsOpenIdScope = true)
                .AddAdministrationApi()
                .AddAuthenticationApi()
                .AddEmailTemplateApi()
                .AddLicensingManagementApi()
                .AddReportingApi()
                .AddTenantManagementApi()
                .AddUserProfileApi()
                .AddEmailingApi()
                .AddAuthorizationApi()
                .AddAutoMapper()
                .AddNotification()
                .AddAuthenticationProxy()
                .AddAuthorizationProxy();
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