import { Component, inject, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { VsAutocompleteGetInput, VsAutocompleteModule, VsAutocompleteOption, VsAutocompleteOutput, VsLabelModule, VsTableCellCustomComponent } from "@viasoft/components";
import { from, map, of, tap } from "rxjs";
import { TipoValorApuracaoDto, TipoValorApuracaoService } from "src/app/services/tipo-valor-apuracao";
import { HomeSelectedItemsService } from "../../services/home-selected-items.service";

export interface ITipoValorApuracaoGridCellData {
    tipoValorApuracao: number;
    descricaoTipoValorApuracao: string;
}

@Component({
    selector: 'tipo-valor-apuracao-grid-cell',
    templateUrl: './tipo-valor-apuracao-grid-cell.component.html',
    styles: 'vs-autocomplete-select { flex: auto; }',
    standalone: true,
    imports: [FormsModule, VsLabelModule, VsAutocompleteModule]
})
export class TipoValorApuracaoGridCellComponent extends VsTableCellCustomComponent<ITipoValorApuracaoGridCellData, string> {
    public options = signal<TipoValorApuracaoDto[]>([]);

    public homeSelectedItemsService = inject(HomeSelectedItemsService);
    public tipoValorApuracaoService = inject(TipoValorApuracaoService);

    public getTipoValorApuracoes = ((input: VsAutocompleteGetInput) => {
        const tipoApuracaoConciliacaoContabil = this.homeSelectedItemsService.selectedConciliacao().tipoApuracaoConciliacaoContabil;
        return from(this.tipoValorApuracaoService.getAll(tipoApuracaoConciliacaoContabil, input.valueToFilter, input.skipCount, input.maxDropSize)).pipe(
            tap((result) => {
                this.options.set(result?.items ?? []);
            }),
            map((result) => {
                return {
                    items: (result.items ?? []).map((item: TipoValorApuracaoDto) => ({
                        name: item.description,
                        value: item.id
                    }))
                } as VsAutocompleteOutput<number>;
            }),
        );
    }).bind(this);

    public getTipoValorApuracaoName = ((value: number) => of(this.data.descricaoTipoValorApuracao)).bind(this);

    public onOptionSelected(option: VsAutocompleteOption<number>): void {
        const selectedOptions = this.options().find(o => o.id === option.value);
        this.data.tipoValorApuracao = selectedOptions.id;
        this.data.descricaoTipoValorApuracao = selectedOptions.description;
    }
}