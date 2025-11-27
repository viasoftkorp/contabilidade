export enum CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus {
    Ok,
    TipoApuracaoInvalido,
    EmpresaInvalida,
    IdLancamentoInvalido,
    DetalhamentoNaoEncontrado
}

export interface ICreateOrEditConciliacaoContabilLancamentoDetalhamentoOutput {
    status: CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus;
}
