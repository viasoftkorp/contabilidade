import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { API_GATEWAY, VS_API_PREFIX, ensureTrailingSlash } from '@viasoft/http';
import { firstValueFrom } from 'rxjs';
import { TipoConciliacaoContabilPaged } from './models/tipo-conciliacao-contabil-paged.model';
import { AdicionarContaInput } from './models/adicionar-conta-input.model';
import { VsGridGetInput } from '@viasoft/components';
import { PagelessResultDto } from '../models/pageless-result-dto.model';
import { TipoConciliacaoContabilConta } from './models/tipo-conciliacao-contabil-conta.model';
import { AdicionarContaOutputEnum } from "./models/adicionar-conta-output.enum";

@Injectable({
  providedIn: 'root'
})
export class TipoConciliacaoService {

  constructor(
    private http: HttpClient,
    @Inject(API_GATEWAY) private gateway: string,
    @Inject(VS_API_PREFIX) private prefix: string
  ) { }

  private get baseUrl(): string {
    return `${ensureTrailingSlash(this.gateway)}${ensureTrailingSlash(this.prefix)}tipo-conciliacao`;
  }

  public getAll(input: Partial<VsGridGetInput>): Promise<TipoConciliacaoContabilPaged> {
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

    return firstValueFrom(this.http.get<TipoConciliacaoContabilPaged>(
      `${ensureTrailingSlash(this.baseUrl)}`,
      { params: queryParameters }
    ));
  }

  public buscarTodasContasPorConciliacao(conciliacaoId: number, input: VsGridGetInput): Promise<PagelessResultDto<TipoConciliacaoContabilConta>> {
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

    return firstValueFrom(this.http.get<PagelessResultDto<TipoConciliacaoContabilConta>>(
      `${ensureTrailingSlash(this.baseUrl)}${conciliacaoId}/contas`,
      { params: queryParameters }
    ));
  }

  public AdicionarConta(conciliacaoId: number, conta: AdicionarContaInput): Promise<{status: AdicionarContaOutputEnum}> {
    if (conciliacaoId === null || conciliacaoId === undefined) {
      throw new Error('The parameter \'conciliacaoId\' must be defined.');
    }
    if (conta.codigoConta === null || conta.codigoConta === undefined) {
      throw new Error('The parameter \'codigoConta\' must be defined.');
    }

    return firstValueFrom(this.http.post<{status: AdicionarContaOutputEnum}>(
      `${ensureTrailingSlash(this.baseUrl)}${conciliacaoId}`,
      conta
    ));
  }

  public DeletarConta(conciliacaoId: number, contaId: number, shouldRemoveLinkedAccounts: boolean | undefined = undefined) {
    if (conciliacaoId === null || conciliacaoId === undefined) {
      throw new Error('The parameter \'conciliacaoId\' must be defined.');
    }
    if (contaId === null || contaId === undefined) {
      throw new Error('The parameter \'contaId\' must be defined.');
    }
    let queryParameters = new HttpParams();
    if (shouldRemoveLinkedAccounts !== undefined) {
      queryParameters = queryParameters.append('shouldRemoveLinkedAccounts', shouldRemoveLinkedAccounts);
    }

    return firstValueFrom(this.http.delete(
      `${ensureTrailingSlash(this.baseUrl)}${conciliacaoId}/contas/${contaId}`, {params: queryParameters}
    ));
  }
}
