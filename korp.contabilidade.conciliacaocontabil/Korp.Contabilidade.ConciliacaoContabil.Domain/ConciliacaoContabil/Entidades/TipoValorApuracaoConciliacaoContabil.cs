using System;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

public enum TipoValorApuracaoConciliacaoContabil
{
    EntradaPagamentoFinanceiro = 1,
    PagamentoFinanceiro = 2,
    EntradaRecebimentoFinanceiro = 3,
    RecebimentoFinanceiro = 4,
    ExtratoFinanceiroPagamento = 5,
    ExtratoFinanceiroRecebimento = 6,
    IcmsCreditarFiscal = 7,
    PisCreditarFiscal = 8,
    CofinsCreditarFiscal = 9,
    IpiCreditarFiscal = 10,
    IcmsRecolherFiscal = 11,
    PisRecolherFiscal = 12,
    CofinsRecolherFiscal = 13,
    IpiRecolherFiscal = 14,
    IcmsStRecolherFiscal = 15,
    IssRecolherFiscal = 16,
    JurosPagamentoFinanceiro = 17,
    MultasPagamentoFinanceiro = 18,
    DescontosObtidosFinanceiro = 19,
    JurosRecebimentoFinanceiro = 20,
    MultasRecebimentoFinanceiro = 21,
    DescontosConcedidosFinanceiro = 22,
    ValorBrutoVendasFaturamento = 23,
    ValorImobilizadosPatrimonial = 24,
    DepreciacaoPatrimonial = 25
}

public static class TipoValorApuracaoConciliacaoContabilDescricoes
{
    public static string Descricao(this TipoValorApuracaoConciliacaoContabil tipo)
    {
        return tipo switch
        {
            TipoValorApuracaoConciliacaoContabil.EntradaPagamentoFinanceiro => "Entrada no Financeiro (Contas a Pagar)",
            TipoValorApuracaoConciliacaoContabil.PagamentoFinanceiro => "Pagamento no Financeiro (Contas a Pagar)",
            TipoValorApuracaoConciliacaoContabil.EntradaRecebimentoFinanceiro => "Entrada no Financeiro (Contas a Receber)",
            TipoValorApuracaoConciliacaoContabil.RecebimentoFinanceiro => "Recebimento no Financeiro (Contas a Receber)",
            TipoValorApuracaoConciliacaoContabil.ExtratoFinanceiroRecebimento => "Extrato Bancário no Financeiro Recebimento",
            TipoValorApuracaoConciliacaoContabil.ExtratoFinanceiroPagamento => "Extrato Bancário no Financeiro Pagamento",
            TipoValorApuracaoConciliacaoContabil.IcmsCreditarFiscal => "ICMS a Creditar no Fiscal",
            TipoValorApuracaoConciliacaoContabil.PisCreditarFiscal => "PIS a Creditar no Fiscal",
            TipoValorApuracaoConciliacaoContabil.CofinsCreditarFiscal => "COFINS a Creditar no Fiscal",
            TipoValorApuracaoConciliacaoContabil.IpiCreditarFiscal => "IPI a Creditar no Fiscal",
            TipoValorApuracaoConciliacaoContabil.IcmsRecolherFiscal => "ICMS a Recolher no Fiscal",
            TipoValorApuracaoConciliacaoContabil.PisRecolherFiscal => "PIS a Recolher no Fiscal",
            TipoValorApuracaoConciliacaoContabil.CofinsRecolherFiscal => "COFINS a Recolher no Fiscal",
            TipoValorApuracaoConciliacaoContabil.IpiRecolherFiscal => "IPI a Recolher no Fiscal",
            TipoValorApuracaoConciliacaoContabil.IcmsStRecolherFiscal => "ICMS-ST a Recolher no Fiscal",
            TipoValorApuracaoConciliacaoContabil.IssRecolherFiscal => "ISS a Recolher no Fiscal",
            TipoValorApuracaoConciliacaoContabil.JurosPagamentoFinanceiro => "Juros Pagamento no Financeiro",
            TipoValorApuracaoConciliacaoContabil.MultasPagamentoFinanceiro => "Multas Pagas no Financeiro",
            TipoValorApuracaoConciliacaoContabil.DescontosObtidosFinanceiro => "Descontos Obtidos no Financeiro",
            TipoValorApuracaoConciliacaoContabil.JurosRecebimentoFinanceiro => "Juros Recebimento no Financeiro",
            TipoValorApuracaoConciliacaoContabil.MultasRecebimentoFinanceiro => "Multas Recebimento no Financeiro",
            TipoValorApuracaoConciliacaoContabil.DescontosConcedidosFinanceiro => "Descontos Concedidos no Financeiro",
            TipoValorApuracaoConciliacaoContabil.ValorBrutoVendasFaturamento => "Valor Bruto de Vendas",
            TipoValorApuracaoConciliacaoContabil.ValorImobilizadosPatrimonial => "Valor dos Imobilizados",
            TipoValorApuracaoConciliacaoContabil.DepreciacaoPatrimonial => "Valor da Depreciação",
            _ => throw new ArgumentOutOfRangeException(nameof(tipo), tipo, null)
        };
    }
}