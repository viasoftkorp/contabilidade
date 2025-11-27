using System;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public class BuscarConciliacaoContabilApuracaoDetalhamentoOutput
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
    public string CodigoFornecedorCliente { get; set; }
    public int IdConciliacaoContabilApuracao { get; set; }
    public int? IdTipoLancamento { get; set; }
    public string CodigoTipoLancamento { get; set; }
    public string DescricaoTipoLancamento { get; set; }

    public BuscarConciliacaoContabilApuracaoDetalhamentoOutput() { }

    public BuscarConciliacaoContabilApuracaoDetalhamentoOutput(ConciliacaoContabilApuracaoDetalhamento entity, string empresaNome, string nomeConta, string codigoTipoLancamento, string descricaoTipoLancamento)
    {
        LegacyId = entity.LegacyId;
        LegacyCompanyId = entity.LegacyCompanyId;
        CompanyName = empresaNome;
        Data = entity.Data;
        NumeroLancamento = entity.NumeroLancamento;
        Historico = entity.Historico;
        Valor = entity.Valor;
        CodigoConta = entity.CodigoConta;
        NomeConta = nomeConta;
        CodigoFornecedorCliente = entity.CodigoFornecedorCliente;
        IdConciliacaoContabilApuracao = entity.IdConciliacaoContabilApuracao;
        IdTipoLancamento = entity.IdTipoLancamento;
        CodigoTipoLancamento = codigoTipoLancamento;
        DescricaoTipoLancamento = descricaoTipoLancamento;
    }
}