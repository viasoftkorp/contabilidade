// ReSharper disable once CheckNamespace
// trocado namespace para não conflitar com o .Entidades da Conciliacao

using System.ComponentModel.DataAnnotations;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;

public class Estoque
{
    [Key]
    public string Codigo { get; set; }
    public int? CodigoConta { get; set; }
}