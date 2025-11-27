using System.Linq;
using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Accounting.Host.Contracts;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;

namespace Viasoft.Accounting.Host.Handlers
{
    public class AccountingOperationDeletedHandler : IHandleMessages<AccountingOperationDeleted>
    {
        private readonly IRepository<AccountingOperationEntryRule> _accountingOperationEntryRules;
        private readonly IUnitOfWork _unitOfWork;

        public AccountingOperationDeletedHandler(IRepository<AccountingOperationEntryRule> accountingOperationEntryRules, IUnitOfWork unitOfWork)
        {
            _accountingOperationEntryRules = accountingOperationEntryRules;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AccountingOperationDeleted message)
        {
            var accountingEntriesFromDeletedAccountingOperation = _accountingOperationEntryRules.Where(e => e.AccountingOperationId == message.AccountingOperationId);
            using (_unitOfWork.Begin())
            {
                foreach (var accountingOperationEntryRule in accountingEntriesFromDeletedAccountingOperation)
                {
                    await _accountingOperationEntryRules.DeleteAsync(accountingOperationEntryRule);
                }
                await _unitOfWork.CompleteAsync();
            }

        }
    }
}