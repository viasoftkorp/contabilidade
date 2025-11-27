using System;
using System.ComponentModel.DataAnnotations.Schema;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class ContasReceber: Entity, IPeriodoFinanceiro
{
    public DateOnly DataEntrada { get; set; }
    public DateOnly? DataPagamento { get; set; }
    public string Documento { get; set; }
    public string RazaoSocial { get; set; }
    public decimal Valor { get; set; }
    public decimal Juros { get; set; }
    public decimal Multa { get; set; }
    public decimal Desconto { get; set; }
    public decimal? Retencao { get; set; }
    public string Codigo { get; set; }
    public string Status { get; set; }   
    public int LegacyCompanyId { get; set; }
    public string Parcela { get; set; }
    public int NumeroExtrato { get; set; }
    public bool? Adiantamento { get; set; }


    [NotMapped]
    public ContasReceberStatus SituacaoTitulo { get; set; }
}