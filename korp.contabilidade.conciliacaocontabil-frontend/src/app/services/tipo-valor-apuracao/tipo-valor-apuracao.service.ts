import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { API_GATEWAY, VS_API_PREFIX, ensureTrailingSlash } from '@viasoft/http';
import { Observable } from 'rxjs';
import { PagelessResultDto } from '../models/pageless-result-dto.model';
import { TipoValorApuracaoDto } from "./models/tipo-valor-apuracao.model";

@Injectable({
  providedIn: 'root'
})
export class TipoValorApuracaoService {

  constructor(
    private http: HttpClient,
    @Inject(API_GATEWAY) private gateway: string,
    @Inject(VS_API_PREFIX) private prefix: string
  ) { }

  private get baseUrl(): string {
    return `${ensureTrailingSlash(this.gateway)}${ensureTrailingSlash(this.prefix)}tipo-valor-apuracao`;
  }

  public getAll(tipoApuracaoConciliacaoContabil: number, filter?: string, skipCount?: number, maxResultCount?: number): Observable<{ items: TipoValorApuracaoDto[] }> {
    let queryParameters = new HttpParams();
    queryParameters = queryParameters.append('tipoApuracaoConciliacaoContabil', tipoApuracaoConciliacaoContabil);
    if (filter) {
      queryParameters = queryParameters.append('filter', filter);
    }
    if (skipCount) {
      queryParameters = queryParameters.append('skipCount', skipCount);
    }
    if (maxResultCount) {
      queryParameters = queryParameters.append('maxResultCount', maxResultCount);
    }

    return this.http.get<{ items: TipoValorApuracaoDto[] }>(`${ensureTrailingSlash(this.baseUrl)}`, { params: queryParameters });
  }
}
