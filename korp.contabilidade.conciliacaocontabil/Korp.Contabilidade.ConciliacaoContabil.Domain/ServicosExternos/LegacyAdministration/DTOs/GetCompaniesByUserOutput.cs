using System.Collections.Generic;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ServicosExternos.LegacyAdministration.DTOs;

public class GetCompaniesByUserOutput
{
    public List<Company> Companies { get; set; }
}