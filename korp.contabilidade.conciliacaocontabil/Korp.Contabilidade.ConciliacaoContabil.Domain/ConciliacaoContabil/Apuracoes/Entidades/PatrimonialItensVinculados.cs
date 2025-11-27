using System;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class PatrimonialItensVinculados: Entity, IPeriodoPatrimonialItens
{
    public int LegacIdBem { get; set; }
    public DateOnly Data { get; set; }
    public decimal Valor { get; set; } 
    public string CodigoFornecedorCliente { get; set; }
    public string RazaoFornecedorCliente { get; set; }

}