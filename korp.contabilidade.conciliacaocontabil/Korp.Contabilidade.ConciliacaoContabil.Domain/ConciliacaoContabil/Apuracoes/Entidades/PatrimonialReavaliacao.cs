using System;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class PatrimonialReavaliacao: Entity, IPeriodoPatrimonialItens
{
    public string CodigoBem { get; set; }
    public DateOnly Data { get; set; }
    public decimal Valor { get; set; }
    public bool Operacao { get; set; }
}