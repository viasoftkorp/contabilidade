using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Viasoft.Core.AmbientData;
using Viasoft.Core.AmbientData.Abstractions;
using Viasoft.Core.AmbientData.Extensions;
using Viasoft.Core.Authentication.Proxy;
using Viasoft.Core.Authentication.Proxy.Dtos.Inputs;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.Identity.Abstractions.AmbientData.Resolver.Contributor;
using Viasoft.Core.ServiceBus.Extensions;

namespace Korp.Contabilidade.ConciliacaoContabil.Host.Contributors;

public class UserContributor: IUserResolveContributor
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IAmbientData _ambientData;
    private readonly IAuthenticationProxyService _authenticationApi;
    private readonly IAmbientDataCallOptionsResolver _ambientDataCallOptionsResolver;

    public UserContributor(IHttpContextAccessor httpContextAccessor, IAmbientData ambientData, IAuthenticationProxyService authenticationApi, IAmbientDataCallOptionsResolver ambientDataCallOptionsResolver)
    {
        _contextAccessor = httpContextAccessor;
        _ambientData = ambientData;
        _authenticationApi = authenticationApi;
        _ambientDataCallOptionsResolver = ambientDataCallOptionsResolver;
    }

    public async ValueTask<AmbientDataResolveResult> TryResolveUser()
    {
       if (!MessageContextExtensions.IsHandlingMessage() &&
            _contextAccessor?.HttpContext?.Request != null && 
            _contextAccessor.HttpContext.Request.Headers.TryGetValue("UserLogin", out var userLogin))
        {
            var userId = await GetUserId(userLogin);
            _ambientData.SetUserId(userId);
            return await ValueTask.FromResult(AmbientDataResolveResult<Guid>.Handle(userId));
        }

        return await ValueTask.FromResult(AmbientDataResolveResult.NotHandled);
    }
    
    private async Task<Guid> GetUserId(StringValues userLogin)
    {
        var advancedFilter = new JsonNetFilterRule
        {
            Condition = "and",
            Rules = new List<JsonNetFilterRule>
            {
                new JsonNetFilterRule
                {
                    Value = userLogin.First(),
                    Operator = "equal",
                    Type = "string",
                    Field = "login"
                }
            }
        };
        var response = await _authenticationApi.UsersGetAll(new GetAllInput
        {
            AdvancedFilter = JsonConvert.SerializeObject(advancedFilter),
            MaxResultCount = 1
        }, _ambientDataCallOptionsResolver.GetOptions());
        return response.Items.First().Id;
    }

}