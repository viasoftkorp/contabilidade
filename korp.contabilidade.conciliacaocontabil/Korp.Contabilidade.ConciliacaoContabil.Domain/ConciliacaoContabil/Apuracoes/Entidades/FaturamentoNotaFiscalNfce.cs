using System;
using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class FaturamentoNotaFiscalNfce: Entity
{
    public int LegacyId { get; set; }
    public int LegacyFaturamentoCaixaId { get; set; }
    public DateTime Data { get; set; }
    public int Documento { get; set; }
    public string CodigoCliente { get; set; }
    public string RazaoSocialCliente { get; set; }
    public decimal Valor { get; set; }
    public string Situacao { get; set; }
}