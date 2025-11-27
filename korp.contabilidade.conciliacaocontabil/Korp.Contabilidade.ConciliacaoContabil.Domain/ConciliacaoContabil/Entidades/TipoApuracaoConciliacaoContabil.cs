using System;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

public enum TipoApuracaoConciliacaoContabil
{
    ExtratoBancarioContasPagasRecebidas = 1,
    ContasReceberFinanceiro = 2,
    IcmsCreditarFiscal = 3,
    PisCreditarFiscal = 4,
    CofinsCreditarFiscal = 5,
    IssCreditarFiscal = 6,
    IcmsstCreditarFiscal = 7,
    ValorDosImobilizadosPatrimonial = 8,
    AdiantamentoFornecedoresFinanceiro = 9,
    ContasPagarFinanceiro = 10,
    IcmsRecolherFiscal = 11,
    PisRecolherFiscal = 12,
    CofinsRecolherFiscal = 13,
    IssRecolherFiscal = 14,
    IcmsstRecolherFiscal = 15,
    AdiantamentoClientesFinanceiro = 16,
    JurosPagosFinanceiro = 17,
    MultasPagasFinanceiro = 18,
    DescontosObtidosFinanceiro = 19,
    JurosRecebidosFinanceiro = 20,
    MultasRecebidasFinanceiro = 21,
    DescontosConcedidosFinanceiro = 22,
    ValorBrutoDeVendasFaturamento = 23,
    IcmsSobreVendasFaturamento = 24,
    PisSobreVendaFaturamento = 25,
    CofinsSobreVendaFaturamento = 26,
    IssSobreVendasFaturamento = 27,
    DepreciacoesPatrimonial = 28,
    IpiCreditarFiscal = 29,
    IpiRecolherFiscal = 30
}
public static class TipoApuracaoConciliacaoContabilDescricoes
{
    public static string Descricao(this TipoApuracaoConciliacaoContabil tipo)
    {
        return tipo switch
        { 
            TipoApuracaoConciliacaoContabil.ExtratoBancarioContasPagasRecebidas  => "Extrato Bancario (Financeiro)",
            TipoApuracaoConciliacaoContabil.ContasReceberFinanceiro  => "Contas a Receber (Financeiro)",
            TipoApuracaoConciliacaoContabil.IcmsCreditarFiscal  => "ICMS a Creditar (Fiscal)",
            TipoApuracaoConciliacaoContabil.PisCreditarFiscal  => "PIS a Creditar (Fiscal)",
            TipoApuracaoConciliacaoContabil.CofinsCreditarFiscal  => "COFINS a Creditar (Fiscal)",
            TipoApuracaoConciliacaoContabil.IssCreditarFiscal  => "ISS a Creditar (Fiscal)",
            TipoApuracaoConciliacaoContabil.IcmsstCreditarFiscal  => "ICMS-ST a Creditar (Fiscal)",
            TipoApuracaoConciliacaoContabil.ValorDosImobilizadosPatrimonial  => "Valor Imobilizado (Patrimonial)",
            TipoApuracaoConciliacaoContabil.AdiantamentoFornecedoresFinanceiro  => "Adiantamento Fornecedores (Financeiro)",
            TipoApuracaoConciliacaoContabil.ContasPagarFinanceiro  => "Contas a Pagar (Financeiro)",
            TipoApuracaoConciliacaoContabil.IcmsRecolherFiscal  => "ICMS a Recolher (Fiscal)",
            TipoApuracaoConciliacaoContabil.PisRecolherFiscal  => "PIS a Recolher (Fiscal)",
            TipoApuracaoConciliacaoContabil.CofinsRecolherFiscal  => "COFINS a Recolher (Fiscal)",
            TipoApuracaoConciliacaoContabil.IssRecolherFiscal  => "ISS a Recolher (Fiscal)",
            TipoApuracaoConciliacaoContabil.IcmsstRecolherFiscal  => "ICMS-ST a Recolher (Fiscal)",
            TipoApuracaoConciliacaoContabil.AdiantamentoClientesFinanceiro  => "Adiantamento a Clientes (Financeiro)",
            TipoApuracaoConciliacaoContabil.JurosPagosFinanceiro  => "Juros Pagos (Financeiro)",
            TipoApuracaoConciliacaoContabil.MultasPagasFinanceiro  => "Multas Pagas (Financeiro)",
            TipoApuracaoConciliacaoContabil.DescontosObtidosFinanceiro  => "Descontos Obtidos (Financeiro)",
            TipoApuracaoConciliacaoContabil.JurosRecebidosFinanceiro  => "Juros Recebidos (Financeiro)",
            TipoApuracaoConciliacaoContabil.MultasRecebidasFinanceiro  => "Multas Recebidas (Financeiro)",
            TipoApuracaoConciliacaoContabil.DescontosConcedidosFinanceiro  => "Descontos Concedidos (Financeiro)",
            TipoApuracaoConciliacaoContabil.ValorBrutoDeVendasFaturamento  => "Valor Bruto Vendas (Faturamento)",
            TipoApuracaoConciliacaoContabil.IcmsSobreVendasFaturamento  => "ICMS sobre Vendas (Faturamento)",
            TipoApuracaoConciliacaoContabil.PisSobreVendaFaturamento  => "PIS sobre Vendas (Faturamento)",
            TipoApuracaoConciliacaoContabil.CofinsSobreVendaFaturamento  => "COFINS sobre Vendas (Faturamento)",
            TipoApuracaoConciliacaoContabil.IssSobreVendasFaturamento  => "ISS sobre Vendas (Faturamento)",
            TipoApuracaoConciliacaoContabil.DepreciacoesPatrimonial  => "Depreciações (Patrimonial)",
            TipoApuracaoConciliacaoContabil.IpiCreditarFiscal  => "IPI a Creditar (Fiscal)",
            TipoApuracaoConciliacaoContabil.IpiRecolherFiscal  => "IPI a Recolher (Fiscal)",
            _ => throw new ArgumentOutOfRangeException(nameof(tipo), tipo, null)
        };
    }
}