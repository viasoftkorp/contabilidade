using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ServicosExternos.TenantManagement.DTOs
{
    public class GetEnvironmentsInput: PagedFilteredAndSortedRequestInput
    {
        public string UnitId { get; set; }
    }
}