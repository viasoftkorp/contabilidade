using System;
using System.Text;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public class BuscarConciliacaoContabilOutput
{
    public int LegacyId { get; set; }
    public string Descricao { get; set; }
    public DateOnly DataInicial { get; set; }
    public DateOnly DataFinal { get; set; }
    public ConciliacaoContabilStatus? Status { get; set; }
    public TipoApuracaoConciliacaoContabil TipoApuracaoConciliacaoContabil { get; set; }
    public string Erro { get; set; }

    public BuscarConciliacaoContabilOutput()
    {
    }

    public BuscarConciliacaoContabilOutput(ConciliacaoContabil conciliacaoContabil)
    {
        LegacyId = conciliacaoContabil.LegacyId;
        Descricao = conciliacaoContabil.Descricao;
        DataInicial = conciliacaoContabil.DataInicial;
        DataFinal = conciliacaoContabil.DataFinal;
        Status = conciliacaoContabil.Status;
        TipoApuracaoConciliacaoContabil = conciliacaoContabil.TipoConciliacaoContabil.TipoApuracao;
        Erro = conciliacaoContabil.Erro is null ? null : Encoding.UTF8.GetString(conciliacaoContabil.Erro);
    }
}