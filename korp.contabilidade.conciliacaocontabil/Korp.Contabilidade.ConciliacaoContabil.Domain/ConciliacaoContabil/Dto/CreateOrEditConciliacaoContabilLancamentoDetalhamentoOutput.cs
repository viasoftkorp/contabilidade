namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public class CreateOrEditConciliacaoContabilLancamentoDetalhamentoOutput
{
    public CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus Status { get; set; }
}

public enum CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus
{
    Ok,
    TipoApuracaoInvalido,
    EmpresaInvalida,
    IdLancamentoInvalido,
    DetalhamentoNaoEncontrado
}