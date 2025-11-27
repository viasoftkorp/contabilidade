using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Viasoft.Accounting.Domain.TenantDbDiscovery;
using Viasoft.Accounting.Domain.Utils;
using Viasoft.Core.ApiClient;
using Viasoft.Core.Identity.Abstractions;
using Viasoft.Core.Identity.Abstractions.Store;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;

namespace Viasoft.Accounting.Domain.Services.External
{
    

    public class GetCodeAccountEntryService : IGetCodeAccountEntryService, ITransientDependency
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ICurrentCompany _currentCompany;
        private readonly ITenantDbDiscoveryService _tenantDbDiscoveryService;
        private readonly ICurrentUser _currentUser;
        private readonly IUserStore _userStore;
        private readonly IApiClientCallBuilder _apiClientCallBuilder;
        private readonly ICurrentEnvironment _currentEnvironment;
        private readonly IEnvironmentStore _environmentStore;
        private readonly ILogger<GetCodeAccountEntryService> _logger;


        public GetCodeAccountEntryService(IHttpClientFactory clientFactory, ICurrentCompany currentCompany, ITenantDbDiscoveryService tenantDbDiscoveryService, ICurrentUser currentUser, IUserStore userStore, IApiClientCallBuilder apiClientCallBuilder, ICurrentEnvironment currentEnvironment, IEnvironmentStore environmentStore, ILogger<GetCodeAccountEntryService> logger)
        {
            _clientFactory = clientFactory;
            _currentCompany = currentCompany;
            _tenantDbDiscoveryService = tenantDbDiscoveryService;
            _currentUser = currentUser;
            _userStore = userStore;
            _apiClientCallBuilder = apiClientCallBuilder;
            _currentEnvironment = currentEnvironment;
            _environmentStore = environmentStore;
            _logger = logger;
        }

        public async Task<EntryCodeResponseOutput> GetEntryCode()
        {

            try
            {
                var currentUser = await _userStore.GetUserDetailsAsync(_currentUser.Id);
                var environment = await _environmentStore.GetEnvironmentAsync(_currentEnvironment.Id.Value);

                using (var client = _clientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("IdEmpresa", _currentCompany.LegacyId.ToString());
                    client.DefaultRequestHeaders.Add("Usuario", currentUser.Login);
                    var dbName = await _tenantDbDiscoveryService.DbName();
                    var serverIp = await _tenantDbDiscoveryService.ServerIp();
                    var externalEndpoint = GetEndpoint(serverIp, dbName, environment.VersionWithoutBuild());
                    var apiResponse = await client.PostAsync(externalEndpoint, null);
                    var result = await apiResponse.Content.ReadAsStringAsync();
                    _logger.LogInformation(result);
                    var convert = JsonConvert.DeserializeObject<EntryCodeResponse>(result);
                    var output = new EntryCodeResponseOutput
                    {
                        Success = true,
                        Value = convert.Value
                    };
                    return output;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return new EntryCodeResponseOutput
                {
                    Error = new Error{
                        Message = e.StackTrace
                    },
                    Success = false,
                    Value = 0
                };
            }
        }

        private string GetEndpoint(string ip, string dbName, string version)
        {
            return $"{ip}/Korp/Services/{version}/Fiscal/{dbName}/Tributos/BuscarProximoIdentificadorLancamentoContabil";
        }

    }
}