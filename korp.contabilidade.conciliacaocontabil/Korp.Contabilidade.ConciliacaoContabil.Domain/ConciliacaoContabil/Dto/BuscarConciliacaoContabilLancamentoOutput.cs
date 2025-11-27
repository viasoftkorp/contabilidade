using System;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public class BuscarConciliacaoContabilLancamentoOutput
{
    public int LegacyId { get; set; }
    public int LegacyCompanyId { get; set; }
    public string CompanyName { get; set; }
    public DateOnly Data { get; set; }
    public int NumeroLancamento { get; set; }
    public string Historico { get; set; }
    public decimal Valor { get; set; }
    public int CodigoConta { get; set; }
    public string NomeConta { get; set; }
    public bool Conciliado { get; set; }
    public string Chave { get; set; }
    public int IdConciliacaoContabil { get; set; }
    public string CodigoFornecedorCliente { get; set; }
    public string RazaoSocialFornecedorCliente { get; set; }
    public int? IdTipoLancamento { get; set; }
    public string CodigoTipoLancamento { get; set; }
    public string DescricaoTipoLancamento { get; set; }

    public BuscarConciliacaoContabilLancamentoOutput() { }
}