using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Accounting.Domain.Services;
using Viasoft.Accounting.Host.Messages;

namespace Viasoft.Accounting.Host.Handlers
{
    public class CreateAccountingEntryFromExternalSourceHandler : IHandleMessages<CreateExternalSourceEntry>
    {

        private readonly IAccountingEntriesService _accountingEntriesService;

        public CreateAccountingEntryFromExternalSourceHandler(IAccountingEntriesService accountingEntriesService)
        {
            _accountingEntriesService = accountingEntriesService;
        }

        public async Task Handle(CreateExternalSourceEntry message)
        {

            await _accountingEntriesService.AddAccountingEntry(message.AccountingEntry, message.AccountingEntryItems);            
            
        }      
        public async Task Handle(CreateExternalSourceEntryV2 message)
        {

            await _accountingEntriesService.AddAccountingEntry(message.AccountingEntry, message.AccountingEntryItems);            
            
        }
    }
}