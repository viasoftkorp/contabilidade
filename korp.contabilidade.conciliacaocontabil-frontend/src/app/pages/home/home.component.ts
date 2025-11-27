import { Component, effect, inject, OnDestroy, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { first, from, map, Observable, of, tap } from 'rxjs';

import { MatDialog } from '@angular/material/dialog';
import { MatDrawer } from '@angular/material/sidenav';
import { VsCommandRunnerService, VsSubscriptionManager } from '@viasoft/common';
import { VsDialog, VsGridCheckboxColumn, VsGridComponent, VsGridCurrencyColumn, VsGridDateColumn, VsGridGetInput, VsGridGetResult, VsGridNumberColumn, VsGridOptions, VsGridSimpleColumn, VsSelectOption } from '@viasoft/components';
import { ApuracaoDetalhamentoService } from 'src/app/services/apuracao-detalhamento/apuracao-detalhamento.service';
import { ConciliacaoContabilApuracaoDetalhamento } from 'src/app/services/apuracao-detalhamento/models/conciliacao-contabil-apuracao-detalhamento.model';
import { ApuracaoService } from 'src/app/services/apuracao/apuracao.service';
import { ConciliacaoContabilService } from 'src/app/services/conciliacao-contabil/conciliacao-contabil.service';
import { BuscarConciliacaoContabilOutput } from 'src/app/services/conciliacao-contabil/models/buscar-conciliacao-contabil-output.models';
import { ConciliacaoContabilStatus } from 'src/app/services/conciliacao-contabil/models/conciliacao-contabil-status.enum';
import { LancamentoDetalhamentoService } from 'src/app/services/lancamento-detalhamento/lancamento-detalhamento.service';
import { ConciliacaoContabilLancamentoDetalhamento } from 'src/app/services/lancamento-detalhamento/models/conciliacao-contabil-lancamento-detalhamento.model';
import { LancamentoService } from 'src/app/services/lancamento/lancamento.service';
import { formatLocalizedDate } from "../utils";
import { AddPeriodoComponent } from './add-periodo/add-periodo.component';
import { HomeSelectedItemsService } from "./services/home-selected-items.service";
import { StatusErrorComponent } from './status-error/status-error.component';
import { getApuracoesDetalhamentoGridOptions, getLancamentoDetalhamentoGridOptions } from "./tokens";
import { ConciliacaoContabilLancamento } from "src/app/services/lancamento/models/conciliacao-contabil-lancamento.model";

@Component({
  selector: 'app-home',
  templateUrl: 'home.component.html',
  styleUrls: ['home.component.scss'],
  providers: [HomeSelectedItemsService]
})
export class Home implements OnDestroy {
  @ViewChild('periodosDrawer') periodosDrawer: MatDrawer;

  private subs = new VsSubscriptionManager();
  private periodosOpened: boolean;
  
  public formGroup: FormGroup;
  public gridPeriodos: VsGridOptions;
  public gridLancamentos: VsGridOptions<ConciliacaoContabilLancamento>;
  public gridLancamentosDet: VsGridOptions<ConciliacaoContabilLancamentoDetalhamento>;
  public gridApuracoes: VsGridOptions;
  public gridApuracoesDet: VsGridOptions<ConciliacaoContabilApuracaoDetalhamento>;
  public viewConciliacaoOptions: VsSelectOption[] = [
    {
      value: 'todas',
      name: 'Todas',
    },
    {
      value: 'conciliadas',
      name: 'Conciliadas',
    },
    {
      value: 'nao_conciliadas',
      name: 'Não Conciliadas',
    },
  ];
  private skipRefreshSetSelectedConciliacao = false;

  public homeSelectedItemsService = inject(HomeSelectedItemsService);

  @ViewChildren(VsGridComponent) gridsList: QueryList<VsGridComponent>;

  ngAfterViewInit(): void {
    this.homeSelectedItemsService.gridPeriodosEl.set(this.gridsList.find(g => g.options.id === this.gridPeriodos.id));
    this.homeSelectedItemsService.gridLancamentosEl.set(this.gridsList.find(g => g.options.id === this.gridLancamentos.id));
    this.homeSelectedItemsService.gridLancamentosDetEl.set(this.gridsList.find(g => g.options.id === this.gridLancamentosDet.id));
    this.homeSelectedItemsService.gridApuracoesEl.set(this.gridsList.find(g => g.options.id === this.gridApuracoes.id));
    this.homeSelectedItemsService.gridApuracoesDetEl.set(this.gridsList.find(g => g.options.id === this.gridApuracoesDet.id));
  }

  constructor(
    private fb: FormBuilder,
    private conciliacaoContabilService: ConciliacaoContabilService,
    private readonly dialog: VsDialog,
    private matDialog: MatDialog,
    private lancamentosService: LancamentoService,
    private apuracoesService: ApuracaoService,
    private apuracoesDetalhamentoService: ApuracaoDetalhamentoService,
    private lancamentosDetalhamentoService: LancamentoDetalhamentoService
  ) {
    this.configForm();
    this.configGrids();
    this.configGridsEffects();
    this.addCommandConciliacaoFinalizada();
  }

  addCommandConciliacaoFinalizada() {
    VsCommandRunnerService.addCommand('ConciliacaoContabilFinalizada', (conciliacaoLegacyId) => {
      if (conciliacaoLegacyId === this.homeSelectedItemsService.selectedConciliacao().legacyId) {
        this.conciliacaoContabilService.getByLegacyId(conciliacaoLegacyId).then((conciliacao) => {
          this.skipRefreshSetSelectedConciliacao = true;
          this.gridPeriodos.refresh();
          this.setSelectedConciliacao(conciliacao);
        });
      }
      return Promise.resolve();
    });
  }

  ngOnDestroy(): void {
    this.subs.clear();
    VsCommandRunnerService.removeCommand('ConciliacaoContabilFinalizada');
  }

  setSelectedConciliacao(conciliacao: BuscarConciliacaoContabilOutput) {
    this.homeSelectedItemsService.selectedConciliacao.set(conciliacao);
    this.homeSelectedItemsService.gridPeriodosEl().table.selection = conciliacao;
  }

  configForm() {
    this.formGroup = this.fb.group({
      view_conciliacao: ['todas'],
    });
    this.subs.add('conciliacao-changed', this.formGroup.get('view_conciliacao')?.valueChanges.subscribe((value) => {
      this.handleConciliacaoChange();
    }));
  }

  configGridsEffects() {
    effect(() => {
      const value = this.homeSelectedItemsService.selectedConciliacao();
      this.handleConciliacaoChange();
      // this is necessary bcz grid.refresh writes to internal signals
    }, { allowSignalWrites: true });

    effect(() => {
      const value = this.homeSelectedItemsService.selectedApuracao();
      this.gridApuracoesDet.refresh();
    }, { allowSignalWrites: true });

    effect(() => {
      const value = this.homeSelectedItemsService.selectedLancamento();
      this.gridLancamentosDet.refresh();
    }, { allowSignalWrites: true });
  }

  handleConciliacaoChange() {
    this.gridLancamentos.refresh();
    this.gridApuracoes.refresh();
  }

  toggleDrawer() {
    this.periodosDrawer.toggle();
  }

  getDrawerIcon() {
    if (this.periodosDrawer) {
      return this.periodosDrawer.opened ? 'chevron-left' : 'chevron-right';
    }
    return 'chevron-left';
  }

  togglePeriodosDrawer() {
    this.periodosOpened = !this.periodosOpened;
  }

  openAddPeriodo() {
    this.dialog.open(AddPeriodoComponent, {}, { hasBackdrop: true }).afterClosed().pipe(first()).subscribe((idsAdded: number[]) => {
      if (idsAdded?.length) {
        this.skipRefreshSetSelectedConciliacao = true;
        this.gridPeriodos.refresh();
        const lastId = idsAdded[idsAdded.length - 1];
        this.conciliacaoContabilService.getByLegacyId(lastId).then((conciliacao) => {
          this.setSelectedConciliacao(conciliacao);
        });
      }
    });
  }

  isDone() {
    return this.homeSelectedItemsService.selectedConciliacao()?.status === ConciliacaoContabilStatus.Finalizado;
  }
  isInProgress() {
    return this.homeSelectedItemsService.selectedConciliacao()?.status === ConciliacaoContabilStatus.Progresso;
  }
  hasError() {
    return this.homeSelectedItemsService.selectedConciliacao()?.status === ConciliacaoContabilStatus.Erro;
  }

  showStatusError() {
    if (this.hasError()) {
      this.matDialog.open(StatusErrorComponent, { maxWidth: '60vw', data: this.homeSelectedItemsService.selectedConciliacao() });
    }
  }

  addAdvancedFilterConciliadas(input: VsGridGetInput) {
    const isConciliadas = this.formGroup.value.view_conciliacao === 'conciliadas';
    const shouldAddFilter = isConciliadas || this.formGroup.value.view_conciliacao === 'nao_conciliadas';
    this.toggleDisableGridFilterConciliado(shouldAddFilter);
    if (shouldAddFilter) {
      const currentFilter = {
        condition: 'and',
        rules: [],
        ...JSON.parse(input.advancedFilter || '{}')
      };

      const conciliadoFilter = currentFilter.rules.find((r: any) => r.field === 'conciliado');

      if (conciliadoFilter) {
        conciliadoFilter.value = isConciliadas;
      } else {
        currentFilter.rules.push({
          field: 'conciliado',
          operator: 'equal',
          type: 'boolean',
          value: isConciliadas
        });
      }

      input.advancedFilter = JSON.stringify(currentFilter);
    }
  }

  toggleDisableGridFilterConciliado(value: boolean) {
    this.gridLancamentos.columns.find(c => c.field === 'conciliado').filterOptions.disable = value;
    this.gridApuracoes.columns.find(c => c.field === 'conciliado').filterOptions.disable = value;
  }

  configGrids() {
    this.configGridPeriodos();
    this.configGridLancamentos();
    this.gridLancamentosDet = getLancamentoDetalhamentoGridOptions(this.lancamentosDetalhamentoService, this.homeSelectedItemsService);
    this.configGridApuracoes();
    this.gridApuracoesDet = getApuracoesDetalhamentoGridOptions(this.apuracoesDetalhamentoService, this.homeSelectedItemsService);
  }

  //#region periodos
  configGridPeriodos() {
    this.gridPeriodos = new VsGridOptions<BuscarConciliacaoContabilOutput>();
    this.gridPeriodos.id = "43ddd969-badb-4b07-9202-148c3e05c998";
    this.gridPeriodos.enableQuickFilter = false;
    this.gridPeriodos.columns = [
      new VsGridDateColumn({
        field: 'dataInicial',
        headerName: 'Dt. Inicial',
        filterOptions: {
          // TODO: Enable this filters after updating ConciliacaoContabil with Viasoft.Core after this PR is merged
          // PR: https://bitbucket.org/viasoftkorp/viasoft.core/pull-requests/356/overview
          disable: true
        },
        width: 100,
        format: (field) => formatLocalizedDate(field)
      }),
      new VsGridDateColumn({
        field: 'dataFinal',
        headerName: 'Dt. Final',
        filterOptions: {
          // TODO: Enable this filters after updating ConciliacaoContabil with Viasoft.Core after this PR is merged
          // PR: https://bitbucket.org/viasoftkorp/viasoft.core/pull-requests/356/overview
          disable: true
        },
        width: 100,
        format: (field) => formatLocalizedDate(field)
      }),
      new VsGridSimpleColumn({
        field: 'descricao',
        headerName: 'Descrição',
        width: 450
      }),
    ];
    this.gridPeriodos.get = (input: VsGridGetInput) => this.getAllPeriodos(input);
    this.gridPeriodos.sizeColumnsToFit = false;
    this.gridPeriodos.onRowClick = (_rowIndex, data) => {
      this.setSelectedConciliacao(data);

    };
    this.gridPeriodos.delete = (_rowIndex, data) => this.delete(data);
  }

  getAllPeriodos(input: VsGridGetInput) {
    return from(this.conciliacaoContabilService.getAll(input))
      .pipe(
        map(res => new VsGridGetResult(res.items)),
        tap((res) => {
          if (this.skipRefreshSetSelectedConciliacao) {
            this.skipRefreshSetSelectedConciliacao = false;
            return;
          }
          this.setSelectedConciliacao(res.data[0]);
        })
      );
  }

  delete(data: any): void {
    this.conciliacaoContabilService.delete(data.legacyId).then(() => {
      this.gridPeriodos.refresh();
    });
  }
  //#endregion periodos

  //#region lancamentos

  configGridLancamentos() {
    this.gridLancamentos = new VsGridOptions();
    this.gridLancamentos.id = "1c45968a-a787-4e8f-abb7-066e23336496";
    this.gridLancamentos.enableQuickFilter = false;
    this.gridLancamentos.sizeColumnsToFit = false;
    this.gridLancamentos.columns = [
      new VsGridCheckboxColumn({
        field: 'conciliado',
        headerName: 'Conciliado',
        width: 80,
        disabled: true
      }),
      new VsGridSimpleColumn({
        field: 'companyName',
        headerName: 'Empresa'
      }),
      new VsGridDateColumn({
        field: 'data',
        headerName: 'Data',
        width: 80,
        format: (field) => formatLocalizedDate(field)
      }),
      new VsGridSimpleColumn({
        field: 'numeroLancamento',
        headerName: 'Lcto',
        width: 80
      }),
      new VsGridSimpleColumn({
        field: 'historico',
        headerName: 'Histórico',
        width: 350
      }),
      new VsGridCurrencyColumn({
        field: 'valor',
        headerName: 'Valor',
        width: 110
      }),
      new VsGridSimpleColumn({
        field: 'codigoConta',
        headerName: 'Conta',
        width: 80
      }),
      new VsGridSimpleColumn({
        field: 'nomeConta',
        headerName: 'Nome Conta',
        width: 150
      }),
      new VsGridSimpleColumn({
        field: 'codigoFornecedorCliente',
        headerName: 'Cód. Forn/Cli',
        width: 100
      }),
      new VsGridSimpleColumn({
        field: 'codigoTipoLancamento',
        headerName: 'Tipo Lançamento',
        width: 120
      }),
      new VsGridSimpleColumn({
        field: 'descricaoTipoLancamento',
        headerName: 'Descrição Tipo Lançamento',
        width: 180
      })
    ];
    this.gridLancamentos.get = (input) => this.getAllLancamentos(input);
    this.gridLancamentos.enableVirtualScroll = true;
    this.gridLancamentos.onRowClick = (_rowIndex, data) => {
      this.homeSelectedItemsService.selectedLancamento.set(data);
    };
  }

  getAllLancamentos(input: VsGridGetInput) {
    if (!this.homeSelectedItemsService.selectedConciliacao()) {
      return of(new VsGridGetResult([]));
    }
    this.addAdvancedFilterConciliadas(input);
    return from(this.lancamentosService.getAll(this.homeSelectedItemsService.selectedConciliacao().legacyId, input))
      .pipe(
        map(res => new VsGridGetResult(res.items)),
        tap((res) => {
          this.homeSelectedItemsService.selectedLancamento.set(res.data[0]);
          this.homeSelectedItemsService.gridLancamentosEl().table.selection = res.data[0];
        })
      );
  }

  //#endregion lancamentos

  //#region apuracoes
  configGridApuracoes() {
    this.gridApuracoes = new VsGridOptions();
    this.gridApuracoes.id = "3e5de4d2-0e3c-4c8f-b5bc-8ee945f6c25e";
    this.gridApuracoes.enableQuickFilter = false;
    this.gridApuracoes.sizeColumnsToFit = false;
    this.gridApuracoes.columns = [
      new VsGridCheckboxColumn({
        field: 'conciliado',
        headerName: 'Conciliado',
        width: 80,
        disabled: true
      }),
      new VsGridSimpleColumn({
        field: 'companyName',
        headerName: 'Empresa'
      }),
      new VsGridDateColumn({
        field: 'data',
        headerName: 'Data',
        width: 80,
        format: (field) => formatLocalizedDate(field)
      }),
      new VsGridSimpleColumn({
        field: 'documento',
        headerName: 'Documento',
        width: 240
      }),
      new VsGridNumberColumn({
        field: 'parcela',
        headerName: 'Parcela',
        width: 75
      }),
      new VsGridCurrencyColumn({
        field: 'valor',
        headerName: 'Valor',
        width: 110
      }),
      new VsGridSimpleColumn({
        field: 'codigoFornecedorCliente',
        headerName: 'Cód. Forn/Cli',
        width: 100
      }),
      new VsGridSimpleColumn({
        field: 'razaoSocialFornecedorCliente',
        headerName: 'Razão Social',
        width: 300
      }),
      new VsGridSimpleColumn({
        field: 'descricaoTipoValorApuracao',
        headerName: 'Tipo Apuração',
        width: 300
      })
    ];
    this.gridApuracoes.get = (input) => this.getAllApuracoes(input);
    this.gridApuracoes.enableVirtualScroll = true;
    this.gridApuracoes.onRowClick = (_rowIndex, data) => {
      this.homeSelectedItemsService.selectedApuracao.set(data);
    };
  }

  getAllApuracoes(input: VsGridGetInput): Observable<VsGridGetResult> {
    if (!this.homeSelectedItemsService.selectedConciliacao()) {
      return of(new VsGridGetResult([]));
    }
    this.addAdvancedFilterConciliadas(input);
    return from(this.apuracoesService.getAll(this.homeSelectedItemsService.selectedConciliacao().legacyId, input))
      .pipe(
        map(res => new VsGridGetResult(res.items)),
        tap((res) => {
          this.homeSelectedItemsService.selectedApuracao.set(res.data[0]);
          this.homeSelectedItemsService.gridApuracoesEl().table.selection = res.data[0];
        })
      );
  }
  //#endregion apuracoes
}
