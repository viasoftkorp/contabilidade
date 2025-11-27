import { TipoValorApuracaoConciliacaoContabil } from "../../apuracao/models/tipo-valor-apuracao-conciliacao-contabil.enum";

export interface ConciliacaoContabilLancamentoDetalhamento {
    legacyId: number;
    legacyCompanyId: number;
    companyName: string;
    data: Date;
    documento: string;
    parcela: string;
    codigoFornecedorCliente: string;
    razaoSocialFornecedorCliente: string;
    valor: number;
    idConciliacaoContabilLancamento: number;
    tipoValorApuracao: TipoValorApuracaoConciliacaoContabil;
    descricaoTipoValorApuracao: string;
}