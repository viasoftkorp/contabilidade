using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.EntityFrameworkCore.SQLServer.Legacy.Extensions;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Accounting.Infrastructure.Migrations.LegacyAccountingDb
{
    public partial class InitialMigration : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;
        private readonly DbContext _dbContext;

        public InitialMigration(ISchemaNameProvider schemaNameProvider, DbContext dbContext)
        {
            _schemaNameProvider = schemaNameProvider;
            _dbContext = dbContext;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountingOperationEntryRule",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
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

            migrationBuilder.CreateTable(
                name: "CT_CABLANCAMENTOS",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NLANC = table.Column<int>(maxLength: 4, nullable: false),
                    TIPOLANC = table.Column<string>(maxLength: 5, nullable: true),
                    DTLANC = table.Column<string>(maxLength: 8, nullable: true),
                    ANOCONT = table.Column<int>(maxLength: 4, nullable: true),
                    MESCONT = table.Column<int>(maxLength: 4, nullable: true),
                    DATAHORA = table.Column<DateTime>(maxLength: 8, nullable: true),
                    NOTA = table.Column<string>(maxLength: 50, nullable: true),
                    SERIE = table.Column<string>(maxLength: 5, nullable: true),
                    CLI = table.Column<string>(maxLength: 10, nullable: true),
                    EMPRESA_RECNO = table.Column<int>(maxLength: 4, nullable: false),
                    SourceType = table.Column<int>(nullable: true),
                    SourceId = table.Column<Guid>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_CABLANCAMENTOS", x => x.Id);
                },
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "CT_HISTPADRAO",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CODHISTORICO = table.Column<string>(maxLength: 5, nullable: true),
                    DESCRICAO = table.Column<string>(maxLength: 100, nullable: true),
                    IsStatic = table.Column<bool>(nullable: false, defaultValue: false),
                    Module = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_HISTPADRAO", x => x.Id);
                },
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "CT_LANCAMENTOS",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NLANC = table.Column<int>(maxLength: 4, nullable: false),
                    CCUSTO = table.Column<string>(maxLength: 20, nullable: true),
                    VALORDEBITO = table.Column<decimal>(nullable: true),
                    VALORCREDITO = table.Column<decimal>(nullable: true),
                    CODHISTORICO = table.Column<string>(maxLength: 450, nullable: true),
                    OBS = table.Column<string>(maxLength: 250, nullable: true),
                    CT_OPERACAO = table.Column<string>(maxLength: 10, nullable: true),
                    COD_CONTA = table.Column<int>(maxLength: 4, nullable: true),
                    EMPRESA_RECNO = table.Column<int>(maxLength: 4, nullable: false),
                    AccountingEntryId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_LANCAMENTOS", x => x.Id);
                },
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "CT_OPERACAO",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CODIGO = table.Column<string>(maxLength: 10, nullable: true),
                    DESCRICAO = table.Column<string>(maxLength: 100, nullable: true),
                    CFOP = table.Column<string>(maxLength: 10, nullable: true),
                    EFDPC_COD_CONTRIBU_SOCIAL = table.Column<string>(maxLength: 5, nullable: true),
                    NAO_GERA_CUSTO_UNITARIO = table.Column<string>(maxLength: 1, nullable: true),
                    GERACONTAS = table.Column<string>(maxLength: 1, nullable: true),
                    CST_ICMS = table.Column<string>(maxLength: 5, nullable: true),
                    CST_PIS = table.Column<string>(maxLength: 5, nullable: true),
                    CST_COFINS = table.Column<string>(maxLength: 5, nullable: true),
                    CteModule = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_OPERACAO", x => x.Id);
                },
                schema: _schemaNameProvider.GetSchemaName());

      
            migrationBuilder.HandleUpLegacyTables(_dbContext, new List<string>
            {
                "CT_CABLANCAMENTOS",
                "CT_LANCAMENTOS",
                "CT_HISTPADRAO",
                "CT_OPERACAO",
                "AccountingOperationEntryRule"

            }, _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingOperationEntryRule");

            migrationBuilder.DropTable(
                name: "CT_CABLANCAMENTOS");

            migrationBuilder.DropTable(
                name: "CT_HISTPADRAO");

            migrationBuilder.DropTable(
                name: "CT_LANCAMENTOS");

            migrationBuilder.DropTable(
                name: "CT_OPERACAO");

            migrationBuilder.DropTable(
                name: "CT_PLANO_CONTAS");

            migrationBuilder.DropTable(
                name: "PLANO");
        }
    }
}
