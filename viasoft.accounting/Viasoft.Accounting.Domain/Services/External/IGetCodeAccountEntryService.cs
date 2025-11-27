using System.Threading.Tasks;

namespace Viasoft.Accounting.Domain.Services.External;

public interface IGetCodeAccountEntryService
    {
        Task<EntryCodeResponseOutput> GetEntryCode();
    }