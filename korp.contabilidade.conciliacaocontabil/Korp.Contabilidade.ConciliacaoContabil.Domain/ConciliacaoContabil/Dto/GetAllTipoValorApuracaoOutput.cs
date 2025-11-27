using System.Collections.Generic;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public class GetAllTipoValorApuracaoOutput
{
    public List<TipoValorApuracaoDto> Items { get; set; }

    public GetAllTipoValorApuracaoOutput()
    {
    }

    public GetAllTipoValorApuracaoOutput(List<TipoValorApuracaoDto> items)
    {
        Items = items;
    }
}