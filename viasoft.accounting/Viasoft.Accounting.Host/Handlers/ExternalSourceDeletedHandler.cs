using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Accounting.Domain.Services;
using Viasoft.Accounting.Host.Messages;
using Viasoft.Core.DDD.Attributes;

namespace Viasoft.Accounting.Host.Handlers
{
    public class ExternalSourceDeletedHandler : IHandleMessages<CteCanceled>
    {
        private readonly IAccountingEntriesService _accountingEntriesService;

        public ExternalSourceDeletedHandler(IAccountingEntriesService accountingEntriesService)
        {
            _accountingEntriesService = accountingEntriesService;
        }
        public async Task Handle(CteCanceled message)
        {
            await _accountingEntriesService.RemoveAccountingEntries(message.CteId);
        }

    }
}