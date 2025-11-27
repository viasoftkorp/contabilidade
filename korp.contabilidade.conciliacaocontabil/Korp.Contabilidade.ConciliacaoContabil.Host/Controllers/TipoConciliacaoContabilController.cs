using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Repositories;

namespace Korp.Contabilidade.ConciliacaoContabil.Host.Controllers;

public class TipoConciliacaoContabilController : BaseReadonlyController<TipoConciliacaoContabil, TipoConciliacaoContabil, PagedFilteredAndSortedRequestInput, string>
{
    private readonly ITipoConciliacaoContabilService _tipoConciliacaoContabilService;

    protected override (Expression<Func<TipoConciliacaoContabil, string>>, bool) DefaultGetAllSorting()
    {
        return (contabil => contabil.Descricao, true);
    }

    protected override IQueryable<TipoConciliacaoContabil> ApplyCustomFilters(IQueryable<TipoConciliacaoContabil> query, PagedFilteredAndSortedRequestInput input)
    {
        return query.Where(contabil => contabil.Ativo);
    }

    public TipoConciliacaoContabilController(IReadOnlyRepository<TipoConciliacaoContabil> repository, IMapper mapper, ITipoConciliacaoContabilService tipoConciliacaoContabilService) : base(repository, mapper)
    {
        _tipoConciliacaoContabilService = tipoConciliacaoContabilService;
    }

    [HttpGet("/contabilidade/conciliacao-contabil/tipo-conciliacao")]
    public override Task<PagedResultDto<TipoConciliacaoContabil>> GetAll(PagedFilteredAndSortedRequestInput input)
    {
        return base.GetAll(input);
    }

    [HttpPost("/contabilidade/conciliacao-contabil/tipo-conciliacao/{legacyId}")]
    public async Task<IActionResult> AdicionarConta([FromRoute] int legacyId, [FromBody] AdicionarContaInput input)
    {
        var status = await _tipoConciliacaoContabilService.AdicionarConta(legacyId, input);

        return status switch
        {
            AdicionarContaResponseEnum.Ok => StatusCode(201),
            _ => UnprocessableEntity(new {status})
        };
    }
    
    [HttpGet("/contabilidade/conciliacao-contabil/tipo-conciliacao/{legacyId}/contas")]
    public async Task<IActionResult> BuscarTodasContasPorConciliacao([FromRoute] int legacyId, [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var contas = await _tipoConciliacaoContabilService.BuscarTodasContasPorConciliacao(legacyId, input);
        return Ok(contas);
    }
    
    [HttpDelete("/contabilidade/conciliacao-contabil/tipo-conciliacao/{legacyId}/contas/{id}")]
    public async Task<IActionResult> DeletarConta([FromRoute] int legacyId, [FromRoute] int id, [FromQuery] bool? shouldRemoveLinkedAccounts)
    {
        var status = await _tipoConciliacaoContabilService.DeletarConta(legacyId, id, shouldRemoveLinkedAccounts);
        return status switch
        {
            RemoverContaResponseEnum.Ok => StatusCode(200),
            _ => UnprocessableEntity(new {status})
        };
    }
}