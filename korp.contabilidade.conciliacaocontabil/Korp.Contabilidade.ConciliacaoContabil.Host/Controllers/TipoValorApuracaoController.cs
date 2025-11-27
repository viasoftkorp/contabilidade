using System;
using System.Linq;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;

namespace Korp.Contabilidade.ConciliacaoContabil.Host.Controllers;

public class TipoValorApuracaoController : BaseController
{
    [HttpGet("/contabilidade/conciliacao-contabil/tipo-valor-apuracao")]
    public GetAllTipoValorApuracaoOutput GetAllTipoValorApuracao([FromQuery] GetAllTipoValorApuracaoInput input)
    {
        var tipoValorApuracaoEnum = Enum.GetValues(typeof(TipoValorApuracaoConciliacaoContabil)).Cast<TipoValorApuracaoConciliacaoContabil>();
        var result = tipoValorApuracaoEnum
            .Select(tipoValorApuracao => new TipoValorApuracaoDto((int) tipoValorApuracao, tipoValorApuracao.Descricao()))
            .ToList();
        result = result
            .Where(t =>
                TipoValorApuracaoConciliacaoContabilMap.Get(input.TipoApuracaoConciliacaoContabil)
                    .Contains((TipoValorApuracaoConciliacaoContabil) t.Id))
            .ToList();
        if (!string.IsNullOrEmpty(input.Filter))
        {
            var filter = input.Filter.ToLower();
            result = result.Where(item => item.Description.ToLower().Contains(filter)).ToList();
        }

        result = result.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

        return new GetAllTipoValorApuracaoOutput(result);
    }
}