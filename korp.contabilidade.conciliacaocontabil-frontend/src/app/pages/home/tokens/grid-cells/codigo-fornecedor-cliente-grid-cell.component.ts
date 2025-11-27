import { Component, inject, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { VsAutocompleteGetInput, VsAutocompleteModule, VsAutocompleteOption, VsAutocompleteOutput, VsLabelModule, VsTableCellCustomComponent } from "@viasoft/components";
import { from, map, of, tap } from "rxjs";
import { HomeSelectedItemsService } from "../../services/home-selected-items.service";
import { CodigoFornecedorClienteService, FornecedorClienteCodigoOutput } from "src/app/services/codigo-fornecedor-cliente";

export interface ICodigoFornecedorClienteGridCellData {
    codigoFornecedorCliente: number;
    descricaoCodigoFornecedorCliente: string;
}

@Component({
    selector: 'codigo-fornecedor-cliente-grid-cell',
    templateUrl: './codigo-fornecedor-cliente-grid-cell.component.html',
    styles: 'vs-autocomplete-select { flex: auto; }',
    standalone: true,
    imports: [FormsModule, VsLabelModule, VsAutocompleteModule]
})
export class CodigoFornecedorClienteGridCellComponent extends VsTableCellCustomComponent<ICodigoFornecedorClienteGridCellData, string> {
    public options = signal<FornecedorClienteCodigoOutput[]>([]);

    public homeSelectedItemsService = inject(HomeSelectedItemsService);
    public codigoFornecedorClienteServiceService = inject(CodigoFornecedorClienteService);

    public getTipoValorApuracoes = ((input: VsAutocompleteGetInput) => {
        return from(this.codigoFornecedorClienteServiceService.getAllCodigos(input.valueToFilter, input.skipCount, input.maxDropSize)).pipe(
            tap((result) => {
                this.options.set(result?.items ?? []);
            }),
            map((result) => {
                return {
                    items: (result.items ?? []).map((item: FornecedorClienteCodigoOutput) => ({
                        name: `${item.razaoSocial} (${item.codigo})`,
                        value: item.codigo
                    }))
                } as VsAutocompleteOutput<number>;
            }),
        );
    }).bind(this);

    public getTipoValorApuracaoName = ((value: number) => of(this.data.descricaoCodigoFornecedorCliente)).bind(this);

    public onOptionSelected(option: VsAutocompleteOption<number>): void {
        const selectedOptions = this.options().find(o => o.codigo === option.value);
        this.data.codigoFornecedorCliente = selectedOptions.codigo;
        this.data.descricaoCodigoFornecedorCliente = selectedOptions.razaoSocial;
    }
}