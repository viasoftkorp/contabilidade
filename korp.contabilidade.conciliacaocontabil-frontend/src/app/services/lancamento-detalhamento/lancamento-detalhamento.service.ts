import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VsGridGetInput } from '@viasoft/components';
import { API_GATEWAY, ensureTrailingSlash, VS_API_PREFIX } from '@viasoft/http';
import { firstValueFrom } from 'rxjs';
import { PagelessResultDto } from '../models/pageless-result-dto.model';
import { ConciliacaoContabilLancamentoDetalhamento } from './models/conciliacao-contabil-lancamento-detalhamento.model';
import { ICreateOrEditConciliacaoContabilLancamentoDetalhamentoOutput } from "./models/create-or-update-lancamento-detalhamento.output";

@Injectable({
  providedIn: 'root'
})
export class LancamentoDetalhamentoService {

  constructor(
    private http: HttpClient,
    @Inject(API_GATEWAY) private gateway: string,
    @Inject(VS_API_PREFIX) private prefix: string
  ) { }

  private get baseUrl(): string {
    return `${ensureTrailingSlash(this.gateway)}${ensureTrailingSlash(this.prefix)}lancamento-detalhamento`;
  }

  public getAll(lancamentoId: number, input: VsGridGetInput): Promise<PagelessResultDto<ConciliacaoContabilLancamentoDetalhamento>> {
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

    return firstValueFrom(this.http.get<PagelessResultDto<ConciliacaoContabilLancamentoDetalhamento>>(
      `${ensureTrailingSlash(this.baseUrl)}${lancamentoId}`,
      { params: queryParameters }
    ));
  }

  public create(input: ConciliacaoContabilLancamentoDetalhamento): Promise<ICreateOrEditConciliacaoContabilLancamentoDetalhamentoOutput> {
    const url = `${ensureTrailingSlash(this.baseUrl)}`;
    return firstValueFrom(this.http.post<ICreateOrEditConciliacaoContabilLancamentoDetalhamentoOutput>(url, input));
  }

  public update(input: ConciliacaoContabilLancamentoDetalhamento): Promise<ICreateOrEditConciliacaoContabilLancamentoDetalhamentoOutput> {
    const url = `${ensureTrailingSlash(this.baseUrl)}${input.legacyId}`;
    return firstValueFrom(this.http.put<ICreateOrEditConciliacaoContabilLancamentoDetalhamentoOutput>(url, input));
  }
}
