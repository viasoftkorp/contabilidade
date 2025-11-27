using System;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApplicationService;

public interface IConciliacaoContabilApplicationService
{
    Task IniciarConciliacaoContabil(Guid idConciliacaoContabil);
    Task ApurarTipo(Guid id);
    Task ApurarLancamentosContabeis(Guid id);
    Task<ConciliacaoContabilOutput> CriarConciliacaoContabil(CriarConciliacaoContabilInput input);
    Task AtualizarConciliacaoContabil(AtualizarConciliacaoContabilInput input);
    Task DeletarConciliacaoContabil(int legacyId);
    Task<BuscarConciliacaoContabilOutput> BuscarConciliacaoContabil(Guid id);
    Task<BuscarConciliacaoContabilOutput> BuscarConciliacaoContabilPorLegacyId(int legacyId);
    Task ConciliarLancamentoApuracao(Guid id);
    Task ConciliarApuracaoLancamento(Guid id);
    Task FalhouInesperadamente(Guid id, string erro);
    Task FinalizarConciliacao(Guid id);
    Task<ListResultDto<BuscarConciliacaoContabilOutput>> BuscarTodasConciliacoesContabeis(BuscarTodasConciliacoesContabeisInput input);
}