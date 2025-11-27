import { ConciliacaoContabilStatus } from "./conciliacao-contabil-status.enum";

export interface BuscarConciliacaoContabilOutput {
    legacyId: number;
    descricao: string;
    dataInicial: Date;
    dataFinal: Date;
    status: ConciliacaoContabilStatus;
    tipoApuracaoConciliacaoContabil: number;
    erro: string;
}