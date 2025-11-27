using System;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class RegistroF100: Entity, IPeriodoContabil
{
    public int LegacyId { get; set; }
    public string CodigoFornecedor { get; set; }
    public string DescricaoOperacao { get; set; }
    public DateOnly Data{ get; set; }
    public int LegacyCompanyId { get; set; }
    public decimal ValorPis { get; set; }
    public decimal  AliquotaPis { get; set; }
    public decimal ValorCofins { get; set; }
    public decimal  AliquotaCofins { get; set; }
    public int IndicadorOperacao { get; set; }
    public bool ParametrizacaoLancamento { get; set; }
}