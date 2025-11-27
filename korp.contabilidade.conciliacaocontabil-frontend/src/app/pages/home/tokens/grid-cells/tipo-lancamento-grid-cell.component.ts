import { Component, inject, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { VsAutocompleteGetInput, VsAutocompleteModule, VsAutocompleteOption, VsAutocompleteOutput, VsGridGetInput, VsLabelModule, VsTableCellCustomComponent } from "@viasoft/components";
import { from, map, of, tap } from "rxjs";
import { TipoLancamentoDto, TipoLancamentoService } from "src/app/services/tipo-lancamentos";

export interface ITipoLancamentoGridCellData {
    idTipoLancamento: number;
    descricaoTipoLancamento: string;
}

@Component({
    selector: 'tipo-lancamento-grid-cell',
    templateUrl: './tipo-lancamento-grid-cell.component.html',
    styles: 'vs-autocomplete-select { flex: auto; }',
    standalone: true,
    imports: [FormsModule, VsLabelModule, VsAutocompleteModule]
})
export class TipoLancamentoGridCellComponent extends VsTableCellCustomComponent<ITipoLancamentoGridCellData, string> {
    public options = signal<TipoLancamentoDto[]>([]);

    public tipoLancamentoService = inject(TipoLancamentoService);

    public getTipoLancamentos = ((input: VsAutocompleteGetInput) => {
        const requestInput = new VsGridGetInput({
            filter: input.valueToFilter,
            skipCount: input.skipCount,
            maxResultCount: input.maxDropSize,
        });
        return from(this.tipoLancamentoService.getAll(requestInput)).pipe(
            tap((result) => {
                this.options.set(result?.items ?? []);
            }),
            map((result) => {
                return {
                    items: (result.items ?? []).map((item: TipoLancamentoDto) => ({
                        name: item.descricao,
                        value: item.codigo
                    }))
                } as VsAutocompleteOutput<string>;
            }),
        );
    }).bind(this);

    public getTipoLancamentoName = ((value: string) => of(this.data.descricaoTipoLancamento)).bind(this);

    public onOptionSelected(option: VsAutocompleteOption<string>): void {
        const selectedOptions = this.options().find(o => o.codigo === option.value);
        this.data.idTipoLancamento = selectedOptions.legacyId;
        this.data.descricaoTipoLancamento = selectedOptions.descricao;
    }
}