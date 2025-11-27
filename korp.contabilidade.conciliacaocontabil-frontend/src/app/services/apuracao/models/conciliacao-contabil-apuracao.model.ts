import { TipoValorApuracaoConciliacaoContabil } from "./tipo-valor-apuracao-conciliacao-contabil.enum";

export interface ConciliacaoContabilApuracao {
    legacyId: number;
    legacyCompanyId: number;
    companyName: string;
    data: Date;
    documento: string;
    parcela: string;
    codigoFornecedorCliente: string;
    razaoSocialFornecedorCliente: string;
    valor: number;
    conciliado: boolean;
    chave: string;
    idConciliacaoContabil: number;
    descricaoTipoValorApuracao: string;
    tipoValorApuracao: TipoValorApuracaoConciliacaoContabil;
}