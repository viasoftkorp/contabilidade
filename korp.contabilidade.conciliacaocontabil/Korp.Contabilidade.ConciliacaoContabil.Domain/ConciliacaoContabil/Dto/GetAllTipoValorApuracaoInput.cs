using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Data.Attributes;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public class GetAllTipoValorApuracaoInput : IMayHaveFilter, IPagedResultRequest, ILimitedResultRequest
{
    [StrictRequired]
    public TipoApuracaoConciliacaoContabil TipoApuracaoConciliacaoContabil { get; set; }
    public string Filter { get; set; }
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = 100;
}