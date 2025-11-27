using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;

public class TipoLancamentoService : ITipoLancamentoService, ITransientDependency
{
    private IRepository<TipoLancamento> _tipoLancamentos;

    public TipoLancamentoService(IRepository<TipoLancamento> tipoLancamentos)
    {
        _tipoLancamentos = tipoLancamentos;
    }

    public async Task<PagedResultDto<TipoLancamentoDto>> GetAllTipoLancamentos(PagedFilteredAndSortedRequestInput input)
    {
        var normalizedFilter = input.Filter is null ? "" : input.Filter.Trim().ToLower();
        var query = _tipoLancamentos
            .WhereIf(!string.IsNullOrEmpty(normalizedFilter), e => e.Descricao.ToLower().Contains(normalizedFilter))
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);
        if (string.IsNullOrEmpty(input.Sorting))
        {
            query = query.OrderByDescending(c => c.Descricao);
        }

        var totalCount = await query.CountAsync();

        query = query.PageBy(input.SkipCount, input.MaxResultCount);

        var tipoLancamentos = await query.AsNoTracking().ToListAsync();
        var tipoLancamentoOutput = tipoLancamentos.Select(tl => new TipoLancamentoDto(tl)).ToList();
        return new PagedResultDto<TipoLancamentoDto>(totalCount, tipoLancamentoOutput);
    }
}