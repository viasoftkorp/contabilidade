using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Accounting.Infrastructure.Migrations
{
    public partial class AddIssueInvoiceAccountingOperation : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddIssueInvoiceAccountingOperation(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IssueInvoice",
                table: "AccountingOperation",
                type: "bit",
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<int>(
                name: "LegacyIdOrigem",
                table: "AccountingEntryItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<string>(
                name: "Origem",
                table: "AccountingEntryItem",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<string>(
                name: "Usuario",
                table: "AccountingEntryItem",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<string>(
                name: "Entrada",
                table: "AccountingEntry",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<string>(
                name: "Fornecedor",
                table: "AccountingEntry",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<int>(
                name: "LegacyIdContaReceber",
                table: "AccountingEntry",
                type: "int",
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AccountingEntry",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<string>(
                name: "Usuario",
                table: "AccountingEntry",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssueInvoice",
                table: "AccountingOperation");

            migrationBuilder.DropColumn(
                name: "LegacyIdOrigem",
                table: "AccountingEntryItem");

            migrationBuilder.DropColumn(
                name: "Origem",
                table: "AccountingEntryItem");

            migrationBuilder.DropColumn(
                name: "Usuario",
                table: "AccountingEntryItem");

            migrationBuilder.DropColumn(
                name: "Entrada",
                table: "AccountingEntry");

            migrationBuilder.DropColumn(
                name: "Fornecedor",
                table: "AccountingEntry");

            migrationBuilder.DropColumn(
                name: "LegacyIdContaReceber",
                table: "AccountingEntry");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AccountingEntry");

            migrationBuilder.DropColumn(
                name: "Usuario",
                table: "AccountingEntry");
        }
    }
}
