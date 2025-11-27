using System;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabil.Core.Domain.TiposItem;
using Korp.Contabil.Core.Domain.TiposItem.Dtos;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;

namespace Korp.Contabil.Core.Host.TiposItem.Services;

public class TipoItemService : ITipoItemService, ITransientDependency
{
    private readonly IReadOnlyRepository<TipoItem> _tiposItem;

    public TipoItemService(IReadOnlyRepository<TipoItem> tiposItem)
    {
        _tiposItem = tiposItem;
    }

    public async Task<TipoItemOutput> Get(Guid id)
    {
        var tipoItem = await _tiposItem.AsNoTracking()
            .FirstAsync(tipoItem => tipoItem.Id == id);

        var output = new TipoItemOutput(tipoItem);

        return output;
    }

    public async Task<PagedResultDto<TipoItemOutput>> GetList(PagedFilteredAndSortedRequestInput input)
    {
        var query = _tiposItem.AsNoTracking()
            .OrderBy(tipoItem => tipoItem.Codigo)
            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), tipoItem =>
                tipoItem.Codigo.ToLower().Contains(input.Filter.ToLower())
                || tipoItem.Descricao.ToLower().Contains(input.Filter.ToLower()))
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var totalCount = await query.CountAsync();

        var itens = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .Select(tipoItem => new TipoItemOutput(tipoItem))
            .ToListAsync();

        var output = new PagedResultDto<TipoItemOutput>(totalCount, itens);

        return output;
    }
}
