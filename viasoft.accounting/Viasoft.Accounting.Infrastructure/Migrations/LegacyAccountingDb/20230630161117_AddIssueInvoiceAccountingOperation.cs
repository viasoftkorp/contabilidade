using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Accounting.Infrastructure.Migrations.LegacyAccountingDb
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CT_FECHAMENTO");

            migrationBuilder.DropTable(
                name: "CT_OPERACAO_CONTA");

            migrationBuilder.DropTable(
                name: "CT_PARAMETROS");

            migrationBuilder.DropTable(
                name: "CT_VARIAVEL_COMPLE");

            migrationBuilder.DropTable(
                name: "CT_VARIAVEL_IMP");

            migrationBuilder.DropColumn(
                name: "OPE_FATURAMENTO",
                table: "CT_OPERACAO");

            migrationBuilder.DropColumn(
                name: "CODORIGEM",
                table: "CT_LANCAMENTOS");

            migrationBuilder.DropColumn(
                name: "ORIGEM",
                table: "CT_LANCAMENTOS");

            migrationBuilder.DropColumn(
                name: "USUARIO",
                table: "CT_LANCAMENTOS");

            migrationBuilder.DropColumn(
                name: "CLIFOR",
                table: "CT_CABLANCAMENTOS");

            migrationBuilder.DropColumn(
                name: "ENTRADA",
                table: "CT_CABLANCAMENTOS");

            migrationBuilder.DropColumn(
                name: "RECNO_CONTASR",
                table: "CT_CABLANCAMENTOS");

            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "CT_CABLANCAMENTOS");

            migrationBuilder.DropColumn(
                name: "USUARIO",
                table: "CT_CABLANCAMENTOS");

            migrationBuilder.AlterColumn<string>(
                name: "CLI",
                table: "CT_CABLANCAMENTOS",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);
        }
    }
}
