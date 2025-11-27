using System;
using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class Extrato: Entity
{
    public int LegacyId { get; set; }
    public DateOnly Data { get; set; }
    public decimal Valor { get; set; }
    public string DebitoCredito { get; set; }

}