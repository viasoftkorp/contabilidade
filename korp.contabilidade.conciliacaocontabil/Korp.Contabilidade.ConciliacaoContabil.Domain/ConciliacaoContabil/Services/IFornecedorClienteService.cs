using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;

public interface IFornecedorClienteService
{
    Task<PagedResultDto<FornecedorClienteCodigoOutput>> GetAllCodigos(GetAllFornecedorClienteCodigoInput input);
    Task<bool> IsCodigoValid(string codigo);
}