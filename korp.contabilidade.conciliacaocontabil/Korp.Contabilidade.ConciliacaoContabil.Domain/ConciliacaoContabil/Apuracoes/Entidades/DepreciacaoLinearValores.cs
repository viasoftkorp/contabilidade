using System;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class DepreciacaoLinearValores: Entity,IPeriodoDepreciacao
{
    public int LegacyIdBem { get; set; }
    public DateOnly Data { get; set; }
    public decimal Valor { get; set; }
    public decimal ValorPis { get; set; }
    public decimal ValorCofins { get; set; }
}