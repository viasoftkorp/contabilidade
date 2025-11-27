using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.IoC.Abstractions;

namespace Viasoft.Accounting.Host.Providers.Pessoas
{
    public class GetPersonProvider : IGetPersonProvider, ITransientDependency
    {
        private readonly IApiClientCallBuilder _apiClientCallBuilder;
        private const string PersonServiceName = "Viasoft.ERP.Person";
        private string GetPersonByIdEndpoint(Guid id) => $"ERP/Person/Person/getById/{id}";
        public GetPersonProvider(IApiClientCallBuilder apiClientCallBuilder)
        {
            _apiClientCallBuilder = apiClientCallBuilder;
        }

        public async Task<PersonOutput> Get(Guid id)
        {
            var call = _apiClientCallBuilder
                .WithServiceName(PersonServiceName)
                .WithEndpoint(GetPersonByIdEndpoint(id))
                .WithHttpMethod(HttpMethod.Get)
                .Build();

            var output = await call.ResponseCallAsync<PersonOutput>();
            return output;
        }
    }
}
