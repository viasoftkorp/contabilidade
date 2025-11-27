using Viasoft.Core.EntityFrameworkCore.SQLServer.DesignTime;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Accounting.Infrastructure.EntityFrameworkCore;

public class LegacyAccountingDbContextDesignTime : SqlServerBaseDesignTimeDbContextFactory<LegacyAccountingDbContext>
{
    public LegacyAccountingDbContextDesignTime()
    {
        AllowedParameters.Add(typeof(ITenantProperties));
    }
}
