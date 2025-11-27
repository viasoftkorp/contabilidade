import { Component, signal, viewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { VsSubscriptionManager } from '@viasoft/common';
import { VsTreeViewComponent, VsTreeViewConfig, VsTreeViewNode } from '@viasoft/components';
import { map, Subject, tap, } from 'rxjs';
import { LegacyAdministrationService } from 'src/app/services/legacy-administration/legacy-administration.service';
import { CompanyMatriz } from 'src/app/services/legacy-administration/models/company-matriz.models';
import { Company } from 'src/app/services/legacy-administration/models/company.model';
import { ConciliacaoContabilService } from 'src/app/services/conciliacao-contabil/conciliacao-contabil.service';
import { TipoApuracaoConciliacaoContabil } from 'src/app/services/tipo-conciliacao/models/tipo-apuracao-conciliacao-contabil.enum';
import { TipoConciliacaoContabil } from 'src/app/services/tipo-conciliacao/models/tipo-conciliacao-contabil.model';
import { TipoConciliacaoService } from 'src/app/services/tipo-conciliacao/tipo-conciliacao.service';
import { endDateAfterStartDateValidator } from '../../utils/end-date-after-start-date-validator.util';
import { atLeastOneTrue } from '../../utils/at-least-one-true-validator.util';
import { MatDialogRef } from '@angular/material/dialog';
import { atLeastOneItem } from '../../utils/min-length-one-validator.util';

@Component({
  selector: 'app-add-periodo',
  templateUrl: './add-periodo.component.html',
  styleUrl: './add-periodo.component.scss'
})
export class AddPeriodoComponent {
  public addPeriodoForm: FormGroup;
  public companies: any[] = [];
  public tipoConciliacoes: TipoConciliacaoContabil[] = [];
  public tiposConciliacoesIncomplete = new Subject<boolean>();
  private subs = new VsSubscriptionManager();
  public treeCompaniesConfig: VsTreeViewConfig;
  private companiesOptionsLegacyId = [];
  public treeViewEl = viewChild(VsTreeViewComponent);;
  private skipFormUpdate = false;
  public isSaving = false;

  public treeConfig = signal<VsTreeViewConfig>(new VsTreeViewConfig());

  get tipoApuracoes() {
    return this.addPeriodoForm.get('tipoApuracoes') as FormArray;
  }

  get empresas() {
    return this.addPeriodoForm.get('empresas') as FormArray;
  }

  public get canSave(): boolean {
    return !this.addPeriodoForm.invalid;
  }

  constructor(
    private fb: FormBuilder,
    private legacyAdministrationService: LegacyAdministrationService,
    private tipoConciliacaoService: TipoConciliacaoService,
    private conciliacaoContabilService: ConciliacaoContabilService,
    private dialogRef: MatDialogRef<AddPeriodoComponent>,
  ) { }

  ngOnInit(): void {
    this.recallSetTipoConciliacoes();
    this.setTipoConciliacoes();
    this.initForm();

    this.initTreeConfigWithSignal();
  }

  private initForm(): void {
    this.addPeriodoForm = this.fb.group({
      dataInicial: ['', [Validators.required]],
      dataFinal: ['', [Validators.required]],
      descricao: [''],
      empresas: this.fb.array([], [atLeastOneItem()]),
      tipoApuracoes: this.fb.array([], [atLeastOneTrue()])
    }, { validator: endDateAfterStartDateValidator('dataInicial', 'dataFinal') });
  }

  initTreeConfigWithSignal() {
    this.treeConfig.update((config) => {
      return {
        ...config,
        key: 'companyName',
        selectionMode: "checkbox",
        onLoad: () => {
          this.companiesOptionsLegacyId = [];
          return this.legacyAdministrationService.getCompanies().pipe(
            map(companies => this.convertToVsTreeViewNode(companies)),
            tap(() => this.skipFormUpdate = false),
          );
        },
        onSelect: (selection: VsTreeViewNode[]) => {
          this.empresas.clear();

          selection.forEach(({ data }: { data: any; }) => {
            this.empresas.push(new FormControl(data.legacyCompanyId));
          });
        }
      };
    });
  }

  convertToVsTreeViewNode(companies: CompanyMatriz[] | Company[]): VsTreeViewNode[] {
    return companies.map(
      (company) => {
        this.companiesOptionsLegacyId.push(company.legacyCompanyId);
        if (!this.skipFormUpdate) {
          this.empresas.push(new FormControl(company.legacyCompanyId));
        }

        return <VsTreeViewNode>({
          data: company,
          children: company?.filiais?.length ? this.convertToVsTreeViewNode(company.filiais) : undefined,
          expanded: true,
          selected: this.empresas.value.includes(company.legacyCompanyId),
        });
      },
    );
  }

  private setTipoConciliacoes(): void {
    const maxCount = 100;
    this.tipoConciliacaoService.getAll({ skipCount: this.tipoConciliacoes.length, maxResultCount: maxCount }).then(result => {
      this.tipoConciliacoes = this.tipoConciliacoes.concat(result.items);
      this.tiposConciliacoesIncomplete.next(result.items.length === maxCount);
    });
  }

  private recallSetTipoConciliacoes() {
    this.subs.add(
      'get-all-tipo-conciliacoes',
      this.tiposConciliacoesIncomplete.subscribe((isIcomplete) => {
        if (isIcomplete) {
          this.setTipoConciliacoes();
        } else {
          this.tipoApuracoes.clear();

          this.tipoConciliacoes.forEach((tipo) => {
            this.tipoApuracoes.push(this.fb.control(false));
          });
        }
      })
    );
  }

  public toggleAllTipoApuracoes() {
    const allSelected = this.tipoApuracoes.controls.every(control => control.value);
    this.tipoApuracoes.controls.forEach(control => {
      control.setValue(!allSelected);
    });
  }

  public toggleAllEmpresas() {
    const allSelected = this.empresas.controls.length === this.companiesOptionsLegacyId.length;
    this.empresas.clear();
    if (!allSelected) {
      this.companiesOptionsLegacyId.forEach((legacyId) => {
        this.empresas.push(new FormControl(legacyId));
      });
    }
    this.skipFormUpdate = true;
    this.treeViewEl().getItems();
  }

  public savePeriodo() {
    if (this.canSave) {
      this.isSaving = true;
      this.conciliacaoContabilService.criarConciliacaoContabil({
        dataInicial: this.formatDate(this.addPeriodoForm.get('dataInicial').value),
        dataFinal: this.formatDate(this.addPeriodoForm.get('dataFinal').value),
        descricao: this.addPeriodoForm.get('descricao').value,
        empresas: this.empresas.value,
        tipoApuracoes: this.getSelectedTiposApuracao()
      }).then((res: { idsConciliacao: number[]; }) => {
        this.dialogRef.close(res.idsConciliacao);
      }).finally(() => {
        this.isSaving = false;
      });
    };
  };
  private formatDate(date: string): string {
    const dateObj = new Date(date);
    return dateObj.getFullYear().toString() +
      (dateObj.getMonth() + 1).toString().padStart(2, '0') +
      dateObj.getDate().toString().padStart(2, '0');
  }

  getSelectedTiposApuracao(): TipoApuracaoConciliacaoContabil[] {
    return this.tipoApuracoes.controls
      .map((control, index) => control.value ? this.tipoConciliacoes[index].tipoApuracao : null)
      .filter(id => id !== null);
  }
}
