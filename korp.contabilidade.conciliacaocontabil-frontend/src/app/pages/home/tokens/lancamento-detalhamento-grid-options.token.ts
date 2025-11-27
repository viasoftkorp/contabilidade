import { IVsTableEditRowErrorResult, IVsTableEditRowResult } from "@viasoft/components";
import { VsGridCurrencyColumn, VsGridDateColumn, VsGridGetInput, VsGridGetResult, VsGridNumberColumn, VsGridOptions, VsGridSimpleColumn } from "@viasoft/components/grid";
import { catchError, from, map, Observable, of } from "rxjs";
import { LancamentoDetalhamentoService } from "src/app/services/lancamento-detalhamento/lancamento-detalhamento.service";
import { ConciliacaoContabilLancamentoDetalhamento } from "src/app/services/lancamento-detalhamento/models/conciliacao-contabil-lancamento-detalhamento.model";
import { formatLocalizedDate, getLocalizedDate } from "../../utils";
import { HomeSelectedItemsService } from "../services/home-selected-items.service";
import { EmpresaGridCellComponent, TipoValorApuracaoGridCellComponent } from "./grid-cells";
import { HttpErrorResponse } from "@angular/common/http";
import { CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus } from "src/app/services/lancamento-detalhamento/models/create-or-update-lancamento-detalhamento.output";

export function getLancamentoDetalhamentoGridOptions(lancamentosDetalhamentoService: LancamentoDetalhamentoService, selectedItemsService: HomeSelectedItemsService): VsGridOptions<ConciliacaoContabilLancamentoDetalhamento> {
    const gridLancamentosDet = new VsGridOptions<ConciliacaoContabilLancamentoDetalhamento>();
    gridLancamentosDet.id = "b149a23c-f826-46c6-a6c3-877ecbf6364e";
    gridLancamentosDet.enableQuickFilter = false;
    gridLancamentosDet.sizeColumnsToFit = false;
    gridLancamentosDet.columns = [
        new VsGridSimpleColumn({
            field: 'companyName',
            headerName: 'Empresa',
            type: EmpresaGridCellComponent
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
            width: 300,
            type: TipoValorApuracaoGridCellComponent
        })
    ];

    gridLancamentosDet.rightActions = [
        {
            icon: 'plus',
            tooltip: 'Conciliacoes.Actions.Add',
            disabled: () => !Boolean(selectedItemsService.selectedLancamento()?.legacyId),
            callback: () => {
                const grid = selectedItemsService.gridLancamentosDetEl();
                if (grid) {
                    selectedItemsService.isCreatingLancamentoDet.set(true);
                    grid.options.refresh();
                }
            }
        }
    ];

    gridLancamentosDet.editRowOptions = {
        isAutoEditable: true,
        isCellEditable(index, fieldName, data) {
            const blockedColumns = ['legacyId', 'companyName', 'codigoFornecedorCliente', 'razaoSocialFornecedorCliente'];
            return !blockedColumns.includes(fieldName);
        },
        onRowEdit: (index, originalData, newData) => {
            const isDataInvalidMessage = validateData(newData);
            if (isDataInvalidMessage) {
                return of({ success: false, errorMessage: isDataInvalidMessage } as IVsTableEditRowResult);
            }
            const isCreatingData = typeof originalData?.legacyId === 'undefined';
            if (isCreatingData) {
                return from(lancamentosDetalhamentoService.create(newData)).pipe(map(result => {
                    const success = result.status === CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus.Ok;
                    if (success) {
                        return { success, shouldAutoRefreshGrid: true, updatedRowData: result } as IVsTableEditRowResult;
                    }
                    const errorMessage = getErrorMessage(result.status);
                    return { success, errorMessage } as IVsTableEditRowErrorResult;
                }), catchError((err: HttpErrorResponse) => {
                    return of({ success: false, errorMessage: getErrorMessage() } as IVsTableEditRowResult);
                }));
            } else {
                newData.legacyId = originalData.legacyId;
                return from(lancamentosDetalhamentoService.update(newData)).pipe(map(result => {
                    const success = result.status === CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus.Ok;
                    if (success) {
                        return { success, shouldAutoRefreshGrid: true, updatedRowData: result } as IVsTableEditRowResult;
                    }
                    const errorMessage = getErrorMessage(result.status);
                    return { success, errorMessage } as IVsTableEditRowErrorResult;
                }), catchError((err: HttpErrorResponse) => {
                    return of({ success: false, errorMessage: getErrorMessage() } as IVsTableEditRowResult);
                }));
            }
        },
    };
    gridLancamentosDet.get = (input) => getAllLancamentosDetalhamento(lancamentosDetalhamentoService, selectedItemsService, input);
    gridLancamentosDet.enableVirtualScroll = true;
    return gridLancamentosDet;
}

function getErrorMessage(status?: CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus) {
    switch (status) {
        case CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus.Ok:
            return '';
        case CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus.EmpresaInvalida:
            return 'Conciliacoes.LancamentoDetalhamentoErrorStatus.EmpresaInvalida';
        case CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus.DetalhamentoNaoEncontrado:
            return 'Conciliacoes.LancamentoDetalhamentoErrorStatus.DetalhamentoNaoEncontrado';
        case CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus.TipoApuracaoInvalido:
            return 'Conciliacoes.LancamentoDetalhamentoErrorStatus.TipoApuracaoInvalido';
        case CreateOrEditConciliacaoContabilLancamentoDetalhamentoStatus.IdLancamentoInvalido:
            return 'Conciliacoes.LancamentoDetalhamentoErrorStatus.IdLancamentoInvalido';
        default:
            return 'Conciliacoes.LancamentoDetalhamentoErrorStatus.Unknown';
    }
}

function validateData(data: ConciliacaoContabilLancamentoDetalhamento) {
    if (!data.documento) { return 'Conciliacoes.LancamentoDetalhamentoErrorStatus.DocumentoInvalido'; }
    if (!data.parcela) { return 'Conciliacoes.LancamentoDetalhamentoErrorStatus.ParcelaInvalida'; }
    if (!data.valor) { return 'Conciliacoes.LancamentoDetalhamentoErrorStatus.ValorInvalido'; }
    if (!data.tipoValorApuracao) { return 'Conciliacoes.LancamentoDetalhamentoErrorStatus.TipoApuracaoInvalido'; }
    if (!data.data) { return 'Conciliacoes.LancamentoDetalhamentoErrorStatus.DataInvalida' }
    return '';
}

export function getAllLancamentosDetalhamento(lancamentosDetalhamentoService: LancamentoDetalhamentoService, selectedItemsService: HomeSelectedItemsService, input: VsGridGetInput): Observable<any> {
    if (!selectedItemsService.selectedLancamento()) {
        return of(new VsGridGetResult([]));
    }
    return from(lancamentosDetalhamentoService.getAll(selectedItemsService.selectedLancamento().legacyId, input))
        .pipe(
            map(res => new VsGridGetResult(res.items)),
            map(result => {
                result.data = (result.data as ConciliacaoContabilLancamentoDetalhamento[]).map(d => {
                    if (typeof d.data === 'string') {
                        d.data = getLocalizedDate(d.data);
                    }
                    return d;
                });
                return result;
            }),
            map(result => {
                if (!selectedItemsService.isCreatingLancamentoDet()) {
                    return result;
                }
                // Add new line at the end so we can add lines too.
                result.data = result.data.concat({
                    idConciliacaoContabilLancamento: selectedItemsService.selectedLancamento()?.legacyId,
                    legacyCompanyId: selectedItemsService.selectedLancamento()?.legacyCompanyId,
                    companyName: selectedItemsService.selectedLancamento()?.companyName,
                    data: getLocalizedDate(selectedItemsService.selectedLancamento()?.data),
                    codigoFornecedorCliente: selectedItemsService.selectedLancamento()?.codigoFornecedorCliente,
                    razaoSocialFornecedorCliente: selectedItemsService.selectedLancamento()?.razaoSocialFornecedorCliente,
                } as ConciliacaoContabilLancamentoDetalhamento);

                const grid = selectedItemsService.gridLancamentosDetEl();
                // TODO: we need 500ms so virtual scroll + edit + startEdit works properly
                setTimeout(() => {
                    const index = result.data.length - 1;
                    selectedItemsService.isCreatingLancamentoDetIndex.set(index);
                    grid.editManagerService.startEdit(index);
                }, 500);
                return result;
            })
        );
};