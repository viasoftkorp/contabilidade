using System;
using System.Collections.Generic;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ServicosExternos.TenantManagement.DTOs
{
    public class GetOrganizationUnitsOutput
    {
        public int TotalCount { get; set; }
        public List<OrganizationsUnit> Items { get; set; }
    }

    public class OrganizationsUnit
    {
        public Guid Id { get; set; }          
        public bool IsActive { get; set; }          
    }
}