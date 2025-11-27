using System;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class FaturamentoNotaFiscal: Entity, IPeriodoContabil
{
    public int LegacyId { get; set; }
    public int LegacyCompanyId { get; set; }
    public DateOnly Data { get; set; }
    public int Documento { get; set; }
    public string CodigoCliente { get; set; }
    public string RazaoSocialCliente { get; set; }
    public decimal Valor { get; set; }
    public bool Devolucao { get; set; }
    public string Situacao { get; set; }
}