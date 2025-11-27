using Viasoft.Core.MultiTenancy.Abstractions.Company.Model;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ServicosExternos.LegacyAdministration.DTOs;

public class Company
{
    public string Id { get; set; }
    public int LegacyCompanyId { get; set; }
    public int? LegacyCompanyIdMatriz { get; set; }
    public string Cnpj { get; set; }
    public string StateRegistration { get; set; }
    public string CompanyName { get; set; }
    public string TradingName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public CompanyAddressInfo CompanyAddressInfo { get; set; }
}