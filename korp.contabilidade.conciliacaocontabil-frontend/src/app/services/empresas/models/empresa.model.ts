export interface IEmpresaDto {
    id: string;
    legacyId: number;
    nome: string;
}

export interface IGetAllEmpresasDto {
    items: IEmpresaDto[];
    totalCount: number;
}
