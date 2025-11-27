using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ServicosExternos.LegacyAdministration.DTOs;
using Viasoft.Core.AmbientData.Abstractions;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.ApiClient.HttpHeaderStrategy;
using Viasoft.Core.IoC.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ServicosExternos.LegacyAdministration;

public class LegacyAdministrationService : ILegacyAdministrationService, ITransientDependency
{
    protected const string BasePath = "administracao";
    protected const string ServiceName = "Korp.Legacy.Administration";
    protected readonly IApiClientCallBuilder ApiClientCallBuilder;

    public LegacyAdministrationService(IApiClientCallBuilder apiClientCallBuilder)
    {
        ApiClientCallBuilder = apiClientCallBuilder;
    }

    public async Task<List<CompanyMatriz>> GetCompaniesByUser(string usuario, AmbientDataCallOptions options)
    {
        var endpoint = $"{BasePath}/empresas";
        var call = GetCall(endpoint, HttpMethod.Get, options, null, new { usuario = "master" });
        var response = await call.CallAsync<GetCompaniesByUserOutput>();
        var companies = (await response.GetResponse()).Companies;
        return NormalizeCompanies(companies);
    }

    private List<CompanyMatriz> NormalizeCompanies(List<Company> companies)
    {
        if(companies is null || companies.Count == 0)
            return new List<CompanyMatriz>();
        
        var companyMatriz = new List<CompanyMatriz>();
        var companyFilial = new List<Company>();
        foreach (var company in companies)
        {
            if (company.LegacyCompanyIdMatriz == 0)
            {
                companyMatriz.Add(new CompanyMatriz(company));
            }
            else
            {
                companyFilial.Add(company);
            }
        }

        foreach (var company in companyFilial)
        {
            var companyMatrizParent = companyMatriz.Find(c => c.LegacyCompanyId == company.LegacyCompanyIdMatriz);
            if (companyMatrizParent != null)
            {
                companyMatrizParent.Filiais.Add(company);
            }
        }

        return companyMatriz;
    }

    protected IApiClientCall GetCall(string endpoint, HttpMethod method, AmbientDataCallOptions callOptions,
        object body = null, object parameters = null, ApiServiceCallTimeout? timeout = null)
    {
        var normalizedEndpoint = endpoint;
        if (parameters != null)
        {
            if (parameters is string parametersString)
            {
                if (!string.IsNullOrEmpty(parametersString))
                {
                    parametersString = parametersString.TrimStart('?');
                    normalizedEndpoint += $"?{parametersString}";
                }
            }
            else
            {
                var queryParams = parameters.ToHttpGetQueryParameter();
                if (!string.IsNullOrEmpty(queryParams))
                    normalizedEndpoint += $"?{queryParams}";
            }
        }

        var call = ApiClientCallBuilder.WithEndpoint(normalizedEndpoint)
            .WithServiceName(ServiceName)
            .WithHttpMethod(method)
            .WithHttpHeaderStrategy(GetHeaderStrategy(callOptions))
            .DontThrowOnFailureCall();
        if (body != null)
            call = call.WithBody(body);
        if (timeout.HasValue)
            call = call.WithTimeout(timeout.Value);
        return call.Build();
    }

    protected IHttpHeaderStrategy GetHeaderStrategy(AmbientDataCallOptions callOptions)
    {
        return new AmbientDataCallOptionsHttpHeaderStrategy(callOptions);
    }
}