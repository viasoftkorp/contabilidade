export enum CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus {
    Ok,
    CodigoFornecedorClienteInvalido,
    EmpresaInvalida,
    PlanoContaInvalido,
    TipoLancamentoInvalido,
    IdApuracaoInvalido,
    DetalhamentoNaoEncontrado
}

export interface ICreateOrEditConciliacaoContabilApuracaoDetalhamentoOutput {
    status: CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus;
}
