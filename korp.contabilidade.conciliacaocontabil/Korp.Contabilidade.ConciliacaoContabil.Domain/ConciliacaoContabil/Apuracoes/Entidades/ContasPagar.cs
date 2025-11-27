using System;
using System.ComponentModel.DataAnnotations.Schema;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Viasoft.Core.DDD.Entities;

// ReSharper disable once CheckNamespace
// trocado namespace para não conflitar com o .Entidades da Conciliacao
namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;

public class ContasPagar: Entity, IPeriodoFinanceiro
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
    public int LegacyId { get; set; }
    public int LegacyIdOrigem { get; set; }
    public int? IdDctf { get; set; }
    public string Parcela { get; set; }
    public int NumeroExtrato { get; set; }
    public bool? Adiantamento { get; set; }

    [NotMapped]
    public ContasPagarStatus SituacaoTitulo { get; set; }
    
}