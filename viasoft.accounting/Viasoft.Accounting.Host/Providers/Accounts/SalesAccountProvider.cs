using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Accounting.Host.Providers.Accounts.Dtos;
using Viasoft.Core.ApiClient;
using Viasoft.Core.IoC.Abstractions;

namespace Viasoft.Accounting.Host.Providers.Accounts;

public class SalesAccountProvider : ISalesAccountProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Viasoft.Sales.CRM.Accounts";
    private const string BaseEndpoint = "Sales/CRM/Accounts";

    public SalesAccountProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public async Task<SalesAccountOutput> Get(Guid id)
    {
        var call = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BaseEndpoint}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var output = await call.ResponseCallAsync<SalesAccountOutput>();

        return output;
    }
}
