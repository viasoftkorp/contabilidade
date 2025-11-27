using System.Collections.Generic;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ServicosExternos.LegacyAdministration.DTOs;

public class CompanyMatriz : Company
{
    public List<Company> Filiais { get; set; }
    
    public CompanyMatriz(Company company)
    {
        Id = company.Id;
        LegacyCompanyId = company.LegacyCompanyId;
        LegacyCompanyIdMatriz = company.LegacyCompanyIdMatriz;
        Cnpj = company.Cnpj;
        StateRegistration = company.StateRegistration;
        CompanyName = company.CompanyName;
        TradingName = company.TradingName;
        Phone = company.Phone;
        Email = company.Email;
        CompanyAddressInfo = company.CompanyAddressInfo;
        Filiais = new List<Company>();
    }
}