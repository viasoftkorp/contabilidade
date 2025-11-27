using System;
using Korp.EntidadesLegadas.ACL.Core.AmbientData;
using Microsoft.Extensions.DependencyInjection;
using Viasoft.Accounting.Host;
using Viasoft.Accounting.Infrastructure.EntityFrameworkCore;
using Viasoft.Core.EntityFrameworkCore.Options;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Core.Testing;
using Viasoft.Data.Extensions;

namespace Viasoft.Accounting.UnitTest;

public class UnitTestBaseWithDbContext : UnitTestBase
{

    protected override void AddEfCoreServices()
    {
        base.AddEfCoreServices();

        Action<EfCoreOptions> optionsFunc = options =>
        {
            options.EnableLegacyMultiTenancyDbContextIntegration = true;
            options.DbContextTypes.Add(typeof(LegacyAccountingDbContext));
        };

        ServiceCollection.Configure(optionsFunc);
        ServiceCollection.AddFilterDataFilter<LegacyFilterDataFilter>();
    }

    protected override void ConfigureServices()
    {
        var tenantProperties = ServiceProvider.GetService<ITenantProperties>();
        tenantProperties.SetIanaTimezone("GMT Standard Time");
        base.ConfigureServices();
    }
}
