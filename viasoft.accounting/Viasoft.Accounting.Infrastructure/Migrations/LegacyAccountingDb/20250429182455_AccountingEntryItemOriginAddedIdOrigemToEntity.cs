using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Accounting.Infrastructure.Migrations.LegacyAccountingDb
{
    public partial class AccountingEntryItemOriginAddedIdOrigemToEntity : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AccountingEntryItemOriginAddedIdOrigemToEntity(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ID_ORIGEM",
                table: "CT_LANCAMENTOS_DETALHES_ORIGEM");

            migrationBuilder.RenameColumn(
                name: "R_E_C_N_O_",
                table: "CT_LANCAMENTOS",
                newName: "LegacyId");

            migrationBuilder.AlterColumn<string>(
                name: "TIPO_ORIGEM",
                table: "CT_LANCAMENTOS_DETALHES_ORIGEM",
                type: "VARCHAR(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "LegacyId",
                table: "CT_LANCAMENTOS",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
