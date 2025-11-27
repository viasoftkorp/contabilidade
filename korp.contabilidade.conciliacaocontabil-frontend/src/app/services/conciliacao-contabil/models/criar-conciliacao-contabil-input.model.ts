import { TipoApuracaoConciliacaoContabil } from "../../tipo-conciliacao/models/tipo-apuracao-conciliacao-contabil.enum";

export interface CriarConciliacaoContabilInput {
    descricao: string 
    dataInicial: string 
    dataFinal: string 
    empresas: number[]
    tipoApuracoes: TipoApuracaoConciliacaoContabil[]
}