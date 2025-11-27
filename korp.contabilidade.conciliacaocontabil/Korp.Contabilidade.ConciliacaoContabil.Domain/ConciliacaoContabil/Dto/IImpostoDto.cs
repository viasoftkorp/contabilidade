using System;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public interface IImpostoDto
{
    public DateOnly Data { get; }
    public string Documento { get; }
    public string RazaoSocial { get; }
    public decimal Valor { get; }
    public string Codigo { get; }
    public string Status { get; }   
    public int LegacyCompanyId { get; }
    public string Parcela { get; }
}

public enum TipoFiscal
{
    Credito = 0,
    Debito = 1
}