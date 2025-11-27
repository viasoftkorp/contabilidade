using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Viasoft.Accounting.Domain.Dtos;

namespace Viasoft.Accounting.Domain.Services
{
    public interface IAccountingEntriesService
    {
        Task AddAccountingEntry(AccountingEntryDto entry, List<AccountingEntryItemDto> items);
        Task RemoveAccountingEntries(Guid sourceId);
        Task<MotivoEstornarLancamentoContabilResult> Estornar(Guid sourceId);
    }
}