using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Viasoft.Core.EntityFrameworkCore.Context;
using Viasoft.Core.Storage.Schema;

namespace Korp.Contabilidade.ConciliacaoContabil.Infrastructure.EntityFrameworkCore;

public class ConciliacaoContabilDbContext: BaseDbContext
{
    public DbSet<CabecalhoLancamentoContabil> CabecalhoLancamentoContabeis {get; set;}
    public DbSet<ConciliacaoContabilApuracao> ConciliacaoContabilApuracao {get; set;}
    public DbSet<ConciliacaoContabilApuracaoDetalhamento> ConciliacaoContabilApuracaoDetalhamento {get; set;}
    public DbSet<ConciliacaoContabilEmpresa> ConciliacaoContabilEmpresa {get; set;}
    public DbSet<ConciliacaoContabilLancamento> ConciliacaoContabilLancamento {get; set;}
    public DbSet<ConciliacaoContabilLancamentoDetalhamento> ConciliacaoContabilLancamentoDetalhamento {get; set;}
    public DbSet<ConciliacaoContabilEtapa> ConciliacaoContabilEtapa { get; set; }
    public DbSet<LancamentoContabil> LancamentoContabil {get; set;}
    public DbSet<PlanoConta> PlanoConta {get; set;}
    public DbSet<TipoConciliacaoContabil> TipoConciliacaoContabil {get; set;}
    public DbSet<TipoConciliacaoContabilConta> TipoConciliacaoContabilConta {get; set;}
    public DbSet<Domain.ConciliacaoContabil.ConciliacaoContabil> ConciliacaoContabil {get; set;}
    public DbSet<ContasPagar> ContasPagar {get; set;}
    public DbSet<ContasReceber> ContasReceber { get; set; }
    public DbSet<Extrato> Extrato { get; set; }
    public DbSet<Empresa> Empresa { get; set; }
    public DbSet<FiscalNotaEntrada> FiscalNotaEntrada { get; set; }
    public DbSet<FiscalItemNotaEntrada> FiscalItemNotaEntrada { get; set; }
    public DbSet<FiscalNotaSaida> FiscalNotaSaida { get; set; }
    public DbSet<FiscalItemNotaSaida> FiscalItemNotaSaida { get; set; }
    public DbSet<OutrosLancamentosFiscais> OutrosLancamentosFiscais { get; set; }
    public DbSet<CabecalhoOutrosLancamentosFiscais> CabecalhoOutrosLancamentosFiscais { get; set; }
    public DbSet<OutrosLancamentosNotaFiscal> OutrosLancamentosNotaFiscal { get; set; }
    public DbSet<FaturamentoNotaFiscal> FaturamentoNotaFiscal { get; set; }
    public DbSet<FaturamentoNotaFiscalNfce> FaturamenteNotaFiscalNfce { get; set; }
    public DbSet<FaturamentoNotaFiscalNfceCaixa> FaturamentoNotaFiscalNfceCaixa { get; set; }
    public DbSet<PatrimonialBens> PatrimonialBens { get; set; }
    public DbSet<PatrimonialGrupoBem> PatrimonialGrupoBem { get; set; }
    public DbSet<TipoLancamento> TipoLancamento { get; set; }
    public DbSet<RegistroF100> RegistroF100 { get; set; }
    public DbSet<RegistroF100Contas> RegistroF100Contas { get; set; }
    public DbSet<Fornecedor> Fornecedor { get; set; }
    public DbSet<DepreciacaoConfiguracao> DepreciacaoConfiguracao { get; set; }
    public DbSet<DepreciacaoAceleradaValores> DepreciacaoAceleradaValores { get; set; }
    public DbSet<DepreciacaoGerencialValores> DepreciacaoGerencialValores { get; set; }
    public DbSet<DepreciacaoIncentivadaValores> DepreciacaoIncentivadaValores { get; set; }
    public DbSet<DepreciacaoLinearValores> DepreciacaoLinearValores { get; set; }
    public DbSet<CreditoPis> CreditoPis { get; set; }
    public DbSet<CreditoCofins> CreditoCofins { get; set; }
    public DbSet<PatrimonialItensVinculados> PatrimonialItensVinculados { get; set; }
    public DbSet<DepreciacaoReavaliacaoLinear> DepreciacaoReavaliacaoLinear { get; set; }
    public DbSet<DepreciacaoReavaliacaoGerencial> DepreciacaoReavaliacaoGerencial { get; set; }
    public DbSet<PatrimonialReavaliacao> PatrimonialReavaliacao { get; set; }
    public DbSet<Cliente> Cliente { get; set; }
    public DbSet<CtParametros> CtParametros { get; set; }
    public DbSet<Estoque> Estoques { get; set; }
    public DbSet<Banco> Bancos { get; set; }
    
    public ConciliacaoContabilDbContext(DbContextOptions options, ISchemaNameProvider schemaNameProvider, ILoggerFactory loggerFactory, IBaseDbContextConfigurationService configurationService) : base(options, schemaNameProvider, loggerFactory, configurationService)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ModelEntidades();
    }
}