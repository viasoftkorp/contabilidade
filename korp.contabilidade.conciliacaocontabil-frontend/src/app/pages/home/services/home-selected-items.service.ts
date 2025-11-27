import { effect, Injectable, OnDestroy, signal } from "@angular/core";
import { VsSubscriptionManager } from "@viasoft/common";
import { VsGridComponent } from "@viasoft/components";
import { ConciliacaoContabilApuracaoDetalhamento } from "src/app/services/apuracao-detalhamento/models/conciliacao-contabil-apuracao-detalhamento.model";
import { BuscarConciliacaoContabilOutput } from "src/app/services/conciliacao-contabil/models/buscar-conciliacao-contabil-output.models";
import { ConciliacaoContabilLancamento } from "src/app/services/lancamento/models/conciliacao-contabil-lancamento.model";

@Injectable()
export class HomeSelectedItemsService implements OnDestroy {
    private subs = new VsSubscriptionManager();
    public selectedConciliacao = signal<BuscarConciliacaoContabilOutput | null>(null);
    public selectedApuracao = signal<ConciliacaoContabilApuracaoDetalhamento | null>(null);
    public selectedLancamento = signal<ConciliacaoContabilLancamento | null>(null);

    public gridPeriodosEl = signal<VsGridComponent>(undefined);
    public gridLancamentosEl = signal<VsGridComponent>(undefined);
    public gridLancamentosDetEl = signal<VsGridComponent>(undefined);
    public gridApuracoesEl = signal<VsGridComponent>(undefined);
    public gridApuracoesDetEl = signal<VsGridComponent>(undefined);

    public isCreatingLancamentoDet = signal<boolean>(false);
    public isCreatingLancamentoDetIndex = signal<number | undefined>(undefined);
    public isCreatingApuracaoDet = signal<boolean>(false);
    public isCreatingApuracaoDetIndex = signal<number | undefined>(undefined);

    constructor() {
        effect(() => {
            const grid = this.gridLancamentosDetEl();
            if (grid) {
                this.subs.add('lancamento-det-edit-grid', grid.editManagerService.isEditModeEnabledSuject.subscribe((isEditing) => {
                    if (!isEditing && this.isCreatingLancamentoDet() && this.isCreatingLancamentoDetIndex() !== undefined && grid.editManagerService.rowToEditIndex !== this.isCreatingLancamentoDetIndex()) {
                        this.isCreatingLancamentoDet.set(false);
                        grid.options.refresh();
                    }
                }))
            }
        }, { allowSignalWrites: true });
        effect(() => {
            const grid = this.gridApuracoesDetEl();
            if (grid) {
                this.subs.add('apuracao-det-edit-grid', grid.editManagerService.isEditModeEnabledSuject.subscribe((isEditing) => {
                    if (!isEditing && this.isCreatingApuracaoDet() && this.isCreatingApuracaoDetIndex() !== undefined && grid.editManagerService.rowToEditIndex !== this.isCreatingApuracaoDetIndex()) {
                        this.isCreatingApuracaoDet.set(false);
                        grid.options.refresh();
                    }
                }))
            }
        }, { allowSignalWrites: true });
    }

    ngOnDestroy(): void {
        this.subs.clear();
    }
}