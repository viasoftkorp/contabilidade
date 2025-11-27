import { Component, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from "@ngx-translate/core";
import { MessageService } from '@viasoft/common';
import { VsGridOptions, VsGridSimpleColumn, VsGridGetResult, VsGridGetInput, VsSelectOption, VsGridNumberColumn, VsSelectModalComponent, IVsSelectModalData, IVsSelectModalGridOptions, VsMessageDialogComponent, VsGridComponent } from '@viasoft/components';
import { first, from, map, tap } from 'rxjs';
import { PlanoContaDto } from 'src/app/services/plano-contas/models/plano-conta.model';
import { PlanoContasService } from 'src/app/services/plano-contas/plano-contas.service';
import { AdicionarContaOutputEnum } from 'src/app/services/tipo-conciliacao/models/adicionar-conta-output.enum';
import { RemoverContaOutputEnum } from "src/app/services/tipo-conciliacao/models/remover-conta-output.enum";
import { TipoConciliacaoContabilConta } from 'src/app/services/tipo-conciliacao/models/tipo-conciliacao-contabil-conta.model';
import { TipoConciliacaoContabil } from 'src/app/services/tipo-conciliacao/models/tipo-conciliacao-contabil.model';
import { TipoConciliacaoService } from 'src/app/services/tipo-conciliacao/tipo-conciliacao.service';

@Component({
  selector: 'app-config-conciliacoes',
  templateUrl: './config-conciliacoes.component.html',
  styleUrl: './config-conciliacoes.component.scss'
})
export class ConfigConciliacoesComponent {
  gridConciliacoes: VsGridOptions;
  gridConciliacoesContas: VsGridOptions;
  gridSelectContas: IVsSelectModalGridOptions;
  selectedConciliacaoId: number;
  public configContasForm: FormGroup;
  @ViewChild(VsGridComponent) gridConciliacoesEl: VsGridComponent;

  constructor(
    private tipoConciliacaoService: TipoConciliacaoService,
    protected dialog: MatDialog,
    private PlanoContasService: PlanoContasService,
    private messageService: MessageService,
    private translateService: TranslateService
  ) {
    this.initGridConciliacoes();
    this.initGridConciliacoesContas();
    this.initGridSelectContas();
  }

  initGridConciliacoes() {
    this.gridConciliacoes = new VsGridOptions<TipoConciliacaoContabil>();
    this.gridConciliacoes.id = "d0277a46-ca4e-4847-8514-9e0e62aeb699";
    this.gridConciliacoes.columns = [
      new VsGridSimpleColumn({
        field: 'descricao',
        headerName: 'Config.Conciliacoes.Conciliacao',
      }),
    ];
    this.gridConciliacoes.get = (input: VsGridGetInput) => this.getAllConciliacoes(input);
    this.gridConciliacoes.onRowClick = (_rowIndex, data) => this.setSelectedConciliacaoId(data);
  }

  initGridConciliacoesContas() {
    this.gridConciliacoesContas = new VsGridOptions<TipoConciliacaoContabilConta>();
    this.gridConciliacoesContas.id = "9e5cb2fe-d905-484c-9466-4172065315ba";
    this.gridConciliacoesContas.columns = [
      new VsGridNumberColumn({
        field: 'codigoConta',
        headerName: 'Config.Contas.CodigoConta',
        width: 50
      }),
      new VsGridSimpleColumn({
        field: 'descricao',
        headerName: 'Config.Contas.Descricao',
      })
    ];
    this.gridConciliacoesContas.delete = (_rowIndex, data) => this.deletarConta(data);
    this.gridConciliacoesContas.get = (input: VsGridGetInput) => this.getAllConciliacoesContas(input);
  }

  initGridSelectContas() {
    this.gridSelectContas = new VsGridOptions<TipoConciliacaoContabilConta>();
    this.gridSelectContas.columns = [
      new VsGridNumberColumn({
        field: 'codigo',
        headerName: 'Config.Contas.CodigoConta',
        width: 80
      }),
      new VsGridSimpleColumn({
        field: 'descricao',
        headerName: 'Config.Contas.Descricao',
      })
    ];
  }

  getAllConciliacoes(input: VsGridGetInput) {
    return from(this.tipoConciliacaoService.getAll(input))
      .pipe(
        map(res => new VsGridGetResult(res.items, res.totalCount)),
        tap((res) => {
          this.setSelectedConciliacaoId(res.data[0]);
        })
      );
  }

  getAllConciliacoesContas(input: VsGridGetInput) {
    if (!this.selectedConciliacaoId) {
      return from([]);
    }
    return from(this.tipoConciliacaoService.buscarTodasContasPorConciliacao(this.selectedConciliacaoId, input))
      .pipe(map(res => new VsGridGetResult(res.items)));
  }

  setSelectedConciliacaoId(conciliacao?: TipoConciliacaoContabil) {
    this.selectedConciliacaoId = conciliacao?.legacyId;
    this.gridConciliacoesEl.table.selection = conciliacao;

    this.gridConciliacoesContas.refresh();
  }

  public openAddAcountModal(): void {
    this.dialog.open(VsSelectModalComponent, {
      data: {
        title: 'Config.Contas.AddTitle',
        icon: 'plus-large',
        gridOptions: this.gridSelectContas,
        service: this.PlanoContasService as any,
      } as IVsSelectModalData
    }).afterClosed().pipe(first()).subscribe((contaSelecionada: PlanoContaDto) => {
      if (!contaSelecionada) {
        return;
      }
      this.addConta(contaSelecionada);
    });
  }

  private addConta(conta: PlanoContaDto, shouldAddLinkedAccounts: boolean | undefined = undefined): void {
    const { codigo: codigoConta, descricao } = conta;
    this.tipoConciliacaoService.AdicionarConta(this.selectedConciliacaoId, { codigoConta, descricao, shouldAddLinkedAccounts }).then(() => {
      this.gridConciliacoesContas.refresh();
    }).catch((e) => {
      if (e.error.status === AdicionarContaOutputEnum.ConciliacaoNaoEncontrada) {
        return this.messageService.error('Config.Contas.Add.ConciliacaoNaoEncontrada');
      }
      if (e.error.status === AdicionarContaOutputEnum.ContaJaAdicionada) {
        return this.messageService.error('Config.Contas.Add.ContaJaAdicionada');
      }
      if (e.error.status === AdicionarContaOutputEnum.ContaVirtualSemAcaoDefinida) {
        const confirmMessage = this.translateService.instant('Config.Contas.Add.ContaVirtualSemAcaoDefinida', {
          accountId: descricao ? `"${descricao}"` : conta,
        });
        return this.messageService.confirm(confirmMessage).pipe(first()).subscribe((shouldAddLinkedAccounts) => {
          this.addConta(conta, shouldAddLinkedAccounts);
        });
      }
      this.messageService.error('Config.Contas.Add.UnknownError');
    });
  }

  private deletarConta(conta: TipoConciliacaoContabilConta, shouldAddLinkedAccounts: boolean | undefined = undefined): void {
    this.tipoConciliacaoService.DeletarConta(this.selectedConciliacaoId, conta.legacyId, shouldAddLinkedAccounts).then(() => {
      this.gridConciliacoesContas.refresh();
    }).catch((e) => {
      if (e.error.status === RemoverContaOutputEnum.ContaVirtualSemAcaoDefinida) {
        const confirmMessage = this.translateService.instant('Config.Contas.Remove.ContaVirtualSemAcaoDefinida', {
          accountId: conta.descricao ? `"${conta.descricao}"` : conta.codigoConta,
        });
        return this.messageService.confirm(confirmMessage).pipe(first()).subscribe((shouldAddLinkedAccounts) => {
          this.deletarConta(conta, shouldAddLinkedAccounts);
        });
      }
      this.messageService.error('Config.Contas.Remove.UnknownError');
    });
  }
}
