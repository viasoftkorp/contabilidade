export interface ConciliacaoContabilApuracaoDetalhamento {
    legacyId: number;
    legacyCompanyId: number;
    companyName: string;
    data: string | Date;
    numeroLancamento: number;
    historico: string;
    valor: number;
    codigoConta: number;
    nomeConta: string;
    codigoFornecedorCliente: string;
    idConciliacaoContabilApuracao: number;
    idTipoLancamento?: number;
    codigoTipoLancamento: string;
    descricaoTipoLancamento: string;
}