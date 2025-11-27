import { Component, inject, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { VsAutocompleteGetInput, VsAutocompleteModule, VsAutocompleteOption, VsAutocompleteOutput, VsGridGetInput, VsLabelModule, VsTableCellCustomComponent } from "@viasoft/components";
import { from, map, of, tap } from "rxjs";
import { EmpresaService, IEmpresaDto } from "src/app/services/empresas";

export interface IEmpresaGridCellData {
    legacyCompanyId: number;
    companyName: string;
}

@Component({
    selector: 'empresa-grid-cell',
    templateUrl: './empresa-grid-cell.component.html',
    styles: 'vs-autocomplete-select { flex: auto; }',
    standalone: true,
    imports: [FormsModule, VsLabelModule, VsAutocompleteModule]
})
export class EmpresaGridCellComponent extends VsTableCellCustomComponent<IEmpresaGridCellData, string> {
    public options = signal<IEmpresaDto[]>([]);

    public empresasService = inject(EmpresaService);

    public getEmpresas = ((input: VsAutocompleteGetInput) => {
        const requestInput = new VsGridGetInput({
            filter: input.valueToFilter,
            skipCount: input.skipCount,
            maxResultCount: input.maxDropSize,
        });
        return from(this.empresasService.getAll(requestInput)).pipe(
            tap((result) => {
                this.options.set(result?.items ?? []);
            }),
            map((result) => {
                return {
                    items: (result.items ?? []).map((item: IEmpresaDto) => ({
                        name: item.nome,
                        value: item.id
                    })),
                    totalCount: result.totalCount
                } as VsAutocompleteOutput<string>;
            }),
        );
    }).bind(this);

    public getEmpresaName = ((value: string) => of(this.data.companyName)).bind(this);

    public onOptionSelected(option: VsAutocompleteOption<string>): void {
        const selectedOptions = this.options().find(o => o.id === option.value);
        this.data.legacyCompanyId = selectedOptions.legacyId;
        this.data.companyName = selectedOptions.nome;
    }
}