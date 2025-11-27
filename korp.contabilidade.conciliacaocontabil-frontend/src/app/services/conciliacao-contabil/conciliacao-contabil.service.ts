import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { API_GATEWAY, ensureTrailingSlash, VS_API_PREFIX } from '@viasoft/http';
import { firstValueFrom } from 'rxjs';
import { CriarConciliacaoContabilInput } from './models/criar-conciliacao-contabil-input.model';
import { VsGridGetInput } from '@viasoft/components';
import { PagelessResultDto } from '../models/pageless-result-dto.model';
import { BuscarConciliacaoContabilOutput } from './models/buscar-conciliacao-contabil-output.models';

@Injectable({
  providedIn: 'root'
})
export class ConciliacaoContabilService {

  constructor(
    private http: HttpClient,
    @Inject(API_GATEWAY) private gateway: string,
    @Inject(VS_API_PREFIX) private prefix: string
  ) { }

  private get baseUrl(): string {
    return `${ensureTrailingSlash(this.gateway)}${ensureTrailingSlash(this.prefix)}`;
  }

  public getAll(input: VsGridGetInput): Promise<PagelessResultDto<BuscarConciliacaoContabilOutput>> {
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

    return firstValueFrom(this.http.get<PagelessResultDto<BuscarConciliacaoContabilOutput>>(
      `${ensureTrailingSlash(this.baseUrl)}`,
      { params: queryParameters }
    ));
  }

  public criarConciliacaoContabil(input: CriarConciliacaoContabilInput) {
    return firstValueFrom(this.http.post(
      `${ensureTrailingSlash(this.baseUrl)}`,
      input
    ));
  }

  public delete(id: number) {
    return firstValueFrom(this.http.delete(
      `${ensureTrailingSlash(this.baseUrl)}${id}`
    ));
  }

  public getByLegacyId(legacyId: number) {
    return firstValueFrom(this.http.get<BuscarConciliacaoContabilOutput>(
      `${ensureTrailingSlash(this.baseUrl)}${legacyId}`
    ));
  }
}
