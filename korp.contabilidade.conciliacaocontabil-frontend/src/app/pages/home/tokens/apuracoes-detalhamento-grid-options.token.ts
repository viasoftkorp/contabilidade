import { IVsTableEditRowErrorResult, IVsTableEditRowResult } from "@viasoft/components";
import { VsGridCurrencyColumn, VsGridDateColumn, VsGridGetInput, VsGridGetResult, VsGridOptions, VsGridSimpleColumn } from "@viasoft/components/grid";
import { catchError, from, map, Observable, of } from "rxjs";
import { ApuracaoDetalhamentoService } from "src/app/services/apuracao-detalhamento/apuracao-detalhamento.service";
import { ConciliacaoContabilApuracaoDetalhamento } from "src/app/services/apuracao-detalhamento/models/conciliacao-contabil-apuracao-detalhamento.model";
import { formatLocalizedDate, getLocalizedDate } from "../../utils";
import { HomeSelectedItemsService } from "../services/home-selected-items.service";
import { EmpresaGridCellComponent, PlanoContaGridCellComponent, TipoLancamentoGridCellComponent, CodigoFornecedorClienteGridCellComponent } from "./grid-cells";
import { CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus } from "src/app/services/apuracao-detalhamento/models/create-or-update-apuracao-detalhamento.output";
import { HttpErrorResponse } from "@angular/common/http";

export function getApuracoesDetalhamentoGridOptions(apuracoesDetalhamentoService: ApuracaoDetalhamentoService, selectedItemsService: HomeSelectedItemsService): VsGridOptions<ConciliacaoContabilApuracaoDetalhamento> {
    const gridApuracoesDet = new VsGridOptions<ConciliacaoContabilApuracaoDetalhamento>();
    gridApuracoesDet.id = "66ea8d68-c426-4b11-8d59-7c45380eb301";
    gridApuracoesDet.enableQuickFilter = false;
    gridApuracoesDet.sizeColumnsToFit = false;
    gridApuracoesDet.columns = [
        new VsGridSimpleColumn({
            field: 'companyName',
            headerName: 'Empresa',
            type: EmpresaGridCellComponent
        }),
        new VsGridDateColumn({
            field: 'data',
            headerName: 'Data',
            width: 150,
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
            width: 80,
        }),
        new VsGridSimpleColumn({
            field: 'nomeConta',
            headerName: 'Nome Conta',
            width: 150,
            type: PlanoContaGridCellComponent
        }),
        new VsGridSimpleColumn({
            field: 'codigoFornecedorCliente',
            headerName: 'Cód. Forn/Cli',
            width: 100,
            type: CodigoFornecedorClienteGridCellComponent
        }),
        new VsGridSimpleColumn({
            field: 'codigoTipoLancamento',
            headerName: 'Tipo Lançamento',
            width: 120
        }),
        new VsGridSimpleColumn({
            field: 'descricaoTipoLancamento',
            headerName: 'Descrição Tipo Lançamento',
            width: 180,
            type: TipoLancamentoGridCellComponent
        })
    ];

    gridApuracoesDet.rightActions = [
        {
            icon: 'plus',
            tooltip: 'Conciliacoes.Actions.Add',
            disabled: () => !Boolean(selectedItemsService.selectedApuracao()?.legacyId),
            callback: () => {
                const grid = selectedItemsService.gridApuracoesDetEl();
                if (grid) {
                    selectedItemsService.isCreatingApuracaoDet.set(true);
                    grid.options.refresh();
                }
            }
        }
    ];

    gridApuracoesDet.editRowOptions = {
        isAutoEditable: true,
        isCellEditable(index, fieldName, data) {
            const blockedColumns = ['codigoConta', 'codigoTipoLancamento', 'legacyId', 'companyName'];
            return !blockedColumns.includes(fieldName);
        },
        onRowEdit(index, originalData, newData) {
            const isDataInvalidMessage = validateData(newData);
            if (isDataInvalidMessage) {
                return of({ success: false, errorMessage: isDataInvalidMessage } as IVsTableEditRowResult);
            }
            const isCreatingData = typeof originalData?.legacyId === 'undefined';
            if (isCreatingData) {
                return from(apuracoesDetalhamentoService.create(newData)).pipe(map(result => {
                    const success = result.status === CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.Ok;
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
                return from(apuracoesDetalhamentoService.update(newData)).pipe(map(result => {
                    const success = result.status === CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.Ok;
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
    gridApuracoesDet.get = (input) => getAllApuracoesDetalhamento(apuracoesDetalhamentoService, selectedItemsService, input);
    gridApuracoesDet.enableVirtualScroll = true;
    return gridApuracoesDet;
}

function getErrorMessage(status?: CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus) {
    switch (status) {
        case CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.Ok:
            return '';
        case CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.EmpresaInvalida:
            return 'Conciliacoes.ApuracaoDetalhamentoErrorStatus.EmpresaInvalida';
        case CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.DetalhamentoNaoEncontrado:
            return 'Conciliacoes.ApuracaoDetalhamentoErrorStatus.DetalhamentoNaoEncontrado';
        case CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.CodigoFornecedorClienteInvalido:
            return 'Conciliacoes.ApuracaoDetalhamentoErrorStatus.CodigoFornecedorClienteInvalido';
        case CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.IdApuracaoInvalido:
            return 'Conciliacoes.ApuracaoDetalhamentoErrorStatus.IdApuracaoInvalido';
        case CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.PlanoContaInvalido:
            return 'Conciliacoes.ApuracaoDetalhamentoErrorStatus.PlanoContaInvalido';
        case CreateOrEditConciliacaoContabilApuracaoDetalhamentoStatus.TipoLancamentoInvalido:
            return 'Conciliacoes.ApuracaoDetalhamentoErrorStatus.TipoLancamentoInvalido';
        default:
            return 'Conciliacoes.ApuracaoDetalhamentoErrorStatus.Unknown';
    }
}

function validateData(data: ConciliacaoContabilApuracaoDetalhamento) {
    if (!data.numeroLancamento) { return 'Conciliacoes.ApuracaoDetalhamentoErrorStatus.NumeroLancamentoInvalido'; }
    if (!data.historico) { return 'Conciliacoes.ApuracaoDetalhamentoErrorStatus.HistoricoInvalido' }
    if (!data.valor) { return 'Conciliacoes.ApuracaoDetalhamentoErrorStatus.ValorInvalido' }
    if (!data.codigoConta) { return 'Conciliacoes.ApuracaoDetalhamentoErrorStatus.PlanoContaInvalido' }
    if (!data.codigoFornecedorCliente) { return 'Conciliacoes.ApuracaoDetalhamentoErrorStatus.CodigoFornecedorClienteInvalido' }
    if (!data.codigoTipoLancamento) { return 'Conciliacoes.ApuracaoDetalhamentoErrorStatus.TipoLancamentoInvalido' }
    if (!data.data) { return 'Conciliacoes.ApuracaoDetalhamentoErrorStatus.DataInvalida' }
    return '';
}

export function getAllApuracoesDetalhamento(apuracoesDetalhamentoService: ApuracaoDetalhamentoService, selectedItemsService: HomeSelectedItemsService, input: VsGridGetInput): Observable<VsGridGetResult> {
    if (!selectedItemsService.selectedApuracao()) {
        return of(new VsGridGetResult([]));
    }
    return from(apuracoesDetalhamentoService.getAll(selectedItemsService.selectedApuracao().legacyId, input))
        .pipe(
            map(res => new VsGridGetResult(res.items)),
            map(result => {
                result.data = (result.data as ConciliacaoContabilApuracaoDetalhamento[]).map(d => {
                    if (typeof d.data === 'string') {
                        d.data = getLocalizedDate(d.data);
                    }
                    return d;
                });
                return result;
            }),
            map(result => {
                if (!selectedItemsService.isCreatingApuracaoDet()) {
                    return result;
                }
                // Add new line at the end so we can add lines too.
                result.data = result.data.concat({
                    idConciliacaoContabilApuracao: selectedItemsService.selectedApuracao()?.legacyId,
                    legacyCompanyId: selectedItemsService.selectedApuracao()?.legacyCompanyId,
                    companyName: selectedItemsService.selectedApuracao()?.companyName,
                    data: getLocalizedDate(selectedItemsService.selectedApuracao()?.data),
                    codigoFornecedorCliente: selectedItemsService.selectedApuracao()?.codigoFornecedorCliente,
                } as ConciliacaoContabilApuracaoDetalhamento);

                const grid = selectedItemsService.gridApuracoesDetEl();
                // TODO: we need 500ms so virtual scroll + edit + startEdit works properly
                setTimeout(() => {
                    const index = result.data.length - 1;
                    selectedItemsService.isCreatingApuracaoDetIndex.set(index);
                    grid.editManagerService.startEdit(index);
                }, 500);

                return result;
            })
        );
}