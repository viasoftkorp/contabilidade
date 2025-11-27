using System;
using System.Collections.Generic;
using Viasoft.Core.DDD.DataFilter;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Core.MultiTenancy.DataFilter;
using Viasoft.Core.MultiTenancy.Extensions;
using Viasoft.Data.DataFilter;

namespace Viasoft.Accounting.Host
{

    public class LegacyFilterDataFilter : IFilterDataFilter
    {
        private readonly ICurrentTenant _currentTenant;

        public LegacyFilterDataFilter(ICurrentTenant currentTenant)
        {
            _currentTenant = currentTenant;
        }

        public bool FilterOutDataFilter(FilterDataFilterContext context)
        {
            if (_currentTenant.IsLegacyModeEnabled())
            {
                return new List<Type>
                {
                    typeof(MustHaveTenantDataFilter), typeof(SoftDeleteDataFilter),
                    typeof(MustHaveEnvironmentDataFilter)
                }.Contains(context.DataFilter.GetType());
            }

            return false;

        }
    }
}