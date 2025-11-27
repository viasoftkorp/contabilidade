using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;

public interface IEmpresaService
{
    Task<PagedResultDto<EmpresaDto>> GetAllEmpresas(PagedFilteredAndSortedRequestInput input);
}