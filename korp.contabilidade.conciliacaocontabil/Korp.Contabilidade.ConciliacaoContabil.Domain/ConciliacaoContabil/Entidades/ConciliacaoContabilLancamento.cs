using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

public class ConciliacaoContabilLancamento
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LegacyId { get; set; }
    public int LegacyCompanyId { get; set; }
    public DateOnly Data { get; set; }
    public int NumeroLancamento { get; set; }
    public string Historico { get; set; }
    public decimal Valor { get; set; }
    public int CodigoConta { get; set; }
    public bool Conciliado { get; set; }
    public string Chave { get; set; }
    public int IdConciliacaoContabil { get; set; }
    public string CodigoFornecedorCliente { get; set; }
    public int? IdTipoLancamento { get; set; }
    public List<ConciliacaoContabilLancamentoDetalhamento> LancamentosDetalhamento { get; set; } = new();

    public void GerarChaveConciliacao()
    {
        Chave = Data.ToString("yyyyMMdd") + Valor.ToString("F") + CodigoFornecedorCliente + LegacyCompanyId;
    }

    public void ConciliadoComApuracoes(IEnumerable<ConciliacaoContabilApuracao> apuracoesCorrespondentes)
    {
        foreach (var apuracaoCorrespondente in apuracoesCorrespondentes)
        {
            LancamentosDetalhamento.Add(new ConciliacaoContabilLancamentoDetalhamento
            {
                LegacyCompanyId = apuracaoCorrespondente.LegacyCompanyId,
                Data = apuracaoCorrespondente.Data,
                Documento = apuracaoCorrespondente.Documento,
                Parcela = apuracaoCorrespondente.Parcela,
                CodigoFornecedorCliente = apuracaoCorrespondente.CodigoFornecedorCliente,
                RazaoSocialFornecedorCliente = apuracaoCorrespondente.RazaoSocialFornecedorCliente,
                Valor = apuracaoCorrespondente.Valor,
                TipoValorApuracao = apuracaoCorrespondente.TipoValorApuracao,
                DescricaoTipoValorApuracao = apuracaoCorrespondente.DescricaoTipoValorApuracao,
                IdConciliacaoContabilLancamento = LegacyId
            });
        }
        
        Conciliado = true;
    }

    public void NaoConciliado()
    {
        Conciliado = false;
    }
}