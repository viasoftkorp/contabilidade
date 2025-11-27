using System;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public interface IContasFinanceiroDto
{
    public DateOnly Data { get; }
    public string Documento { get; }
    public string RazaoSocial { get; }
    public decimal Valor { get; }
    public string Codigo { get; }
    public string Status { get; }   
    public int LegacyCompanyId { get; }
    public string Parcela { get; }
    public bool? Adiantamento { get; }
    public TipoValorApuracaoConciliacaoContabil TipoValorApuracaoConciliacaoContabil { get; }
}

public enum ContaTipoFinanceiro
{
    Pagamento = 0,
    Recebimento = 1
}