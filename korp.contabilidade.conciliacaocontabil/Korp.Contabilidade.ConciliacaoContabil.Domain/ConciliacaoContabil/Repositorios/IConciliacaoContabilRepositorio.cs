using System;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;

public interface IConciliacaoContabilRepositorio
{
    Task<ConciliacaoContabil> BuscarConciliacaoContabil(Guid id);
    Task<BuscarConciliacaoContabilOutput> BuscarConciliacaoContabilPorLegacyId(int legacyId);
    Task<ListResultDto<BuscarConciliacaoContabilOutput>> BuscarTodasConciliacoesContabeis(BuscarTodasConciliacoesContabeisInput input);
    Task<ConciliacaoContabil> BuscarConciliacaoContabilLancamentosApuracoes(Guid id);
    Task Create(ConciliacaoContabil conciliacaoContabil);
    Task Update(AtualizarConciliacaoContabilInput input, TipoApuracaoConciliacaoContabil tipo);
    Task Delete(int legacyId);
}