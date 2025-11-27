import { Company } from "./company.model";

export interface CompanyMatriz extends Company {
    filiais: Company[];
}