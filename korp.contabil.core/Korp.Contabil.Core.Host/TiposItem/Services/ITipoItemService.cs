using System;
using System.Threading.Tasks;
using Korp.Contabil.Core.Domain.TiposItem.Dtos;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabil.Core.Host.TiposItem.Services;

public interface ITipoItemService
{
    public Task<TipoItemOutput> Get(Guid id);
    public Task<PagedResultDto<TipoItemOutput>> GetList(PagedFilteredAndSortedRequestInput input);
}
