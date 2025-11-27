using System;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.LancamentoContabil.Dto;

public class LancamentoContabilOutput
{
    public int LegacyCompanyId { get; set; } 
    public DateOnly DataLancamento { get; set; } 
    public int NumeroLancamento { get; set; } 
    public string Historico { get; set; } 
    public decimal Valor { get; set; } 
    public int CodigoConta { get; set; } 
    public string DescricaoConta { get; set; } 
    public string CodigoFornecedorCliente { get; set; }
    public int? IdTipoLancamento { get; set; }
}