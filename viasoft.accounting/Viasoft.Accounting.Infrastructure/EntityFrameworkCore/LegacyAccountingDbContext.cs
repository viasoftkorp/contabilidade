using System.Linq;
using Korp.EntidadesLegadas.ACL.Core.LegacyValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Accounting.Domain.Utils;
using Viasoft.Core.DDD.Entities;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.EntityFrameworkCore.Legacy;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Accounting.Infrastructure.EntityFrameworkCore
{
    public class LegacyAccountingDbContext : LegacyBaseDbContext
    {
        public LegacyAccountingDbContext(DbContextOptions options, ISchemaNameProvider schemaNameProvider,
            ITenantProperties tenantProperties) : base(options, schemaNameProvider)
        {
            _tenantProperties = tenantProperties;
        }

        public DbSet<AccountingEntry> AccountingEntries { get; set; }
        public DbSet<AccountingEntryItem> AccountingEntryItems { get; set; }
        public DbSet<AccountingEntryItemOrigin> AccountingEntryItemOrigins { get; set; }
        public DbSet<BookkeepingAccount> BookkeepingAccounts { get; set; }
        public DbSet<AccountingEntryHistory> AccountingEntryHistories { get; set; }
        public DbSet<AccountingOperationEntryRule> AccountingOperationEntryRules { get; set; }
        public DbSet<AccountingOperation> AccountingOperations { get; set; }
        public DbSet<ManagerialAccount> ManagerialAccounts { get; set; }
        public DbSet<Configuracao> Configuracoes { get; set; }
        public DbSet<LegacyComplemento> LegacyComplementos { get; set; }
        public DbSet<LegacyTemplateLancamentoContabil> LegacyTemplateLancamentoContabils { get; set; }
        public DbSet<LegacyRegraValorLancamento> LegacyRegraValorLancamento { get; set; }
        public DbSet<LegacyFechamentoPeriodoContabil> LegacyFechamentoPeriodoContabils { get; set; }
        public DbSet<BookkeepingAccountView> BookkeepingAccountViews { get; set; }

        private readonly ITenantProperties _tenantProperties;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            LegacyModelAccountEntry(modelBuilder);
            LegacyModelAccountEntryItem(modelBuilder);
            LegacyModelAccountEntryItemOrigin(modelBuilder);
            LegacyModelBookkeepingAccount(modelBuilder);
            LegacyModelAccountingEntryHistory(modelBuilder);
            LegacyModelAccountingOperation(modelBuilder);
            LegacyModelManagerialAccount(modelBuilder);
            Configuracao(modelBuilder);
            TemplateLancamentoContabil(modelBuilder);
            LegacyComplemento(modelBuilder);
            RegraValorLancamento(modelBuilder);
            LegacyFechamentoPeriodoContabil(modelBuilder);
            LegacyModelBookkeepingAccountView(modelBuilder);
            modelBuilder.Entity<AccountingEntryHistory>().Property<bool>(p => p.IsStatic).HasDefaultValue(false);
            modelBuilder.Entity<AccountingOperation>().Property<bool>(p => p.CteModule).HasDefaultValue(false);
            RemoveSdkProperties(modelBuilder);
        }
        private void Configuracao(ModelBuilder modelBuilder)
        {
            var entityTypeBuilder = modelBuilder.Entity<Configuracao>();
            entityTypeBuilder
                .ToTable("CT_PARAMETROS");
            entityTypeBuilder
                .Property(entity => entity.CodigoOperacaoContabilAdiantamento)
                .HasColumnName("OPE_SINAL_VENDA");
            entityTypeBuilder
                .Property(entity => entity.CodigoContaContabilPai)
                .HasColumnName("CONTA_CONTABIL_PAI_CLIENTE");
            entityTypeBuilder
                .Property(entity => entity.CodigoContaReferencia)
                .HasColumnName("CONTA_REFERENCIAL_CLIENTE");
        }

        private void LegacyModelAccountEntryItem(ModelBuilder modelBuilder)
        {
            var entityTypeBuilder = modelBuilder.Entity<AccountingEntryItem>();
            
            entityTypeBuilder.HasKey(e => e.LegacyId);
            
            entityTypeBuilder
                .Property(entity => entity.LegacyId)
                .HasColumnName("R_E_C_N_O_")
                .ValueGeneratedOnAdd();
            
            entityTypeBuilder
                .ToTable("CT_LANCAMENTOS");
            entityTypeBuilder
                .Property(entity => entity.EntryCode)
                .HasColumnName("NLANC")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.CostCenter)
                .HasColumnName("CCUSTO")
                .HasMaxLength(20);
            entityTypeBuilder
                .Property(entity => entity.DebitValue)
                .HasColumnName("VALORDEBITO");
            entityTypeBuilder
                .Property(entity => entity.CreditValue)
                .HasColumnName("VALORCREDITO");
            entityTypeBuilder
                .Property(entity => entity.EntryHistoricCode)
                .HasColumnName("CODHISTORICO");
            entityTypeBuilder
                .Property(entity => entity.Notes)
                .HasMaxLength(250)
                .HasColumnName("OBS");
            entityTypeBuilder
                .Property(entity => entity.AccountingOperation)
                .HasMaxLength(10)
                .HasColumnName("CT_OPERACAO");
            entityTypeBuilder
                .Property(entity => entity.AccountCode)
                .HasMaxLength(4)
                .HasColumnName("COD_CONTA");
            entityTypeBuilder
                .Property(entity => entity.CompanyCode)
                .HasMaxLength(4)
                .HasColumnName("EMPRESA_RECNO");
            entityTypeBuilder
                .Property(entity => entity.Usuario)
                .HasColumnName("USUARIO");

            entityTypeBuilder
                .Property(entity => entity.Origem)
                .HasColumnName("ORIGEM");
            entityTypeBuilder
                .Property(entity => entity.LegacyIdOrigem)
                .HasColumnName("CODORIGEM");
            
        }

        private void LegacyModelAccountEntryItemOrigin(ModelBuilder modelBuilder)
        {
            var entityTypeBuilder = modelBuilder.Entity<AccountingEntryItemOrigin>();
            entityTypeBuilder
                .ToTable("CT_LANCAMENTOS_DETALHES_ORIGEM");
            
            entityTypeBuilder
                .Property(entity => entity.LegacyId)
                .HasColumnName("R_E_C_N_O_")
                .ValueGeneratedOnAdd();
            
            entityTypeBuilder
                .Property(entity => entity.AccountingEntryItemLegacyId)
                .HasColumnName("RECNO_CT_LANCAMENTOS")
                .IsRequired();

            entityTypeBuilder.HasOne(e => e.AccountingEntryItem)
                .WithMany() 
                .HasForeignKey(e => e.AccountingEntryItemLegacyId)
                .HasPrincipalKey(e => e.LegacyId);
            
            entityTypeBuilder
                .Property(entity => entity.LegacyIdOrigem)
                .HasColumnName("RECNO_ORIGEM")
                .IsRequired();
            
            entityTypeBuilder
                .Property(entity => entity.IdOrigem)
                .HasColumnName("ID_ORIGEM")
                .IsRequired(false);
            
            entityTypeBuilder
                .Property(entity => entity.OriginType)
                .HasConversion(originType => AccountingEntryItemOriginTypeConversions.GetAccountingEntryItemOriginTypeString(originType),
                               originTypeName => AccountingEntryItemOriginTypeConversions.GetAccountingEntryItemOriginTypeEnum(originTypeName))
                .HasColumnType("VARCHAR(200)")
                .HasColumnName("TIPO_ORIGEM")
                .HasMaxLength(200)
                .IsRequired();
            
            entityTypeBuilder
                .Property(entity => entity.CreationDate)
                .HasColumnType("DATETIME")
                .HasColumnName("DATAHORA")
                .IsRequired();
            
            entityTypeBuilder
                .Property(entity => entity.CreditValue)
                .HasColumnType("decimal(19,2)")
                .HasColumnName("VALOR_CREDITO")
                .HasPrecision(19, 2)
                .IsRequired();
            
            entityTypeBuilder
                .Property(entity => entity.DebitValue)
                .HasColumnType("decimal(19,2)")
                .HasColumnName("VALOR_DEBITO")
                .HasPrecision(19, 2)
                .IsRequired();
        }

        private void LegacyModelAccountEntry(ModelBuilder modelBuilder)
        {
            var entityTypeBuilder = modelBuilder.Entity<AccountingEntry>();
            entityTypeBuilder
                .ToTable("CT_CABLANCAMENTOS");
            entityTypeBuilder
                .Property(entity => entity.Code)
                .HasColumnName("NLANC")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.EntryType)
                .HasColumnName("TIPOLANC")
                .HasMaxLength(5);
            entityTypeBuilder
                .Property(entity => entity.EntryDateLegacy)
                .HasColumnName("DTLANC")
                .HasMaxLength(8);
            entityTypeBuilder
                .Property(entity => entity.AccountingYear)
                .HasColumnName("ANOCONT")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.AccountingMonth)
                .HasColumnName("MESCONT")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.CreationTimeLegacy)
                .HasColumnName("DATAHORA")
                .HasMaxLength(8);
            entityTypeBuilder
                .Property(entity => entity.Notes)
                .HasColumnName("NOTA")
                .HasMaxLength(50);
            entityTypeBuilder
                .Property(entity => entity.Series)
                .HasColumnName("SERIE")
                .HasMaxLength(5);
            entityTypeBuilder
                .Property(entity => entity.Customer)
                .HasColumnName("CLI");
            entityTypeBuilder
                .Property(entity => entity.Fornecedor)
                .HasColumnName("CLIFOR");
            entityTypeBuilder
                .Property(entity => entity.CompanyCode)
                .HasColumnName("EMPRESA_RECNO")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.Status)
                .HasColumnName("STATUS");
            entityTypeBuilder
                .Property(entity => entity.Usuario)
                .HasColumnName("USUARIO");
            entityTypeBuilder
                .Property(entity => entity.Entrada)
                .HasColumnName("ENTRADA");
            entityTypeBuilder
                .Property(entity => entity.LegacyIdContaReceber)
                .HasColumnName("RECNO_CONTASR");
        }

        private void LegacyModelManagerialAccount(ModelBuilder modelBuilder)
        {
            var entityTypeBuilder = modelBuilder.Entity<ManagerialAccount>();
            entityTypeBuilder
                .ToTable("PLANO");
            entityTypeBuilder
                .Property(entity => entity.Code)
                .HasColumnName("PLANO")
                .HasMaxLength(20);
            entityTypeBuilder
                .Property(entity => entity.Description)
                .HasColumnName("DESC_PLA")
                .HasMaxLength(200);
        }

        private void LegacyModelBookkeepingAccount(ModelBuilder modelBuilder)
        {
            var entityTypeBuilder = modelBuilder.Entity<BookkeepingAccount>();
            entityTypeBuilder
                .ToTable("CT_PLANO_CONTAS");
            entityTypeBuilder
                .Property(entity => entity.Code)
                .HasColumnName("CODIGO")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.Classification)
                .HasColumnName("CLASSIFICACAO")
                .HasMaxLength(50);
            entityTypeBuilder
                .Property(entity => entity.Name)
                .HasColumnName("NOME")
                .HasMaxLength(100);
            entityTypeBuilder
                .Property(entity => entity.IsSynthetic)
                .HasColumnName("SINTETICO")
                .HasMaxLength(1);
            entityTypeBuilder
                .Property(entity => entity.Changed)
                .HasColumnName("ALTERADO")
                .HasMaxLength(15);
            entityTypeBuilder
                .Property(entity => entity.CenterCostCode)
                .HasColumnName("CCUSTO")
                .HasMaxLength(20);
            entityTypeBuilder
                .Property(entity => entity.Number)
                .HasColumnName("NUMERO")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.ParentCode)
                .HasColumnName("CODPAI")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.Date)
                .HasColumnName("DATA")
                .HasConversion(new LegacyDateStringToNullableAppliedTimezoneDateTime(_tenantProperties))
                .HasMaxLength(8);
            entityTypeBuilder
                .Property(entity => entity.Model)
                .HasColumnName("MODELO")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.Nature)
                .HasColumnName("NATUREZA")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.Level)
                .HasColumnName("NIVEL")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.Required)
                .HasColumnName("OBRIGATORIOCC")
                .HasConversion(new BoolToStringConverter("N", "S"))
                .HasMaxLength(1);
            entityTypeBuilder
                .Property(entity => entity.Type)
                .HasColumnName("TIPO")
                .HasMaxLength(1);
            entityTypeBuilder
                .Property(entity => entity.Summarised)
                .HasColumnName("RESUMIDO")
                .HasMaxLength(50);
            entityTypeBuilder
                .Property(entity => entity.Order)
                .HasColumnName("ORDEM")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.BookkeepingAccountGroupLegacyId)
                .HasColumnName("RECNO_GRUPO_PLANOCONTAS")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.CtaCode)
                .HasColumnName("COD_CTA_REF")
                .HasMaxLength(50);
            entityTypeBuilder
                .Property(entity => entity.FgRevenues)
                .HasColumnName("FG_RECEITAS")
                .HasConversion(new BoolToStringConverter("N", "S"))
                .HasMaxLength(1);
            entityTypeBuilder
                .Property(entity => entity.CalculateCosts)
                .HasColumnName("CONTABILIZAR_CUSTEIO")
                .HasConversion(new BoolToStringConverter("N", "S"))
                .HasMaxLength(1);
        }

        private void LegacyModelAccountingOperation(ModelBuilder modelBuilder)
        {
            var entityTypeBuilder = modelBuilder.Entity<AccountingOperation>();
            entityTypeBuilder
                .ToTable("CT_OPERACAO");
            entityTypeBuilder
                .Property(entity => entity.Code)
                .HasColumnName("CODIGO")
                .HasMaxLength(10);
            entityTypeBuilder
                .Property(entity => entity.Description)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(100);
            entityTypeBuilder
                .Property(entity => entity.Cfop)
                .HasColumnName("CFOP")
                .HasMaxLength(10);
            entityTypeBuilder
                .Property(entity => entity.EvaluatedSocialContribution)
                .HasColumnName("EFDPC_COD_CONTRIBU_SOCIAL")
                .HasMaxLength(5);
            entityTypeBuilder
                .Property(entity => entity.DoesntGenerateUnitaryCost)
                .HasColumnName("NAO_GERA_CUSTO_UNITARIO")
                .HasMaxLength(1);
            entityTypeBuilder
                .Property(entity => entity.ShouldGenerateEntries)
                .HasColumnName("GERACONTAS")
                .HasMaxLength(1);
            entityTypeBuilder
                .Property(entity => entity.CstIcms)
                .HasColumnName("CST_ICMS")
                .HasMaxLength(5);
            entityTypeBuilder
                .Property(entity => entity.CstPis)
                .HasColumnName("CST_PIS")
                .HasMaxLength(5);
            entityTypeBuilder
                .Property(entity => entity.CstCofins)
                .HasColumnName("CST_COFINS")
                .HasMaxLength(5);
            entityTypeBuilder
                .Property(entity => entity.IssueInvoice)
                .HasColumnName("OPE_FATURAMENTO")
                .HasConversion(new BoolToStringConverter("N", "S"))
                .HasMaxLength(1);
            entityTypeBuilder
                .Property(entity => entity.UsedInWarehouseRequisitionModule)
                .HasColumnName("REQUISICAO_ALMOX")
                .HasConversion(new BoolToStringConverter("N", "S"))
                .HasMaxLength(1);
        }

        private void LegacyModelAccountingEntryHistory(ModelBuilder modelBuilder)
        {
            var entityTypeBuilder = modelBuilder.Entity<AccountingEntryHistory>();
            entityTypeBuilder
                .ToTable("CT_HISTPADRAO");
            entityTypeBuilder
                .Property(entity => entity.Code)
                .HasColumnName("CODHISTORICO")
                .HasMaxLength(5);
            entityTypeBuilder
                .Property(entity => entity.Description)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(100);

        }
        private void LegacyComplemento(ModelBuilder modelBuilder)
        {
            var entityTypeBuilder = modelBuilder.Entity<LegacyComplemento>();
            entityTypeBuilder
                .ToTable("CT_VARIAVEL_COMPLE");
            entityTypeBuilder
                .Property(entity => entity.Codigo)
                .HasColumnName("CODIGO");
            entityTypeBuilder
                .Property(entity => entity.Tabela)
                .HasColumnName("TABELA");
            entityTypeBuilder
                .Property(entity => entity.Campo)
                .HasColumnName("CAMPO");

        }
        private void RegraValorLancamento(ModelBuilder modelBuilder)
        {
            var entityTypeBuilder = modelBuilder.Entity<LegacyRegraValorLancamento>();
            entityTypeBuilder
                .ToTable("CT_VARIAVEL_IMP");
            entityTypeBuilder
                .Property(entity => entity.Codigo)
                .HasColumnName("CODIGO");
            entityTypeBuilder
                .Property(entity => entity.Campo)
                .HasColumnName("CAMPO");

        }
        private void LegacyFechamentoPeriodoContabil(ModelBuilder modelBuilder)
        {
            var entityTypeBuilder = modelBuilder.Entity<LegacyFechamentoPeriodoContabil>();
            entityTypeBuilder
                .ToTable("CT_FECHAMENTO");
            entityTypeBuilder
                .Property(entity => entity.Data)
                .HasColumnName("DATA")
                .HasConversion(new LegacyDateStringToNullableAppliedTimezoneDateTime(_tenantProperties));
            entityTypeBuilder
                .Property(entity => entity.LegacyIdEmpresa)
                .HasColumnName("EMPRESA_RECNO")
                .HasMaxLength(4);
        }
        private void TemplateLancamentoContabil(ModelBuilder modelBuilder)
        {
            var entityTypeBuilder = modelBuilder.Entity<LegacyTemplateLancamentoContabil>();
            entityTypeBuilder
                .ToTable("CT_OPERACAO_CONTA");
            entityTypeBuilder
                .Property(entity => entity.CodigoOperacaoContabil)
                .HasColumnName("CODIGO");
            entityTypeBuilder
                .Property(entity => entity.TipoLancamento)
                .HasColumnName("TIPO");
            entityTypeBuilder
                .Property(entity => entity.CodigoContaContabil)
                .HasColumnName("COD_CONTA");
            entityTypeBuilder
                .Property(entity => entity.CodigoHistorico)
                .HasColumnName("CODHISTORICO");
            entityTypeBuilder
                .Property(entity => entity.CodigoCampo)
                .HasColumnName("COD_CAMPO");
            entityTypeBuilder
                .Property(entity => entity.CodigoComplemento1)
                .HasColumnName("COMPL01");
            entityTypeBuilder
                .Property(entity => entity.CodigoComplemento2)
                .HasColumnName("COMPL02");
            entityTypeBuilder
                .Property(entity => entity.CodigoComplemento3)
                .HasColumnName("COMPL03");
            entityTypeBuilder
                .Property(entity => entity.CodigoComplemento4)
                .HasColumnName("COMPL04");
            entityTypeBuilder
                .Property(entity => entity.CodigoComplemento5)
                .HasColumnName("COMPL05");

        }

        private void LegacyModelBookkeepingAccountView(ModelBuilder modelBuilder)
        {
            var entityTypeBuilder = modelBuilder.Entity<BookkeepingAccountView>();
            entityTypeBuilder
                .ToTable("CT_PLANO_CONTAS_VISAO");
            entityTypeBuilder
                .Property(entity => entity.Code)
                .HasColumnName("CODIGO")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.Classification)
                .HasColumnName("CLASSIFICACAO")
                .HasMaxLength(50);
            entityTypeBuilder
                .Property(entity => entity.Name)
                .HasColumnName("NOME")
                .HasMaxLength(100);
            entityTypeBuilder
                .Property(entity => entity.IsSynthetic)
                .HasColumnName("SINTETICO")
                .HasMaxLength(1);
            entityTypeBuilder
                .Property(entity => entity.Changed)
                .HasColumnName("ALTERADO")
                .HasMaxLength(15);
            entityTypeBuilder
                .Property(entity => entity.CenterCostCode)
                .HasColumnName("CCUSTO")
                .HasMaxLength(20);
            entityTypeBuilder
                .Property(entity => entity.Number)
                .HasColumnName("NUMERO")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.ParentCode)
                .HasColumnName("CODPAI")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.Date)
                .HasColumnName("DATA")
                .HasConversion(new LegacyDateStringToNullableAppliedTimezoneDateTime(_tenantProperties))
                .HasMaxLength(8);
            entityTypeBuilder
                .Property(entity => entity.Model)
                .HasColumnName("MODELO")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.Nature)
                .HasColumnName("NATUREZA")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.Level)
                .HasColumnName("NIVEL")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.Required)
                .HasColumnName("OBRIGATORIOCC")
                .HasConversion(new BoolToStringConverter("N", "S"))
                .HasMaxLength(1);
            entityTypeBuilder
                .Property(entity => entity.Type)
                .HasColumnName("TIPO")
                .HasMaxLength(1);
            entityTypeBuilder
                .Property(entity => entity.Summarised)
                .HasColumnName("RESUMIDO")
                .HasMaxLength(50);
            entityTypeBuilder
                .Property(entity => entity.Order)
                .HasColumnName("ORDEM")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.BookkeepingAccountGroupLegacyId)
                .HasColumnName("RECNO_GRUPO_PLANOCONTAS")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.CtaCode)
                .HasColumnName("COD_CTA_REF")
                .HasMaxLength(50);
            entityTypeBuilder
                .Property(entity => entity.FgRevenues)
                .HasColumnName("FG_RECEITAS")
                .HasConversion(new BoolToStringConverter("N", "S"))
                .HasMaxLength(1);
            entityTypeBuilder
                .Property(entity => entity.CalculateCosts)
                .HasColumnName("CONTABILIZAR_CUSTEIO")
                .HasConversion(new BoolToStringConverter("N", "S"))
                .HasMaxLength(1);
            entityTypeBuilder
                .Property(entity => entity.CtaCode)
                .HasColumnName("COD_CTA_REF")
                .HasMaxLength(50);
            entityTypeBuilder
                .Property(entity => entity.ParentViewLegacyId)
                .HasColumnName("RECNO_CONTA_PAI")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.ParentAccountLegacyId)
                .HasColumnName("RECNO_CT_PLANO_CONT_VIS_CAB")
                .HasMaxLength(4);
            entityTypeBuilder
                .Property(entity => entity.LegacyId)
                .HasColumnName("R_E_C_N_O_")
                .HasMaxLength(4)
                .ValueGeneratedOnAdd();
        }

        private void RemoveSdkProperties(ModelBuilder modelBuilder)
        {
            foreach (var type in modelBuilder.Model.GetEntityTypes()
                .Where(t => t.ClrType.GetInterfaces().Contains(typeof(IMustHaveTenant)))
                .Select(t => t.ClrType))
                modelBuilder.Entity(type).Ignore(nameof(IMustHaveTenant.TenantId));

            foreach (var type in modelBuilder.Model.GetEntityTypes()
                .Where(t => t.ClrType.GetInterfaces().Contains(typeof(IMustHaveEnvironment)))
                .Select(t => t.ClrType))
                modelBuilder.Entity(type).Ignore(nameof(IMustHaveEnvironment.EnvironmentId));

            foreach (var type in modelBuilder.Model.GetEntityTypes()
                         .Where(t => t.ClrType.GetInterfaces().Contains(typeof(IMustHaveTenantAndEnvironment)))
                         .Select(t => t.ClrType))
            {
                modelBuilder.Entity(type).Ignore(nameof(IMustHaveTenantAndEnvironment.EnvironmentId));
                modelBuilder.Entity(type).Ignore(nameof(IMustHaveTenantAndEnvironment.TenantId));
            }

            foreach (var type in modelBuilder.Model.GetEntityTypes()
                .Where(t => t.ClrType.GetInterfaces().Contains(typeof(ISoftDelete)))
                .Select(t => t.ClrType))
                modelBuilder.Entity(type).Ignore(nameof(ISoftDelete.IsDeleted));
            foreach (var type in modelBuilder.Model.GetEntityTypes()
                .Where(t => t.ClrType.GetInterfaces().Contains(typeof(IDeletionAuditedEntity)))
                .Select(t => t.ClrType))
            {
                modelBuilder.Entity(type).Ignore(nameof(IDeletionAuditedEntity.DeleterId));
                modelBuilder.Entity(type).Ignore(nameof(IDeletionAuditedEntity.DeletionTime));
            }
            foreach (var type in modelBuilder.Model.GetEntityTypes()
                .Where(t => t.ClrType.GetInterfaces().Contains(typeof(IModificationAuditedEntity)))
                .Select(t => t.ClrType))
            {
                modelBuilder.Entity(type).Ignore(nameof(IModificationAuditedEntity.LastModificationTime));
                modelBuilder.Entity(type).Ignore(nameof(IModificationAuditedEntity.LastModifierId));
            }
            foreach (var type in modelBuilder.Model.GetEntityTypes()
                .Where(t => t.ClrType.GetInterfaces().Contains(typeof(ICreationAuditedEntity)))
                .Select(t => t.ClrType))
            {
                modelBuilder.Entity(type).Ignore(nameof(ICreationAuditedEntity.CreationTime));
                modelBuilder.Entity(type).Ignore(nameof(ICreationAuditedEntity.CreatorId));
            }
        }
    }
}
