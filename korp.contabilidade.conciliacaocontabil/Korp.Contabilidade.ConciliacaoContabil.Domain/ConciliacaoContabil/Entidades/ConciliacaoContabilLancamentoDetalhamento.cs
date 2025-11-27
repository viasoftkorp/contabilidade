using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

public class ConciliacaoContabilLancamentoDetalhamento
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LegacyId { get; set; }

    public int LegacyCompanyId { get; set; }
    public DateOnly Data { get; set; }
    public string Documento { get; set; }
    public string Parcela { get; set; }
    public string CodigoFornecedorCliente { get; set; }
    public string RazaoSocialFornecedorCliente { get; set; }
    public decimal Valor { get; set; }
    public int IdConciliacaoContabilLancamento { get; set; }
    public TipoValorApuracaoConciliacaoContabil TipoValorApuracao { get; set; }
    public string DescricaoTipoValorApuracao { get; set; }

    public ConciliacaoContabilLancamentoDetalhamento()
    {
    }

    public ConciliacaoContabilLancamentoDetalhamento(ConciliacaoContabilLancamentoDetalhamentoInput input)
    {
        LegacyId = input.LegacyId;
        LegacyCompanyId = input.LegacyCompanyId;
        Data = input.Data;
        Documento = input.Documento;
        Parcela = input.Parcela;
        CodigoFornecedorCliente = input.CodigoFornecedorCliente;
        RazaoSocialFornecedorCliente = input.RazaoSocialFornecedorCliente;
        Valor = input.Valor;
        IdConciliacaoContabilLancamento = input.IdConciliacaoContabilLancamento;
        TipoValorApuracao = input.TipoValorApuracao;
        DescricaoTipoValorApuracao = input.DescricaoTipoValorApuracao;
    }
}