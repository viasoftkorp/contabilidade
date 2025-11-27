import { TipoConciliacaoContabil } from "./tipo-conciliacao-contabil.model";

export interface TipoConciliacaoContabilPaged {
    totalCount?: number;
    items?: TipoConciliacaoContabil[] | null;
}