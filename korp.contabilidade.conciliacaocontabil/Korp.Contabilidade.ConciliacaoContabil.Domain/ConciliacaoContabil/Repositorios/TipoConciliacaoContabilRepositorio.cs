using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;
using Z.EntityFramework.Plus;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;

public class TipoConciliacaoContabilRepositorio : ITipoConciliacaoContabilRepositorio, ITransientDependency
{
    private readonly IRepository<TipoConciliacaoContabil> _repository;
    private readonly IRepository<TipoConciliacaoContabilConta> _conciliacaoContabilContas;
    private readonly ILogger<TipoConciliacaoContabilRepositorio> _logger;

    public TipoConciliacaoContabilRepositorio(IRepository<TipoConciliacaoContabil> repository,
        ILogger<TipoConciliacaoContabilRepositorio> logger,
        IRepository<TipoConciliacaoContabilConta> conciliacaoContabilContas)
    {
        _repository = repository;
        _logger = logger;
        _conciliacaoContabilContas = conciliacaoContabilContas;
    }

    public async Task<TipoConciliacaoContabil> BuscarConciliacaoContabilTipo(
        TipoApuracaoConciliacaoContabil tipoApuracaoConciliacao)
    {
        return await _repository.FirstAsync(l => l.TipoApuracao == tipoApuracaoConciliacao);
    }

    public async Task AdicionarConta(TipoConciliacaoContabilConta conta)
    {
        await _conciliacaoContabilContas.InsertAsync(conta);
    }

    public async Task AdicionarContas(IEnumerable<TipoConciliacaoContabilConta> contas)
    {
        await _conciliacaoContabilContas.InsertRangeAsync(contas);
    }

    public async Task<ListResultDto<TipoConciliacaoContabilConta>> BuscarTodasContasPorConciliacao(int legacyId,
        PagedFilteredAndSortedRequestInput input)
    {
        var normalizedFilter = input.Filter is null ? "" : input.Filter.Trim().ToLower();
        var query = _conciliacaoContabilContas.Where(x => x.IdTipoConciliacaoContabil == legacyId)
            .WhereIf(!string.IsNullOrEmpty(normalizedFilter), p => p.Descricao.ToLower().Contains(normalizedFilter))
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        if (string.IsNullOrEmpty(input.Sorting))
        {
            query = query.OrderBy(c => c.CodigoConta);
        }

        var queryPaged = query.PageBy(input.SkipCount, input.MaxResultCount);
        var contas = await queryPaged.AsNoTracking().ToListAsync();
        return new ListResultDto<TipoConciliacaoContabilConta>(contas);
    }

    public async Task<TipoConciliacaoContabilConta> GetConta(int legacyId, int id)
    {
       return await _conciliacaoContabilContas
            .Where(conta => conta.IdTipoConciliacaoContabil == legacyId && conta.LegacyId == id)
            .FirstOrDefaultAsync();
    }

    public async Task DeletarConta(int legacyId, int id)
    {
        await _conciliacaoContabilContas
            .Where(conta => conta.IdTipoConciliacaoContabil == legacyId && conta.LegacyId == id)
            .DeleteAsync();
    }
    
    public async Task DeletarContas(int legacyId, List<int> ids)
    {
        await _conciliacaoContabilContas
            .Where(conta => conta.IdTipoConciliacaoContabil == legacyId && ids.Contains(conta.CodigoConta))
            .DeleteAsync();
    }

    public async Task<TipoConciliacaoContabil> BuscarTipoConciliacaoContabilPorId(int legacyId)
    {
        _logger.LogInformation("Buscando tipo conciliação {legacyId}", legacyId);

        var tipoConciliacao = await _repository.Where(contabil => contabil.LegacyId == legacyId)
            .FirstOrDefaultAsync();

        _logger.LogInformation("Buscou tipo conciliação {legacyId}", legacyId);

        return tipoConciliacao;
    }
}