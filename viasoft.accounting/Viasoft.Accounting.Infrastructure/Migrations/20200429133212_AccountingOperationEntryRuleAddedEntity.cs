using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Accounting.Infrastructure.Migrations
{
    public partial class AccountingOperationEntryRuleAddedEntity : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AccountingOperationEntryRuleAddedEntity(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountingOperationEntryRule",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AccountingOperationId = table.Column<Guid>(nullable: false),
                    AccountingEntryType = table.Column<int>(nullable: false),
                    BookkeepingAccountId = table.Column<Guid>(nullable: false),
                    EntryVariable = table.Column<string>(maxLength: 450, nullable: false),
                    HistoricId = table.Column<Guid>(nullable: false),
                    FirstDisplayInfo = table.Column<int>(nullable: true),
                    SecondDisplayInfo = table.Column<int>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingOperationEntryRule", x => x.Id);
                },
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingOperationEntryRule",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
