using System.Collections.Generic;
using System.Linq;

// Alias for readability
using TipoValor = Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades.TipoValorApuracaoConciliacaoContabil;
using TipoApuracao = Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades.TipoApuracaoConciliacaoContabil;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

public class TipoValorApuracaoConciliacaoContabilMap
{
    private static readonly Dictionary<TipoApuracao, TipoValor[]> Items =
        new()
        {
            {TipoApuracao.ExtratoBancarioContasPagasRecebidas, new[] {TipoValor.ExtratoFinanceiroPagamento}},
            {TipoApuracao.ContasReceberFinanceiro, new[] {TipoValor.RecebimentoFinanceiro}},
            {TipoApuracao.IcmsCreditarFiscal, new[] {TipoValor.IcmsCreditarFiscal}},
            {TipoApuracao.PisCreditarFiscal, new[] {TipoValor.PisCreditarFiscal}},
            {TipoApuracao.CofinsCreditarFiscal, new[] {TipoValor.CofinsCreditarFiscal}},
            {TipoApuracao.IssCreditarFiscal, new[] {TipoValor.IssRecolherFiscal}},
            {TipoApuracao.IcmsstCreditarFiscal, new[] {TipoValor.IcmsStRecolherFiscal}},
            {TipoApuracao.ValorDosImobilizadosPatrimonial, new[] {TipoValor.ValorImobilizadosPatrimonial}},
            {TipoApuracao.AdiantamentoFornecedoresFinanceiro, new[] {TipoValor.PagamentoFinanceiro, TipoValor.EntradaPagamentoFinanceiro}},
            {TipoApuracao.ContasPagarFinanceiro, new[] {TipoValor.PagamentoFinanceiro, TipoValor.EntradaPagamentoFinanceiro}},
            {TipoApuracao.IcmsRecolherFiscal, new[] {TipoValor.IcmsRecolherFiscal}},
            {TipoApuracao.PisRecolherFiscal, new[] {TipoValor.PisRecolherFiscal}},
            {TipoApuracao.CofinsRecolherFiscal, new[] {TipoValor.CofinsRecolherFiscal}},
            {TipoApuracao.IssRecolherFiscal, new[] {TipoValor.IssRecolherFiscal}},
            {TipoApuracao.IcmsstRecolherFiscal, new[] {TipoValor.IcmsRecolherFiscal}},
            {TipoApuracao.AdiantamentoClientesFinanceiro, new[] {TipoValor.RecebimentoFinanceiro, TipoValor.EntradaRecebimentoFinanceiro}},
            {TipoApuracao.JurosPagosFinanceiro, new[] {TipoValor.JurosPagamentoFinanceiro}},
            {TipoApuracao.MultasPagasFinanceiro, new[] {TipoValor.MultasPagamentoFinanceiro}},
            {TipoApuracao.DescontosObtidosFinanceiro, new[] {TipoValor.DescontosObtidosFinanceiro}},
            {TipoApuracao.JurosRecebidosFinanceiro, new[] {TipoValor.JurosRecebimentoFinanceiro}},
            {TipoApuracao.MultasRecebidasFinanceiro, new[] {TipoValor.MultasRecebimentoFinanceiro}},
            {TipoApuracao.DescontosConcedidosFinanceiro, new[] {TipoValor.DescontosConcedidosFinanceiro}},
            {TipoApuracao.ValorBrutoDeVendasFaturamento, new[] {TipoValor.ValorBrutoVendasFaturamento}},
            {TipoApuracao.DepreciacoesPatrimonial, new[] {TipoValor.DepreciacaoPatrimonial}},
            {TipoApuracao.IpiCreditarFiscal, new[] {TipoValor.IpiCreditarFiscal}},
            {TipoApuracao.IpiRecolherFiscal, new[] {TipoValor.IpiRecolherFiscal}}
        };

    public static IEnumerable<TipoValor> Get(TipoApuracao tipoApuracaoConciliacaoContabil)
    {
        return Items.FirstOrDefault(i => i.Key == tipoApuracaoConciliacaoContabil).Value;
    }
}