using System;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Patrimonial.Dto;

public class PatrimonialDto
{
    public DateOnly Data { get; set; }
    public string Documento { get; set; }
    public string RazaoSocial { get; set; }
    public decimal Valor { get; set; }
    public string Codigo { get; set; }
    public string Status { get; set; }   
    public int LegacyCompanyId { get; set; }
    public string Parcela { get; set; }  
    public TipoValorApuracaoConciliacaoContabil TipoValorApuracao { get; set; }
}