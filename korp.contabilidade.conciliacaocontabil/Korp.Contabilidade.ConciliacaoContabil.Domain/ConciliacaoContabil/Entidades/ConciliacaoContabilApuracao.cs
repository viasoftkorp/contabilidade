using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

public class ConciliacaoContabilApuracao
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
    public bool Conciliado { get; set; }
    public string Chave { get; set; }
    public int IdConciliacaoContabil { get; set; }
    public string DescricaoTipoValorApuracao { get; set; }
    public TipoValorApuracaoConciliacaoContabil TipoValorApuracao { get; set; }
    public List<ConciliacaoContabilApuracaoDetalhamento> ApuracoesDetalhamento { get; set; }= new();

    public void GerarChaveConciliacao(decimal valorAgrupado)
    {
        Chave = Data.ToString("yyyyMMdd") + valorAgrupado.ToString("F") + CodigoFornecedorCliente + LegacyCompanyId;
    }

    public void ConciliadoComLancamentos(IEnumerable<ConciliacaoContabilLancamento> lancamentosCorrespondentes)
    {
        foreach (var lancamentoCorrespondente in lancamentosCorrespondentes)
        {
            ApuracoesDetalhamento.Add(new ConciliacaoContabilApuracaoDetalhamento
            {
                LegacyCompanyId = lancamentoCorrespondente.LegacyCompanyId,
                Data = lancamentoCorrespondente.Data,
                NumeroLancamento = lancamentoCorrespondente.NumeroLancamento,
                Historico = lancamentoCorrespondente.Historico,
                Valor = lancamentoCorrespondente.Valor,
                CodigoConta = lancamentoCorrespondente.CodigoConta,
                CodigoFornecedorCliente = lancamentoCorrespondente.CodigoFornecedorCliente,
                IdConciliacaoContabilApuracao = LegacyId,
                IdTipoLancamento = lancamentoCorrespondente.IdTipoLancamento
            });
        }
        
        Conciliado = true;
    }

    public void NaoConciliada()
    {
        Conciliado = false;
    }
}