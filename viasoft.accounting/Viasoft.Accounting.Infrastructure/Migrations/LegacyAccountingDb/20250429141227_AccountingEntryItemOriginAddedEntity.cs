using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Accounting.Infrastructure.Migrations.LegacyAccountingDb
{
    public partial class AccountingEntryItemOriginAddedEntity : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AccountingEntryItemOriginAddedEntity(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CT_LANCAMENTOS_DETALHES_ORIGEM");

            migrationBuilder.DropTable(
                name: "CT_PLANO_CONTAS_VISAO");

            migrationBuilder.DropColumn(
                name: "ALTERADO",
                table: "CT_PLANO_CONTAS");

            migrationBuilder.DropColumn(
                name: "CCUSTO",
                table: "CT_PLANO_CONTAS");

            migrationBuilder.DropColumn(
                name: "CODPAI",
                table: "CT_PLANO_CONTAS");

            migrationBuilder.DropColumn(
                name: "COD_CTA_REF",
                table: "CT_PLANO_CONTAS");

            migrationBuilder.DropColumn(
                name: "CONTABILIZAR_CUSTEIO",
                table: "CT_PLANO_CONTAS");

            migrationBuilder.DropColumn(
                name: "DATA",
                table: "CT_PLANO_CONTAS");

            migrationBuilder.DropColumn(
                name: "FG_RECEITAS",
                table: "CT_PLANO_CONTAS");

            migrationBuilder.DropColumn(
                name: "MODELO",
                table: "CT_PLANO_CONTAS");

            migrationBuilder.DropColumn(
                name: "NATUREZA",
                table: "CT_PLANO_CONTAS");

            migrationBuilder.DropColumn(
                name: "NIVEL",
                table: "CT_PLANO_CONTAS");

            migrationBuilder.DropColumn(
                name: "NUMERO",
                table: "CT_PLANO_CONTAS");

            migrationBuilder.DropColumn(
                name: "OBRIGATORIOCC",
                table: "CT_PLANO_CONTAS");

            migrationBuilder.DropColumn(
                name: "ORDEM",
                table: "CT_PLANO_CONTAS");

            migrationBuilder.DropColumn(
                name: "RECNO_GRUPO_PLANOCONTAS",
                table: "CT_PLANO_CONTAS");

            migrationBuilder.DropColumn(
                name: "RESUMIDO",
                table: "CT_PLANO_CONTAS");

            migrationBuilder.DropColumn(
                name: "TIPO",
                table: "CT_PLANO_CONTAS");

            migrationBuilder.DropColumn(
                name: "CONTA_CONTABIL_PAI_CLIENTE",
                table: "CT_PARAMETROS");

            migrationBuilder.DropColumn(
                name: "CONTA_REFERENCIAL_CLIENTE",
                table: "CT_PARAMETROS");

            migrationBuilder.DropColumn(
                name: "LegacyId",
                table: "CT_LANCAMENTOS");

            migrationBuilder.DropColumn(
                name: "EMPRESA_RECNO",
                table: "CT_FECHAMENTO");

            migrationBuilder.AlterColumn<string>(
                name: "NOME",
                table: "CT_PLANO_CONTAS",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DATA",
                table: "CT_FECHAMENTO",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
