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

public class EmpresaService : IEmpresaService, ITransientDependency
{
    private IRepository<Empresa> _empresas;

    public EmpresaService(IRepository<Empresa> empresas)
    {
        _empresas = empresas;
    }

    public async Task<PagedResultDto<EmpresaDto>> GetAllEmpresas(PagedFilteredAndSortedRequestInput input)
    {
        var normalizedFilter = input.Filter is null ? "" : input.Filter.Trim().ToLower();
        var query = _empresas
            .WhereIf(!string.IsNullOrEmpty(normalizedFilter), e => e.Nome.ToLower().Contains(normalizedFilter))
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);
        if (string.IsNullOrEmpty(input.Sorting))
        {
            query = query.OrderByDescending(c => c.Nome);
        }

        var totalCount = await query.CountAsync();

        query = query.PageBy(input.SkipCount, input.MaxResultCount);

        var empresas = await query.AsNoTracking().ToListAsync();
        var empresasOutput = empresas.Select(e => new EmpresaDto(e)).ToList();
        return new PagedResultDto<EmpresaDto>(totalCount, empresasOutput);
    }
}