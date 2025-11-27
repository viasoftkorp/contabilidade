using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

public class ConciliacaoContabilApuracaoDetalhamento
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LegacyId { get; set; }

    public int LegacyCompanyId { get; set; }
    public DateOnly Data { get; set; }
    public int NumeroLancamento { get; set; }
    public string Historico { get; set; }
    public decimal Valor { get; set; }
    public int CodigoConta { get; set; }
    public string CodigoFornecedorCliente { get; set; }
    public int IdConciliacaoContabilApuracao { get; set; }
    public int? IdTipoLancamento { get; set; }

    public ConciliacaoContabilApuracaoDetalhamento()
    {
    }

    public ConciliacaoContabilApuracaoDetalhamento(ConciliacaoContabilApuracaoDetalhamentoInput input)
    {
        LegacyId = input.LegacyId;
        LegacyCompanyId = input.LegacyCompanyId;
        Data = input.Data;
        NumeroLancamento = input.NumeroLancamento;
        Historico = input.Historico;
        Valor = input.Valor;
        CodigoConta = input.CodigoConta;
        CodigoFornecedorCliente = input.CodigoFornecedorCliente;
        IdConciliacaoContabilApuracao = input.IdConciliacaoContabilApuracao;
        IdTipoLancamento = input.IdTipoLancamento;
    }
}