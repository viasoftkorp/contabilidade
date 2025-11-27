import { TipoApuracaoConciliacaoContabil } from "./tipo-apuracao-conciliacao-contabil.enum";

export interface TipoConciliacaoContabil {
    legacyId: number;
    descricao: string;
    tipoApuracao: TipoApuracaoConciliacaoContabil;
}