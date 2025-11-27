using System;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class PatrimonialBens : Entity, IPeriodoPatrimonial
{
    public int LegacyId { get; set; }
    public int LegacyCompanyId { get; set; }
    public string CodigoBem { get; set; }
    public string Nome { get; set; }
    public string NumeroNota { get; set; }
    public DateOnly? DataEntrada { get; set; }
    public DateOnly? DataSaida { get; set; }
    public string Fornecedor { get; set; }
    public decimal Valor { get; set; }
    public decimal ValorSaida { get; set; }
    public string CodigoGrupoBem { get; set; }
    public bool? EfdpcGerarBem { get; set; }
    public int OpcaoCreditoEfd { get; set; }
    public string RazaoSocial { get; set; }
}