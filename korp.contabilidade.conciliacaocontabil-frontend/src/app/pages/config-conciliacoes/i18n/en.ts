export const CONCILIACAO_CONFIG_EN = {
    Config: {
        Title: "Configure Reconciliations",
        Conciliacoes: {
            Title: "Reconciliations",
            Conciliacao: "Reconciliation"
        },
        Contas: {
            Title: "Accounts",
            CodigoConta: "Account Code",
            Descricao: "Description",
            AddTitle: "Add Account",
            Add: {
                Title: "Select Account",
                ConciliacaoNaoEncontrada: "Could not find the reconciliation to add the account.",
                ContaJaAdicionada: "Account has already been added to the current reconciliation.",
                ContaVirtualSemAcaoDefinida: "The account {{accountId}} refers to a virtual account, do you want to import all accounts linked to it?",
                UnknownError: "Unknown error adding account."
            },
            Remove: {
                ContaVirtualSemAcaoDefinida: "The account {{accountId}} refers to a virtual account, do you want to delete all accounts linked to it?",
                UnknownError: "Unknown error removing account."
            }
        }
    }
};
