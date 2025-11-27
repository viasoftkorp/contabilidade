using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Accounting.Infrastructure.Migrations
{
    public partial class AccountingEntryHistoryAddedEntity : Migration
    {
        
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AccountingEntryHistoryAddedEntity(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountingEntryHistory",
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
                    Code = table.Column<string>(maxLength: 450, nullable: true),
                    Description = table.Column<string>(maxLength: 450, nullable: true),
                    IsStatic = table.Column<bool>(nullable: false, defaultValue: false),
                    Module = table.Column<string>(maxLength: 450, nullable: true),
                    TenantId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingEntryHistory", x => x.Id);
                },
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingEntryHistory",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
