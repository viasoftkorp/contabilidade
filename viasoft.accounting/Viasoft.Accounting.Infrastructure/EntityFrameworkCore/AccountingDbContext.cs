using Viasoft.Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.EntityFrameworkCore.Context;
using Viasoft.Core.Storage.Schema;
using AccountingEntry = Viasoft.Accounting.Domain.Entities.AccountingEntry;
using AccountingEntryItem = Viasoft.Accounting.Domain.Entities.AccountingEntryItem;
using System;
using Microsoft.Extensions.Logging;

namespace Viasoft.Accounting.Infrastructure.EntityFrameworkCore
{
    public class AccountingDbContext : BaseDbContext
    {
        public DbSet<AccountingEntry> AccountingEntries { get; set; }
        public DbSet<AccountingEntryItem> AccountingEntryItems { get; set; }
        public DbSet<BookkeepingAccount> BookkeepingAccounts { get; set; }
        public DbSet<AccountingEntryHistory> AccountingEntryHistories { get; set; }
        public DbSet<AccountingOperationEntryRule> AccountingOperationEntryRules { get; set; }
        public DbSet<AccountingOperation> AccountingOperations { get; set; }
        public DbSet<ManagerialAccount> ManagerialAccounts { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountingEntryHistory>().Property<bool>(p => p.IsStatic).HasDefaultValue(false);
            modelBuilder.Entity<AccountingOperation>().Property<bool>(p => p.CteModule).HasDefaultValue(false);
            modelBuilder.Entity<ManagerialAccount>().Property(e => e.TenantId).HasDefaultValue(Guid.Empty);
            modelBuilder.Entity<AccountingOperation>().Ignore(e => e.UsedInWarehouseRequisitionModule);
            
            base.OnModelCreating(modelBuilder);
        }

        public AccountingDbContext(DbContextOptions options, ISchemaNameProvider schemaNameProvider, ILoggerFactory loggerFactory, IBaseDbContextConfigurationService baseDbContextConfigurationService) : base(options, schemaNameProvider, loggerFactory, baseDbContextConfigurationService)
        {
        }
    }
}