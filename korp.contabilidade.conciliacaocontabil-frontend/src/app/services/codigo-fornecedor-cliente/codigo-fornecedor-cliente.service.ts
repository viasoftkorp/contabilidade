import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { API_GATEWAY, VS_API_PREFIX, ensureTrailingSlash } from '@viasoft/http';
import { firstValueFrom } from 'rxjs';
import { PagelessResultDto } from '../models/pageless-result-dto.model';
import { FornecedorClienteCodigoOutput } from "./models";

@Injectable({
  providedIn: 'root'
})
export class CodigoFornecedorClienteService {

  constructor(
    private http: HttpClient,
    @Inject(API_GATEWAY) private gateway: string,
    @Inject(VS_API_PREFIX) private prefix: string
  ) { }

  private get baseUrl(): string {
    return `${ensureTrailingSlash(this.gateway)}${ensureTrailingSlash(this.prefix)}fornecedor-cliente`;
  }

  public getAllCodigos(filter?: string, skipCount?: number, maxResultCount?: number): Promise<PagelessResultDto<FornecedorClienteCodigoOutput>> {
    let queryParameters = new HttpParams();
    if (filter) {
      queryParameters = queryParameters.append('filter', filter);
    }
    if (maxResultCount) {
      queryParameters = queryParameters.append('maxResultCount', maxResultCount);
    }
    if (skipCount) {
      queryParameters = queryParameters.append('skipCount', skipCount);
    }

    return firstValueFrom(this.http.get<PagelessResultDto<FornecedorClienteCodigoOutput>>(
      `${ensureTrailingSlash(this.baseUrl)}codigos`,
      { params: queryParameters }
    ));
  }
}
