using Viasoft.Core.DDD.Entities;

// ReSharper disable once CheckNamespace
// trocado namespace para não conflitar com o .Entidades da Conciliacao
namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;

public class PlanoConta: Entity
{
    public int Codigo { get; set; }
    public string Descricao { get; set; }
}