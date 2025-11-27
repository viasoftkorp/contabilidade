using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Accounting.Domain.Events;
using Viasoft.Accounting.Host.Controllers.Inputs;
using Viasoft.Accounting.Host.Providers.Accounts;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Accounting.Host.Services;

public class BookkeepingAccountService : IBookkeepingAccountService, ITransientDependency
{
    private readonly IRepository<BookkeepingAccount> _bookkeepingAccounts;
    private readonly IRepository<BookkeepingAccountView> _bookkeepingAccountsViews;
    private readonly IRepository<Configuracao> _configuracoes;
    private readonly ISalesAccountProvider _salesAccountProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceBus _serviceBus;

    private const int MaxNameColumnLength = 100;
    private const int MaxSummarisedColumnLength = 50;

    public BookkeepingAccountService(
        IRepository<BookkeepingAccount> bookkeepingAccounts,
        IRepository<BookkeepingAccountView> bookkeepingAccountsViews,
        IRepository<Configuracao> configuracoes,
        ISalesAccountProvider salesAccountProvider,
        IUnitOfWork unitOfWork,
        IServiceBus serviceBus)
    {
        _bookkeepingAccounts = bookkeepingAccounts;
        _bookkeepingAccountsViews = bookkeepingAccountsViews;
        _configuracoes = configuracoes;
        _salesAccountProvider = salesAccountProvider;
        _unitOfWork = unitOfWork;
        _serviceBus = serviceBus;
    }

    public async Task Create(BookkeepingAccountInput input)
    {
        var salesAccount = await _salesAccountProvider.Get(input.SalesAccountId);

        var ctaCodeFromParameters = await _configuracoes.AsNoTracking()
            .Select(configuracao => configuracao.CodigoContaReferencia).FirstAsync();

        var parentBookkeepingAccount = await _bookkeepingAccounts.AsNoTracking()
            .FirstAsync(bookkeepingAccount => bookkeepingAccount.Id == input.ParentBookkeepingAccountId);

        var parentBookkeepingAccountView = await _bookkeepingAccountsViews.AsNoTracking()
            .FirstAsync(bookkeepingAccountView => bookkeepingAccountView.Code == parentBookkeepingAccount.Code);

        var currentCode = await _bookkeepingAccounts.AsNoTracking()
            .MaxAsync(bookkeepingAccount => bookkeepingAccount.Code);

        var hasChildren = await _bookkeepingAccounts.AsNoTracking()
            .AnyAsync(bookkeepingAccount => bookkeepingAccount.ParentCode.HasValue && bookkeepingAccount.ParentCode == parentBookkeepingAccount.Code);

        var currentNumber = 0;
        var currentOrder = 0;

        if (hasChildren)
        {
            currentNumber = await _bookkeepingAccounts.AsNoTracking()
                .Where(bookkeepingAccount => bookkeepingAccount.ParentCode.HasValue && bookkeepingAccount.ParentCode == parentBookkeepingAccount.Code)
                .MaxAsync(bookkeepingAccount => bookkeepingAccount.Number);

            currentOrder = await _bookkeepingAccounts.AsNoTracking()
                .Where(bookkeepingAccount => bookkeepingAccount.ParentCode.HasValue && bookkeepingAccount.ParentCode == parentBookkeepingAccount.Code)
                .MaxAsync(bookkeepingAccount => bookkeepingAccount.Order);
        }

        var bookkeepingAccount = new BookkeepingAccount
        {
            Id = Guid.NewGuid(),
            Code = currentCode + 1,
            Classification = $"{parentBookkeepingAccount.Classification}.{currentNumber + 1}",
            IsSynthetic = "N",
            Changed = parentBookkeepingAccount.Changed,
            CenterCostCode = parentBookkeepingAccount.CenterCostCode,
            Number = currentNumber + 1,
            ParentCode = parentBookkeepingAccount.Code,
            Date = parentBookkeepingAccount.Date,
            Model = parentBookkeepingAccount.Model,
            Nature = parentBookkeepingAccount.Nature,
            Level = parentBookkeepingAccount.Level + 1,
            Required = parentBookkeepingAccount.Required,
            Type = parentBookkeepingAccount.Type,
            Order = currentOrder + 1,
            BookkeepingAccountGroupLegacyId = parentBookkeepingAccount.BookkeepingAccountGroupLegacyId,
            FgRevenues = parentBookkeepingAccount.FgRevenues,
            CalculateCosts = parentBookkeepingAccount.CalculateCosts,
            CtaCode = !string.IsNullOrWhiteSpace(ctaCodeFromParameters)
                ? ctaCodeFromParameters
                : parentBookkeepingAccount.CtaCode,
            Name = salesAccount.CompanyName.Length <= MaxNameColumnLength
                ? salesAccount.CompanyName
                : salesAccount.CompanyName[..MaxNameColumnLength],
            Summarised = salesAccount.CompanyName.Length <= MaxSummarisedColumnLength
                ? salesAccount.CompanyName
                : salesAccount.CompanyName[..MaxSummarisedColumnLength]
        };

        var bookkeepingAccountView = new BookkeepingAccountView
        {
            Id = Guid.NewGuid(),
            Code = bookkeepingAccount.Code,
            Classification = bookkeepingAccount.Classification,
            Name = bookkeepingAccount.Name,
            IsSynthetic = bookkeepingAccount.IsSynthetic,
            Changed = bookkeepingAccount.Changed,
            CenterCostCode = bookkeepingAccount.CenterCostCode,
            Number = bookkeepingAccount.Number,
            ParentCode = bookkeepingAccount.ParentCode,
            Date = bookkeepingAccount.Date,
            Model = bookkeepingAccount.Model,
            Nature = bookkeepingAccount.Nature,
            Level = bookkeepingAccount.Level,
            Required = bookkeepingAccount.Required,
            Type = bookkeepingAccount.Type,
            Summarised = bookkeepingAccount.Summarised,
            Order = bookkeepingAccount.Order,
            BookkeepingAccountGroupLegacyId = bookkeepingAccount.BookkeepingAccountGroupLegacyId,
            CtaCode = bookkeepingAccount.CtaCode,
            FgRevenues = bookkeepingAccount.FgRevenues,
            CalculateCosts = bookkeepingAccount.CalculateCosts,
            ParentViewLegacyId = parentBookkeepingAccountView.LegacyId,
            ParentAccountLegacyId = parentBookkeepingAccountView.ParentAccountLegacyId
        };

        var bookkeepingAccountCreated = new BookkeepingAccountCreated(bookkeepingAccount.Id, salesAccount.Id);

        using (_unitOfWork.Begin())
        {
            await _bookkeepingAccounts.InsertAsync(bookkeepingAccount);
            await _bookkeepingAccountsViews.InsertAsync(bookkeepingAccountView);
            await _serviceBus.Publish(bookkeepingAccountCreated);
            await _unitOfWork.CompleteAsync();
        }
    }
}
