using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Accounting.Domain.Services;
using Viasoft.Accounting.Host.Controllers.Outputs;
using Viasoft.Core.AspNetCore.Controller;

namespace Viasoft.Accounting.Host.Controllers
{
        [ApiVersion(2023.4), ApiVersion(2023.3), ApiVersion(2023.2), ApiVersion(2023.1), ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    public class CteVariablesController : BaseController
    {
        private readonly ICteVariablesService _cteVariablesService;
        
        public CteVariablesController(ICteVariablesService cteVariablesService)
        {
            _cteVariablesService = cteVariablesService;
        }

        
        [HttpGet]
        public Task<List<CteVariableSelectOutput>> GetCteTaxVariablesSelection()
        {

            return Task.FromResult(_cteVariablesService.GetCteTaxVariablesForSelect().Select(variable => new CteVariableSelectOutput(variable)).ToList());

        }        
        [HttpGet]
        public Task<List<CteVariableSelectOutput>> GetCteAccountingVariablesSelection()
        {

            return Task.FromResult(_cteVariablesService.GetCteAccountingVariablesForSelect().Select(variable => new CteVariableSelectOutput(variable)).ToList());

        }

    }
}