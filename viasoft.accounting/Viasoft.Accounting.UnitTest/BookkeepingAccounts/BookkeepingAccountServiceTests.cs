// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using FluentAssertions;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using NSubstitute;
// using Rebus.TestHelpers.Events;
// using Viasoft.Accounting.Domain.Entities;
// using Viasoft.Accounting.Domain.Events;
// using Viasoft.Accounting.Host.Controllers.Inputs;
// using Viasoft.Accounting.Host.Providers.Accounts;
// using Viasoft.Accounting.Host.Providers.Accounts.Dtos;
// using Viasoft.Accounting.Host.Services;
// using Viasoft.Core.DDD.Repositories;
// using Viasoft.Core.DDD.UnitOfWork;
// using Viasoft.Core.EntityFrameworkCore.Extensions;
// using Viasoft.Core.ServiceBus.InMemory.Bus;
// using Xunit;
//
// namespace Viasoft.Accounting.UnitTest.BookkeepingAccounts;
//
// public class BookkeepingAccountServiceTests : UnitTestBaseWithDbContext
// {
//     [Fact(DisplayName = "Criar plano de conta contábil")]
//     public async Task Create()
//     {
//         // Arrange
//         var mock = GetMock();
//         var service = GetService(mock);
//
//         var input = new BookkeepingAccountInput
//         {
//             SalesAccountId = TestUtils.Guids[0],
//             ParentBookkeepingAccountId = TestUtils.Guids[1]
//         };
//
//         var configuracao = new Configuracao
//         {
//             Id = TestUtils.Guids[0],
//             CodigoOperacaoContabilAdiantamento = TestUtils.CodeString[0],
//             CodigoContaContabilPai = TestUtils.CodeInt[0],
//             CodigoContaReferencia = TestUtils.CodeString[1]
//         };
//
//         var parentBookkeepingAccount = new BookkeepingAccount
//         {
//             Id = TestUtils.Guids[1],
//             Code = TestUtils.CodeInt[0],
//             Classification = TestUtils.CodeString[0],
//             Name = TestUtils.Names[0],
//             IsSynthetic = "S",
//             Changed = TestUtils.CodeString[1],
//             CenterCostCode = TestUtils.CodeString[2],
//             Number = TestUtils.CodeInt[1],
//             ParentCode = TestUtils.CodeInt[2],
//             Date = TestUtils.Dates[0],
//             Model = TestUtils.CodeInt[3],
//             Nature = TestUtils.CodeInt[4],
//             Level = TestUtils.CodeInt[5],
//             Required = true,
//             Type = TestUtils.CodeString[3],
//             Summarised = TestUtils.Names[1],
//             Order = TestUtils.CodeInt[6],
//             BookkeepingAccountGroupLegacyId = TestUtils.CodeInt[7],
//             CtaCode = TestUtils.CodeString[4],
//             FgRevenues = true,
//             CalculateCosts = true
//         };
//
//         var parentBookkeepingAccountView = new BookkeepingAccountView
//         {
//             Id = TestUtils.Guids[2],
//             LegacyId = TestUtils.CodeInt[0],
//             ParentAccountLegacyId = TestUtils.CodeInt[1],
//             Code = TestUtils.CodeInt[0]
//         };
//
//         var childrenBookkeepingAccounts = new List<BookkeepingAccount>
//         {
//             new()
//             {
//                 Id = TestUtils.Guids[3],
//                 Code = TestUtils.CodeInt[1],
//                 Number = TestUtils.CodeInt[2],
//                 Order = TestUtils.CodeInt[7],
//                 ParentCode = TestUtils.CodeInt[0]
//             },
//             new()
//             {
//                 Id = TestUtils.Guids[4],
//                 Code = TestUtils.CodeInt[2],
//                 Number = TestUtils.CodeInt[3],
//                 Order = TestUtils.CodeInt[8],
//                 ParentCode = TestUtils.CodeInt[0]
//             }
//         };
//
//         var getSalesAccountOutput = new SalesAccountOutput
//         {
//             Id = TestUtils.Guids[0],
//             CompanyName = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
//         };
//
//         await mock.Configuracoes.InsertAsync(configuracao);
//         await mock.BookkeepingAccounts.InsertAsync(parentBookkeepingAccount);
//         await mock.BookkeepingAccounts.InsertRangeAsync(childrenBookkeepingAccounts);
//         await mock.BookkeepingAccountsViews.InsertAsync(parentBookkeepingAccountView);
//         await mock.UnitOfWork.SaveChangesAsync();
//
//         mock.BookkeepingAccounts.GetUnderlyingDbContext().ChangeTracker.Clear();
//         mock.BookkeepingAccountsViews.GetUnderlyingDbContext().ChangeTracker.Clear();
//
//         mock.SalesAccountProvider.Get(input.SalesAccountId)
//             .Returns(getSalesAccountOutput);
//
//         // Act
//         await service.Create(input);
//
//         // Assert
//         mock.BookkeepingAccounts.GetUnderlyingDbContext().ChangeTracker.Clear();
//         mock.BookkeepingAccountsViews.GetUnderlyingDbContext().ChangeTracker.Clear();
//
//         var createdBookkeepingAccount = await mock.BookkeepingAccounts
//             .FirstAsync(bookkeepingAccount => bookkeepingAccount.Code == 4);
//
//         var createdBookkeepingAccountView = await mock.BookkeepingAccountsViews
//             .FirstAsync(bookkeepingAccountView => bookkeepingAccountView.Code == 4);
//
//         createdBookkeepingAccount.Id.Should().NotBeEmpty();
//         createdBookkeepingAccount.Code.Should().Be(4);
//         createdBookkeepingAccount.Classification.Should().Be("0001.5");
//         createdBookkeepingAccount.IsSynthetic.Should().Be("N");
//         createdBookkeepingAccount.Changed.Should().Be(TestUtils.CodeString[1]);
//         createdBookkeepingAccount.CenterCostCode.Should().Be(TestUtils.CodeString[2]);
//         createdBookkeepingAccount.Number.Should().Be(5);
//         createdBookkeepingAccount.ParentCode.Should().Be(TestUtils.CodeInt[0]);
//         createdBookkeepingAccount.Date.Should().Be(TestUtils.Dates[0]);
//         createdBookkeepingAccount.Model.Should().Be(TestUtils.CodeInt[3]);
//         createdBookkeepingAccount.Nature.Should().Be(TestUtils.CodeInt[4]);
//         createdBookkeepingAccount.Level.Should().Be(7);
//         createdBookkeepingAccount.Required.Should().BeTrue();
//         createdBookkeepingAccount.Type.Should().Be(TestUtils.CodeString[3]);
//         createdBookkeepingAccount.Order.Should().Be(10);
//         createdBookkeepingAccount.BookkeepingAccountGroupLegacyId.Should().Be(TestUtils.CodeInt[7]);
//         createdBookkeepingAccount.FgRevenues.Should().BeTrue();
//         createdBookkeepingAccount.CalculateCosts.Should().BeTrue();
//         createdBookkeepingAccount.CtaCode.Should().Be(TestUtils.CodeString[1]);
//         createdBookkeepingAccount.Name.Should().Be("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
//         createdBookkeepingAccount.Summarised.Should().Be("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
//
//         createdBookkeepingAccountView.Id.Should().NotBeEmpty();
//         createdBookkeepingAccountView.Code.Should().Be(4);
//         createdBookkeepingAccountView.Classification.Should().Be("0001.5");
//         createdBookkeepingAccountView.IsSynthetic.Should().Be("N");
//         createdBookkeepingAccountView.Changed.Should().Be(TestUtils.CodeString[1]);
//         createdBookkeepingAccountView.CenterCostCode.Should().Be(TestUtils.CodeString[2]);
//         createdBookkeepingAccountView.Number.Should().Be(5);
//         createdBookkeepingAccountView.ParentCode.Should().Be(TestUtils.CodeInt[0]);
//         createdBookkeepingAccountView.Date.Should().Be(TestUtils.Dates[0]);
//         createdBookkeepingAccountView.Model.Should().Be(TestUtils.CodeInt[3]);
//         createdBookkeepingAccountView.Nature.Should().Be(TestUtils.CodeInt[4]);
//         createdBookkeepingAccountView.Level.Should().Be(7);
//         createdBookkeepingAccountView.Required.Should().BeTrue();
//         createdBookkeepingAccountView.Type.Should().Be(TestUtils.CodeString[3]);
//         createdBookkeepingAccountView.Order.Should().Be(10);
//         createdBookkeepingAccountView.BookkeepingAccountGroupLegacyId.Should().Be(TestUtils.CodeInt[7]);
//         createdBookkeepingAccountView.FgRevenues.Should().BeTrue();
//         createdBookkeepingAccountView.CalculateCosts.Should().BeTrue();
//         createdBookkeepingAccountView.CtaCode.Should().Be(TestUtils.CodeString[1]);
//         createdBookkeepingAccountView.Name.Should().Be("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
//         createdBookkeepingAccountView.Summarised.Should().Be("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
//         createdBookkeepingAccountView.ParentViewLegacyId.Should().Be(TestUtils.CodeInt[0]);
//         createdBookkeepingAccountView.ParentAccountLegacyId.Should().Be(TestUtils.CodeInt[1]);
//
//         var publishedBookkeepingAccountCreatedEvents = mock.ServiceBus.FakeBus.Events
//             .OfType<MessagePublished<BookkeepingAccountCreated>>()
//             .ToList();
//
//         publishedBookkeepingAccountCreatedEvents.Should().HaveCount(1);
//         publishedBookkeepingAccountCreatedEvents.First().EventMessage.SalesAccountId.Should().Be(TestUtils.Guids[0]);
//         publishedBookkeepingAccountCreatedEvents.First().EventMessage.BookkeepingAccountId.Should().Be(createdBookkeepingAccount.Id);
//     }
//
//     public BookkeepingAccountServiceTests()
//     {
//         SetLegacyModeEnabled(true);
//     }
//
//     private static BookkeepingAccountService GetService(BookkeepingAccountServiceMock mock)
//     {
//         return new BookkeepingAccountService(mock.BookkeepingAccounts, mock.BookkeepingAccountsViews,
//             mock.Configuracoes, mock.SalesAccountProvider, mock.UnitOfWork, mock.ServiceBus);
//     }
//
//     private BookkeepingAccountServiceMock GetMock()
//     {
//         return new BookkeepingAccountServiceMock
//         {
//             BookkeepingAccounts = ServiceProvider.GetService<IRepository<BookkeepingAccount>>(),
//             BookkeepingAccountsViews = ServiceProvider.GetService<IRepository<BookkeepingAccountView>>(),
//             Configuracoes = ServiceProvider.GetService<IRepository<Configuracao>>(),
//             SalesAccountProvider = Substitute.For<ISalesAccountProvider>(),
//             UnitOfWork = UnitOfWork,
//             ServiceBus = ServiceBus
//         };
//     }
//
//     private class BookkeepingAccountServiceMock
//     {
//         public IRepository<BookkeepingAccount> BookkeepingAccounts { get; set; }
//         public IRepository<BookkeepingAccountView> BookkeepingAccountsViews { get; set; }
//         public IRepository<Configuracao> Configuracoes { get; set; }
//         public ISalesAccountProvider SalesAccountProvider { get; set; }
//         public IUnitOfWork UnitOfWork { get; set; }
//         public InMemoryServiceBus ServiceBus { get; set; }
//     }
// }
