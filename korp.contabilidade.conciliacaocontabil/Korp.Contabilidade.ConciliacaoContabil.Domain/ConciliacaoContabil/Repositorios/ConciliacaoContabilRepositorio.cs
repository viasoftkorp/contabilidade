using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
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

public class ConciliacaoContabilRepositorio : IConciliacaoContabilRepositorio, ITransientDependency
{
    private readonly IRepository<ConciliacaoContabil> _repository;
    private readonly IRepository<ConciliacaoContabilLancamento> _lancamentos;
    private readonly IRepository<ConciliacaoContabilLancamentoDetalhamento> _lancamentosDetalhamentos;
    private readonly IRepository<ConciliacaoContabilApuracao> _apuracoes;
    private readonly IRepository<ConciliacaoContabilApuracaoDetalhamento> _apuracaosDetalhamentos;
    private readonly ILogger<ConciliacaoContabilRepositorio> _logger;
    private readonly IRepository<ConciliacaoContabilEmpresa> _empresas;
    private readonly IRepository<ConciliacaoContabilEtapa> _etapas;

    public ConciliacaoContabilRepositorio(IRepository<ConciliacaoContabil> repository,
        IRepository<ConciliacaoContabilLancamento> lancamentos,
        IRepository<ConciliacaoContabilApuracao> apuracoes, ILogger<ConciliacaoContabilRepositorio> logger,
        IRepository<ConciliacaoContabilEmpresa> empresas, IRepository<ConciliacaoContabilEtapa> etapas, IRepository<ConciliacaoContabilLancamentoDetalhamento> lancamentosDetalhamentos, IRepository<ConciliacaoContabilApuracaoDetalhamento> apuracaosDetalhamentos)
    {
        _repository = repository;
        _lancamentos = lancamentos;
        _apuracoes = apuracoes;
        _logger = logger;
        _empresas = empresas;
        _etapas = etapas;
        _lancamentosDetalhamentos = lancamentosDetalhamentos;
        _apuracaosDetalhamentos = apuracaosDetalhamentos;
    }

    public async Task<ConciliacaoContabil> BuscarConciliacaoContabil(Guid id)
    {
        _logger.LogInformation("Buscando conciliação {id}", id);

        var conciliacao = await _repository.Where(contabil => contabil.Id == id)
            .Include(contabil => contabil.Empresas)
            .Include(contabil => contabil.TipoConciliacaoContabil)
            .Include(contabil => contabil.TipoConciliacaoContabil.ConciliacaoContabilContas)
            .FirstAsync();

        _logger.LogInformation("Buscou conciliação {id} do tipo {conciliacaoContabil}", id,
            conciliacao.TipoConciliacaoContabil.TipoApuracao.ToString());

        return conciliacao;
    }

    public async Task<BuscarConciliacaoContabilOutput> BuscarConciliacaoContabilPorLegacyId(int legacyId)
    {
        _logger.LogInformation("Buscando conciliação {id}", legacyId);

        var conciliacao = await _repository.Where(contabil => contabil.LegacyId == legacyId)
            .Select(c => new BuscarConciliacaoContabilOutput(c))
            .FirstAsync();

        _logger.LogInformation("Buscou conciliação {id}", legacyId);

        return conciliacao;
    }

    public async Task<ListResultDto<BuscarConciliacaoContabilOutput>> BuscarTodasConciliacoesContabeis(
        BuscarTodasConciliacoesContabeisInput input)
    {
        var normalizedFilter = input.Filter is null ? "" : input.Filter.Trim().ToLower();
        var query = _repository
            .WhereIf(!string.IsNullOrEmpty(normalizedFilter), p => p.Descricao.ToLower().Contains(normalizedFilter))
            .Include(c => c.TipoConciliacaoContabil)
            .Select(contabil => new
                {
                    contabil.LegacyId,
                    contabil.DataInicial,
                    contabil.DataFinal,
                    contabil.Descricao,
                    contabil.Erro,
                    contabil.Status,
                    contabil.TipoConciliacaoContabil
                }
            ).ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);
            
        if (string.IsNullOrEmpty(input.Sorting))
        {
            query = query.OrderByDescending(c => c.DataInicial).ThenBy(c => c.Descricao);
        }

        var queryPaged = query.PageBy(input.SkipCount, input.MaxResultCount);
        var conciliacoes = await queryPaged.AsNoTracking().ToListAsync();
        var conciliacoesOutput = conciliacoes
            .Select(c => new BuscarConciliacaoContabilOutput
            {
                LegacyId = c.LegacyId,
                Descricao = c.Descricao,
                DataInicial = c.DataInicial,
                DataFinal = c.DataFinal,
                Status = c.Status,
                TipoApuracaoConciliacaoContabil = c.TipoConciliacaoContabil.TipoApuracao,
                Erro = c.Erro is null ? null : Encoding.UTF8.GetString(c.Erro)
            }).ToList();
        return new ListResultDto<BuscarConciliacaoContabilOutput>(conciliacoesOutput);
    }

    public async Task<ConciliacaoContabil> BuscarConciliacaoContabilLancamentosApuracoes(Guid id)
    {
        _logger.LogInformation("Buscando conciliação {id} com os lançamentos e apurações", id);

        var conciliacao = await _repository.Where(contabil => contabil.Id == id)
            .Include(contabil => contabil.TipoConciliacaoContabil)
            .ThenInclude(tipo => tipo.ConciliacaoContabilContas)
            .Include(contabil => contabil.Etapas)
            .FirstAsync();

        //Não utilizei o include do EFC pois ele acaba montando uma query que multiplica todos os valores para cada um dos include
        await _lancamentos.Where(l => l.IdConciliacaoContabil == conciliacao.LegacyId)
            .OrderBy(o => o.Chave)
            .ToListAsync();
        await _apuracoes.Where(l => l.IdConciliacaoContabil == conciliacao.LegacyId)
            .OrderBy(o => o.Chave)
            .ToListAsync();

        _logger.LogInformation("Buscou conciliação {id} do tipo {conciliacaoContabil} com os lançamentos e apurações",
            id, conciliacao.TipoConciliacaoContabil.TipoApuracao.ToString());

        return conciliacao;
    }

    public async Task Create(ConciliacaoContabil conciliacaoContabil)
    {
        await _repository.InsertAsync(conciliacaoContabil);
    }

    public async Task Update(AtualizarConciliacaoContabilInput input, TipoApuracaoConciliacaoContabil tipo)
    {
        var conciliacaoContabilEntity = await _repository
            .Where(c => c.Id == input.Id && c.TipoConciliacaoContabil.TipoApuracao == tipo)
            .FirstAsync();

        conciliacaoContabilEntity.Conciliado = input.Conciliado;

        await _repository.UpdateAsync(conciliacaoContabilEntity);
    }

    public async Task Delete(int legacyId)
    {
        var conciliacaoContabilEntity = await _repository.FirstOrDefaultAsync(l => l.LegacyId == legacyId);

        await _empresas.Where(e => e.IdConciliacaoContabil == legacyId).DeleteAsync();
        await _etapas.Where(e => e.IdConciliacaoContabil == legacyId).DeleteAsync();

        var apuracaosIds = await _apuracoes
            .Where(a => a.IdConciliacaoContabil == legacyId)
            .Select(a => a.LegacyId)
            .ToListAsync();

        await _apuracaosDetalhamentos.Where(a => apuracaosIds.Contains(a.IdConciliacaoContabilApuracao)).DeleteAsync();
        await _apuracoes.Where(a => apuracaosIds.Contains(a.LegacyId)).DeleteAsync();

        var lancamentosIds = await _lancamentos
            .Where(l => l.IdConciliacaoContabil == legacyId)
            .Select(l => l.LegacyId)
            .ToListAsync();

        await _lancamentosDetalhamentos.Where(l => lancamentosIds.Contains(l.IdConciliacaoContabilLancamento)).DeleteAsync();
        await _lancamentos.Where(l => lancamentosIds.Contains(l.LegacyId)).DeleteAsync();

        await _repository.DeleteAsync(conciliacaoContabilEntity);
    }
}