using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public class GetAllFornecedorClienteCodigoInput : IMayHaveFilter, IPagedResultRequest, ILimitedResultRequest
{
    public string Filter { get; set; }
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = 100;
}