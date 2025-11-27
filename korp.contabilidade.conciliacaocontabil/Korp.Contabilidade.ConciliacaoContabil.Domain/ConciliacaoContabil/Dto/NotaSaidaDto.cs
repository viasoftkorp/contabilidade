using System;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public class NotaSaidaDto: IImpostoDto
{
    public DateOnly Data { get; set; }
    public string Documento { get; set; }
    public string RazaoSocial { get; set; }
    public decimal Valor { get; set; }
    public string Codigo { get; set; }
    public string Status { get; set; }   
    public int LegacyCompanyId { get; set; }
    public string Parcela { get; set; } 
}