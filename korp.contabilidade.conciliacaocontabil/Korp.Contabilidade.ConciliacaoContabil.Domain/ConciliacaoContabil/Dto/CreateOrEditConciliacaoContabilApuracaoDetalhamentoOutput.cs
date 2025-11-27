namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public class CreateOrEditConciliacaoContabilApuracaoDetalhamentoOutput
{
    public CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus Status { get; set; }
}

public enum CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus
{
    Ok,
    CodigoFornecedorClienteInvalido,
    EmpresaInvalida,
    PlanoContaInvalido,
    TipoLancamentoInvalido,
    IdApuracaoInvalido,
    DetalhamentoNaoEncontrado,
}