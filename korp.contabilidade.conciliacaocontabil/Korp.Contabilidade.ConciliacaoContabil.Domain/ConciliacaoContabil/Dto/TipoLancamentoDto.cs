using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public class TipoLancamentoDto
{
    public int LegacyId { get; set; }
    public string Codigo { get; set; }
    public string Descricao { get; set; }

    public TipoLancamentoDto()
    {
    }

    public TipoLancamentoDto(TipoLancamento tipoLancamento)
    {
        LegacyId = tipoLancamento.LegacyId;
        Codigo = tipoLancamento.Codigo;
        Descricao = tipoLancamento.Descricao;
    }
}