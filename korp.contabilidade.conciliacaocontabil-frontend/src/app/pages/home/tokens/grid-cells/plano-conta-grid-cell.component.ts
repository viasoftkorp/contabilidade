import { Component, inject, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { VsAutocompleteGetInput, VsAutocompleteModule, VsAutocompleteOption, VsAutocompleteOutput, VsGridGetInput, VsLabelModule, VsTableCellCustomComponent, VsTableEditRowManagerService } from "@viasoft/components";
import { from, map, of, tap } from "rxjs";
import { PlanoContaDto, PlanoContasService } from "src/app/services/plano-contas";

export interface IPlanoContaGridCellData {
    codigoConta: number;
    nomeConta: string;
}

@Component({
    selector: 'plano-conta-grid-cell',
    templateUrl: './plano-conta-grid-cell.component.html',
    styles: 'vs-autocomplete-select { flex: auto; }',
    standalone: true,
    imports: [FormsModule, VsLabelModule, VsAutocompleteModule]
})
export class PlanoContaGridCellComponent extends VsTableCellCustomComponent<IPlanoContaGridCellData, string> {
    public options = signal<PlanoContaDto[]>([]);

    public planoContaService = inject(PlanoContasService);
    public vsTableEditRowManagerService = inject(VsTableEditRowManagerService);

    public getPlanoContas = ((input: VsAutocompleteGetInput) => {
        const requestInput = new VsGridGetInput({
            filter: input.valueToFilter,
            skipCount: input.skipCount,
            maxResultCount: input.maxDropSize,
        });
        return from(this.planoContaService.getAll(requestInput)).pipe(
            tap((result) => {
                this.options.set(result?.items ?? []);
            }),
            map((result) => {
                return {
                    items: (result.items ?? []).map((item: PlanoContaDto) => ({
                        name: item.descricao,
                        value: item.codigo
                    }))
                } as VsAutocompleteOutput<number>;
            }),
        );
    }).bind(this);

    public getPlanoContaName = ((value: number) => of(this.data.nomeConta)).bind(this);

    public onOptionSelected(option: VsAutocompleteOption<number>): void {
        const selectedOptions = this.options().find(o => o.codigo === option.value);
        this.data.codigoConta = selectedOptions.codigo;
        this.data.nomeConta = selectedOptions.descricao;
    }
}