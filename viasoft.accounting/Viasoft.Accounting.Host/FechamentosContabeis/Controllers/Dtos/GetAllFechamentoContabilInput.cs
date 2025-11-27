using System;
using Viasoft.Core.DDD.Application.Dto.Paged;

namespace Viasoft.Accounting.Host.PeriodosContabeis.Controllers.Dtos;

public class GetAllFechamentoContabilInput : PagedFilteredAndSortedRequestInput
{
    public DateTime? DataLancamento { get; set; }
}