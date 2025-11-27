import { CompanyAddressInfo } from "./company-address-info.model";

export interface Company {
    id: string;
    legacyCompanyId: number;
    legacyCompanyIdMatriz?: number;
    cnpj: string;
    stateRegistration: string;
    companyName: string;
    tradingName: string;
    phone: string;
    email: string;
    companyAddressInfo: CompanyAddressInfo;
}