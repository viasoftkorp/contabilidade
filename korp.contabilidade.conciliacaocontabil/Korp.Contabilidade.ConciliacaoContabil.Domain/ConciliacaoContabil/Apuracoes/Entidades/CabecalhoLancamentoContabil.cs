using System;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Viasoft.Core.DDD.Entities;

// ReSharper disable once CheckNamespace
// trocado namespace para não conflitar com o .Entidades da Conciliacao
namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;

public class CabecalhoLancamentoContabil: Entity,IPeriodoContabil
{
    public int LegacyId { get; set; }
    public DateOnly Data { get; set; }
    public int LegacyCompanyId { get; set; }
    public string CodigoCliente { get; set; }
    public string CodigoFornecedor { get; set; }
    public int NumeroLancamento { get; set; }
    public string TipoLancamento { get; set; }
}