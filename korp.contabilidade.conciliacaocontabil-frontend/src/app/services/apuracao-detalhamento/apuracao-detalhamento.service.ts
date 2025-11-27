import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VsGridGetInput } from '@viasoft/components';
import { API_GATEWAY, VS_API_PREFIX, ensureTrailingSlash } from '@viasoft/http';
import { firstValueFrom } from 'rxjs';
import { PagelessResultDto } from '../models/pageless-result-dto.model';
import { ConciliacaoContabilApuracaoDetalhamento } from './models/conciliacao-contabil-apuracao-detalhamento.model';
import { ICreateOrEditConciliacaoContabilApuracaoDetalhamentoOutput } from "./models/create-or-update-apuracao-detalhamento.output";

@Injectable({
  providedIn: 'root'
})
export class ApuracaoDetalhamentoService {

  constructor(
    private http: HttpClient,
    @Inject(API_GATEWAY) private gateway: string,
    @Inject(VS_API_PREFIX) private prefix: string
  ) { }

  private get baseUrl(): string {
    return `${ensureTrailingSlash(this.gateway)}${ensureTrailingSlash(this.prefix)}apuracao-detalhamento`;
  }

  public getAll(apuracaoId: number, input: VsGridGetInput): Promise<PagelessResultDto<ConciliacaoContabilApuracaoDetalhamento>> {
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

    return firstValueFrom(this.http.get<PagelessResultDto<ConciliacaoContabilApuracaoDetalhamento>>(
      `${ensureTrailingSlash(this.baseUrl)}${apuracaoId}`,
      { params: queryParameters }
    ));
  }

  public create(input: ConciliacaoContabilApuracaoDetalhamento): Promise<ICreateOrEditConciliacaoContabilApuracaoDetalhamentoOutput> {
    const url = `${ensureTrailingSlash(this.baseUrl)}`;
    return firstValueFrom(this.http.post<ICreateOrEditConciliacaoContabilApuracaoDetalhamentoOutput>(url, input));
  }

  public update(input: ConciliacaoContabilApuracaoDetalhamento): Promise<ICreateOrEditConciliacaoContabilApuracaoDetalhamentoOutput> {
    const url = `${ensureTrailingSlash(this.baseUrl)}${input.legacyId}`;
    return firstValueFrom(this.http.put<ICreateOrEditConciliacaoContabilApuracaoDetalhamentoOutput>(url, input));
  }
}
