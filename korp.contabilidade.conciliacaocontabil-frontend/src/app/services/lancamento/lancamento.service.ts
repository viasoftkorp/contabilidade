import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { API_GATEWAY, VS_API_PREFIX, ensureTrailingSlash } from '@viasoft/http';
import { firstValueFrom } from 'rxjs';
import { ConciliacaoContabilLancamento } from './models/conciliacao-contabil-lancamento.model';
import { VsGridGetInput } from '@viasoft/components';
import { PagelessResultDto } from '../models/pageless-result-dto.model';

@Injectable({
  providedIn: 'root'
})
export class LancamentoService {

  constructor(
    private http: HttpClient,
    @Inject(API_GATEWAY) private gateway: string,
    @Inject(VS_API_PREFIX) private prefix: string
  ) { }

  private get baseUrl(): string {
    return `${ensureTrailingSlash(this.gateway)}${ensureTrailingSlash(this.prefix)}lancamento`;
  }

  public getAll(conciliacaoId: number, input: VsGridGetInput): Promise<PagelessResultDto<ConciliacaoContabilLancamento>> {
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

    return firstValueFrom(this.http.get<PagelessResultDto<ConciliacaoContabilLancamento>>(
      `${ensureTrailingSlash(this.baseUrl)}${conciliacaoId}`,
      { params: queryParameters }
    ));
  }
}
