using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ServicosExternos.LegacyAdministration.DTOs;
using Viasoft.Core.AmbientData.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ServicosExternos.LegacyAdministration;

public interface ILegacyAdministrationService
{
    Task<List<CompanyMatriz>> GetCompaniesByUser(string usuario, AmbientDataCallOptions options);
}