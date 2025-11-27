using System;
using System.Collections.Generic;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ServicosExternos.TenantManagement.DTOs
{
    public class GetEnvironmentByUnitOutput
    {
        public int TotalCount { get; set; }
        public List<OrganizationsUnitEnvironment> Items { get; set; }
    }
    
    public class OrganizationsUnitEnvironment
    {
        public Guid Id { get; set; } 
        public string DatabaseName { get; set; } 
        public bool IsActive { get; set; }    
    }
}