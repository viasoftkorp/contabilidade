import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { API_GATEWAY, VS_API_PREFIX, PORTAL_CONFIG, IPortalConfig, ensureTrailingSlash, VsJwtProviderService } from '@viasoft/http';
import { VsUserService } from '@viasoft/common';
import { Observable } from 'rxjs';
import { CompanyMatriz } from './models/company-matriz.models';

@Injectable({
  providedIn: 'root'
})
export class LegacyAdministrationService {

  constructor(
    private http: HttpClient,
    @Inject(API_GATEWAY) private gateway: string,
    @Inject(VS_API_PREFIX) private prefix: string,
    @Inject(PORTAL_CONFIG) protected portalConfig: IPortalConfig,
    private userService: VsUserService
  ) { }

  private get baseUrl(): string {
    return `${ensureTrailingSlash(this.gateway)}${ensureTrailingSlash(this.prefix)}administracao`;
  }

  getCompanies(): Observable<CompanyMatriz[]> {
    let params = new HttpParams()
      .set('usuario', this.userService.currentUser.name);
    return this.http.get<CompanyMatriz[]>(`${this.baseUrl}/empresas`, { params });
  }
}
