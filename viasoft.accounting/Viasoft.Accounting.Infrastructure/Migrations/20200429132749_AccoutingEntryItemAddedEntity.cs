using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Accounting.Infrastructure.Migrations
{
    public partial class AccoutingEntryItemAddedEntity : Migration
    {
        
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AccoutingEntryItemAddedEntity(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountingEntryItem",
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
                    EntryCode = table.Column<int>(nullable: false),
                    CostCenter = table.Column<string>(maxLength: 450, nullable: true),
                    DebitValue = table.Column<decimal>(nullable: true),
                    CreditValue = table.Column<decimal>(nullable: true),
                    EntryHistoricCode = table.Column<string>(maxLength: 450, nullable: true),
                    Notes = table.Column<string>(maxLength: 450, nullable: true),
                    AccountingOperation = table.Column<string>(maxLength: 450, nullable: true),
                    AccountCode = table.Column<int>(nullable: true),
                    CompanyCode = table.Column<int>(nullable: false),
                    AccountingEntryId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingEntryItem", x => x.Id);
                },
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingEntryItem",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
