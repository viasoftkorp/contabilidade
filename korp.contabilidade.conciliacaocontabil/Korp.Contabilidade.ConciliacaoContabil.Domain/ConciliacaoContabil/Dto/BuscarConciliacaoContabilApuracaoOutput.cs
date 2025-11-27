using System;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public class BuscarConciliacaoContabilApuracaoOutput
{
    public int LegacyId { get; set; }
    public int LegacyCompanyId { get; set; }
    public string CompanyName { get; set; }
    public DateOnly Data { get; set; }
    public string Documento { get; set; }
    public string Parcela { get; set; }
    public string CodigoFornecedorCliente { get; set; }
    public string RazaoSocialFornecedorCliente { get; set; }
    public decimal Valor { get; set; }
    public bool Conciliado { get; set; }
    public string Chave { get; set; }
    public int IdConciliacaoContabil { get; set; }
    public string DescricaoTipoValorApuracao { get; set; }
    public TipoValorApuracaoConciliacaoContabil TipoValorApuracao { get; set; }
    
    public BuscarConciliacaoContabilApuracaoOutput() { }
}