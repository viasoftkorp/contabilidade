import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VsGridGetInput } from '@viasoft/components';
import { API_GATEWAY, ensureTrailingSlash, VS_API_PREFIX } from '@viasoft/http';
import { firstValueFrom } from 'rxjs';
import { IGetAllEmpresasDto } from "./models";

@Injectable({
  providedIn: 'root'
})
export class EmpresaService {

  constructor(
    private http: HttpClient,
    @Inject(API_GATEWAY) private gateway: string,
    @Inject(VS_API_PREFIX) private prefix: string
  ) { }

  private get baseUrl(): string {
    return `${ensureTrailingSlash(this.gateway)}${ensureTrailingSlash(this.prefix)}empresas`;
  }

  public getAll(input: VsGridGetInput): Promise<IGetAllEmpresasDto> {
    let queryParameters = new HttpParams();
    if (input.filter) {
      queryParameters = queryParameters.append('filter', input.filter);
    }
    if (input.maxResultCount) {
      queryParameters = queryParameters.append('maxResultCount', input.maxResultCount);
    }
    if (input.skipCount) {
      queryParameters = queryParameters.append('skipCount', input.skipCount);
    }
    if (input.advancedFilter) {
      queryParameters = queryParameters.append('advancedFilter', input.advancedFilter);
    }
    if (input.sorting) {
      queryParameters = queryParameters.append('sorting', input.sorting);
    }

    return firstValueFrom(this.http.get<IGetAllEmpresasDto>(this.baseUrl));
  }
}
