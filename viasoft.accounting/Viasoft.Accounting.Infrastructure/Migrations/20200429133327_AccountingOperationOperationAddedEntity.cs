using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Accounting.Infrastructure.Migrations
{
    public partial class AccountingOperationOperationAddedEntity : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AccountingOperationOperationAddedEntity(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountingOperation",
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
                    Cfop = table.Column<string>(maxLength: 450, nullable: true),
                    EvaluatedSocialContribution = table.Column<string>(maxLength: 450, nullable: true),
                    DoesntGenerateUnitaryCost = table.Column<string>(maxLength: 450, nullable: true),
                    ShouldGenerateEntries = table.Column<string>(maxLength: 450, nullable: true),
                    CstIcms = table.Column<string>(maxLength: 450, nullable: true),
                    CstPis = table.Column<string>(maxLength: 450, nullable: true),
                    CstCofins = table.Column<string>(maxLength: 450, nullable: true),
                    CteModule = table.Column<bool>(nullable: false, defaultValue: false),
                    TenantId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingOperation", x => x.Id);
                },
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingOperation",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
