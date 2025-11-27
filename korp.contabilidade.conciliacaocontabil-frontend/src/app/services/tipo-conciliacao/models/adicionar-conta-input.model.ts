export interface AdicionarContaInput {
    codigoConta: number;
    descricao: string;
    shouldAddLinkedAccounts?: boolean;
}