export interface ConciliacaoContabilLancamento {
    legacyId: number;
    legacyCompanyId: number;
    companyName: string;
    data: Date;
    numeroLancamento: number;
    historico: string;
    valor: number;
    codigoConta: number;
    nomeConta: string;
    conciliado: boolean;
    chave: string;
    idConciliacaoContabil: number;
    codigoFornecedorCliente: string;
    razaoSocialFornecedorCliente: string;
    idTipoLancamento?: number;
    codigoTipoLancamento: string;
    descricaoTipoLancamento: string;
}