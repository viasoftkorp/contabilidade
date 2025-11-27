using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Accounting.Infrastructure.Migrations
{
    public partial class AccoutingEntryAddedEntity : Migration
    {

        private readonly ISchemaNameProvider _schemaNameProvider;

        public AccoutingEntryAddedEntity(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountingEntry",
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
                    Code = table.Column<int>(nullable: false),
                    EntryType = table.Column<string>(maxLength: 450, nullable: true),
                    EntryDateLegacy = table.Column<string>(maxLength: 8, nullable: true),
                    AccountingYear = table.Column<int>(nullable: true),
                    AccountingMonth = table.Column<int>(nullable: true),
                    CreationTimeLegacy = table.Column<DateTime>(maxLength: 8, nullable: true),
                    Notes = table.Column<string>(maxLength: 450, nullable: true),
                    Series = table.Column<string>(maxLength: 450, nullable: true),
                    Customer = table.Column<string>(maxLength: 450, nullable: true),
                    CompanyCode = table.Column<int>(nullable: false),
                    SourceType = table.Column<int>(nullable: true),
                    SourceId = table.Column<Guid>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingEntry", x => x.Id);
                },
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingEntry",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
