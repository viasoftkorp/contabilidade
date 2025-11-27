using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Accounting.Host.Controllers.Inputs;
using Viasoft.Accounting.Host.Controllers.Outputs;

namespace Viasoft.Accounting.Host.Controllers.Dtos
{
    public interface IHaveAutocomplete
    {
        Task<AutocompleteOutput> GetForAutocomplete([FromQuery] AutocompleteInput input);
        Task<List<NameValueDto>> GetAutocompleteOptions([FromQuery] List<Guid> ids);
    }
}