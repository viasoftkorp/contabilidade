using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Korp.Contabil.Core.Domain.TiposItem.Dtos;
using Korp.Contabil.Core.Host.TiposItem.Services;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabil.Core.Host.TiposItem.Controllers;

[ApiVersion(2024.2)]
[ApiVersion(2024.1)]
[Route("tipos-item")]
public class TipoItemController : BaseController
{
    private readonly ITipoItemService _tipoItemService;

    public TipoItemController(ITipoItemService tipoItemService)
    {
        _tipoItemService = tipoItemService;
    }

    [HttpGet("{id:guid}")]
    public async Task<TipoItemOutput> Get([FromRoute] Guid id)
    {
        var output = await _tipoItemService.Get(id);
        return output;
    }

    [HttpGet]
    public async Task<PagedResultDto<TipoItemOutput>> GetList([FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var output = await _tipoItemService.GetList(input);
        return output;
    }
}
