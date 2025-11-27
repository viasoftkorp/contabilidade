export const CONCILIACAO_CONTABIL_EN = {
    Conciliacoes: {
        ViewConciliacoes: "Reconciliation view",
        Lancamentos: "Entries",
        Apuracoes: "Reconciliations",
        Periodos: "Period",
        LancamentosDetail: "Entries Detail",
        ApuracoesDetail: "Reconciliation Details",
        AddPeriodo: {
            Title: "Add period",
            InitialDate: "Start date",
            FinalDate: "End date",
            Description: "Description",
            FilterConciliacao: "Conciliation filter",
            FilterCompany: "Company filter",
            ToggleFilter: "Check/Uncheck",
            Errors: {
                EndDateBeforeStartDate: "End date cannot be prior to the start date",
                ConciliacaoMinLength: "Select at least one conciliation",
                CompanyMinLength: "Select at least one company",
            },
            Save: "Generate",
        },
        LancamentoDetalhamentoErrorStatus: {
            Unknown: 'Unknown error',
            TipoApuracaoInvalido: 'Invalid Reconciliation Type',
            EmpresaInvalida: 'Invalid company',
            IdLancamentoInvalido: 'Invalid Entry Id',
            DetalhamentoNaoEncontrado: 'Detail not found for editing',
            DocumentoInvalido: 'Invalid Document',
            ParcelaInvalida: 'Invalid Installment',
            ValorInvalido: 'Invalid Value',
            Data: 'Invalid Date',
        },
        ApuracaoDetalhamentoErrorStatus: {
            Unknown: 'Unknown error',
            EmpresaInvalida: 'Invalid company',
            DetalhamentoNaoEncontrado: 'Detail not found for editing',
            CodigoFornecedorClienteInvalido: 'Invalid Supplier/Customer code',
            IdApuracaoInvalido: 'Invalid Reconciliation Id',
            PlanoContaInvalido: 'Invalid Account Plan',
            TipoLancamentoInvalido: 'Invalid Entry Type',
            NumeroLancamentoInvalido: 'Invalid Entry Number',
            HistoricoInvalido: 'Invalid History',
            ValorInvalido: 'Invalid Value',
            Data: 'Invalid Date',
        },
        Status: {
            Progress: "In progress",
            Done: "Finished",
            Error: "Error",
        },
        Actions: {
            Add: 'Add'
        }
    }
};
