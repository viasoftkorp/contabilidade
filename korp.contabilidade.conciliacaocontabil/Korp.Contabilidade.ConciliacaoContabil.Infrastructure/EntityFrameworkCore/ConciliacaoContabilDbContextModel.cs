using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Viasoft.Core.EntityFrameworkCore.SQLServer.Extensions;

namespace Korp.Contabilidade.ConciliacaoContabil.Infrastructure.EntityFrameworkCore;

public static class ConciliacaoContabilDbContextModel
{
    public static void ModelEntidades(this ModelBuilder modelBuilder)
    {
        modelBuilder.ModelCreditoPis();
        modelBuilder.ModelCreditoCofins();
        modelBuilder.ModelDepreciacaoLinearValores();
        modelBuilder.ModelDepreciacaoAceleradaValores();
        modelBuilder.ModelDepreciacaoGerencialValores();
        modelBuilder.ModelDepreciacaoIncentivadaValores();
        modelBuilder.ModelDepreciacaoConfiguracao();
        modelBuilder.ModelDepreciacaoReavaliacaoLinear();
        modelBuilder.ModelDepreciacaoReavaliacaoGerencial();
        modelBuilder.ModelDepreciacaoReavaliacao();
        modelBuilder.ModelFornecedor();
        modelBuilder.ModelCliente();
        modelBuilder.ModelRegistroF100Contas();
        modelBuilder.ModelRegistroF100();
        modelBuilder.ModelPatrimonialBens();
        modelBuilder.ModelPatrimonialGrupoBem();
        modelBuilder.ModelPatrimonialItensVinculados();
        modelBuilder.ModelFaturamentoNotaFiscalCaixa();
        modelBuilder.ModelFaturamentoNotaFiscalNfce();
        modelBuilder.ModelFaturamentoNotaFiscal();
        modelBuilder.ModelCabOutrosLancamentosFiscais();
        modelBuilder.ModelOutrosLancamentosFiscais();
        modelBuilder.ModelOutrosLancamentosNotaFiscal();
        modelBuilder.ModelItemNotaSaida();
        modelBuilder.ModelNotaSaida();
        modelBuilder.ModelItemNotaEntrada();
        modelBuilder.ModelNotaEntrada();
        modelBuilder.ModelExtrato();
        modelBuilder.ModelEmpresa();
        modelBuilder.ModelContasPagar();
        modelBuilder.ModelContasReceber();
        modelBuilder.ModelPlanoConta();
        modelBuilder.ModelTipoLancamento();
        modelBuilder.ModelCabecalhoLancamentoContabil();
        modelBuilder.ModelLancamentoContabil();
        modelBuilder.ModelConciliacaoContabilLancamento();
        modelBuilder.ModelConciliacaoContabilLancamentoDetalhamento();

        modelBuilder.ModelConciliacaoContabilApuracao();
        modelBuilder.ModelConciliacaoContabilApuracaoDetalhamento();
        
        modelBuilder.ModelTipoConciliacaoContabil();
        modelBuilder.ModelTipoConciliacaoContabilConta();
        modelBuilder.ModelConciliacaoContabilEmpresa();
        modelBuilder.ModelConciliacaoContabilEtapa();
        modelBuilder.ModelConciliacaoContabil();
        modelBuilder.ModelCtParametros();
        modelBuilder.ModelBanco();
        modelBuilder.ModelEstoque();
    }
    private static void ModelCreditoPis(this ModelBuilder modelBuilder)
    {
        var creditoPis =  modelBuilder.Entity<CreditoPis>();
        creditoPis.ToTable("CT_CREDITO_ATIVO_PIS");
        creditoPis.Property(ce => ce.LegacyIdBem)
            .HasColumnName("RECNO_BEM");
        creditoPis.Property(ce => ce.Ano)
            .HasColumnName("ANO");            
        creditoPis.Property(ce => ce.Mes)
            .HasColumnName("MES");   
        creditoPis.Property(ce => ce.Valor)
            .HasColumnName("VALOR_CREDITO");   
        creditoPis.ApplyDecimalMappingForEntity();
    }

    private static void ModelCreditoCofins(this ModelBuilder modelBuilder)
    {
        var creditoCofins =  modelBuilder.Entity<CreditoCofins>();
        creditoCofins.ToTable("CT_CREDITO_ATIVO_COFINS");
        creditoCofins.Property(ce => ce.LegacyIdBem)
            .HasColumnName("RECNO_BEM");
        creditoCofins.Property(ce => ce.Ano)
            .HasColumnName("ANO");            
        creditoCofins.Property(ce => ce.Mes)
            .HasColumnName("MES");   
        creditoCofins.Property(ce => ce.Valor)
            .HasColumnName("VALOR_CREDITO");   
        creditoCofins.ApplyDecimalMappingForEntity();
    }
    private static void ModelDepreciacaoAceleradaValores(this ModelBuilder modelBuilder)
    {
        var depreciacaoAcelerada =  modelBuilder.Entity<DepreciacaoAceleradaValores>();
        depreciacaoAcelerada.ToTable("CT_DEPRECIACAO_ACELERADA_VALORES");
        depreciacaoAcelerada.Property(ce => ce.LegacyIdBem)
            .HasColumnName("RECNO_BEM");    
        depreciacaoAcelerada.Property(ce => ce.Data)
            .HasColumnName("DATA")            
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8);   
        depreciacaoAcelerada.Property(ce => ce.ValorPis)
            .HasColumnName("CREDITO_PIS");   
        depreciacaoAcelerada.Property(ce => ce.ValorCofins)
            .HasColumnName("CREDITO_COFINS");   
        depreciacaoAcelerada.Property(ce => ce.Valor)
            .HasColumnName("VALOR");
        depreciacaoAcelerada.ApplyDecimalMappingForEntity();
    }
    
    private static void ModelDepreciacaoGerencialValores(this ModelBuilder modelBuilder)
    {
        var depreciacaoGerencial =  modelBuilder.Entity<DepreciacaoGerencialValores>();
        depreciacaoGerencial.ToTable("CT_DEPRECIACAO_GERENCIAL_VALORES");
        depreciacaoGerencial.Property(ce => ce.LegacyIdBem)
            .HasColumnName("RECNO_BEM");    
        depreciacaoGerencial.Property(ce => ce.Data)
            .HasColumnName("DATA")            
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8);   
        depreciacaoGerencial.Property(ce => ce.ValorPis)
            .HasColumnName("CREDITO_PIS");   
        depreciacaoGerencial.Property(ce => ce.ValorCofins)
            .HasColumnName("CREDITO_COFINS");   
        depreciacaoGerencial.Property(ce => ce.Valor)
            .HasColumnName("VALOR");
        depreciacaoGerencial.ApplyDecimalMappingForEntity();
    }
    
    private static void ModelDepreciacaoIncentivadaValores(this ModelBuilder modelBuilder)
    {
        var depreciacaoIncentivada =  modelBuilder.Entity<DepreciacaoIncentivadaValores>();
        depreciacaoIncentivada.ToTable("CT_DEPRECIACAO_INCENTIVADA_VALORES");
        depreciacaoIncentivada.Property(ce => ce.LegacyIdBem)
            .HasColumnName("RECNO_BEM");    
        depreciacaoIncentivada.Property(ce => ce.Data)
            .HasColumnName("DATA")            
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8);   
        depreciacaoIncentivada.Property(ce => ce.ValorPis)
            .HasColumnName("CREDITO_PIS");   
        depreciacaoIncentivada.Property(ce => ce.ValorCofins)
            .HasColumnName("CREDITO_COFINS");   
        depreciacaoIncentivada.Property(ce => ce.Valor)
            .HasColumnName("VALOR");
        depreciacaoIncentivada.ApplyDecimalMappingForEntity();
    }
    
    private static void ModelDepreciacaoLinearValores(this ModelBuilder modelBuilder)
    {
        var depreciacaoLinear =  modelBuilder.Entity<DepreciacaoLinearValores>();
        depreciacaoLinear.ToTable("CT_DEPRECIACAO_LINEAR_VALORES");
        depreciacaoLinear.Property(ce => ce.LegacyIdBem)
            .HasColumnName("RECNO_BEM");    
        depreciacaoLinear.Property(ce => ce.Data)
            .HasColumnName("DATA")            
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8);   
        depreciacaoLinear.Property(ce => ce.ValorPis)
            .HasColumnName("CREDITO_PIS");   
        depreciacaoLinear.Property(ce => ce.ValorCofins)
            .HasColumnName("CREDITO_COFINS");
        depreciacaoLinear.Property(ce => ce.Valor)
            .HasColumnName("VALOR");
        depreciacaoLinear.ApplyDecimalMappingForEntity();
    }
    private static void ModelDepreciacaoReavaliacaoLinear(this ModelBuilder modelBuilder)
    {
        var depreciacaoLinear =  modelBuilder.Entity<DepreciacaoReavaliacaoLinear>();
        depreciacaoLinear.ToTable("CT_DEPRECIACAO_REAVALIACAO_LINEAR");
        depreciacaoLinear.Property(ce => ce.LegacyIdBem)
            .HasColumnName("RECNO_BEM");    
        depreciacaoLinear.Property(ce => ce.Data)
            .HasColumnName("DATA")            
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8);   
        depreciacaoLinear.Property(ce => ce.Valor)
            .HasColumnName("VALOR");
        depreciacaoLinear.ApplyDecimalMappingForEntity();
    }
    private static void ModelDepreciacaoReavaliacaoGerencial(this ModelBuilder modelBuilder)
    {
        var depreciacaoGerencial =  modelBuilder.Entity<DepreciacaoReavaliacaoGerencial>();
        depreciacaoGerencial.ToTable("CT_DEPRECIACAO_REAVALIACAO_ACELERADA");
        depreciacaoGerencial.Property(ce => ce.LegacyIdBem)
            .HasColumnName("RECNO_BEM");    
        depreciacaoGerencial.Property(ce => ce.Data)
            .HasColumnName("DATA")            
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8);   
        depreciacaoGerencial.Property(ce => ce.Valor)
            .HasColumnName("VALOR");
        depreciacaoGerencial.ApplyDecimalMappingForEntity();
    }
    
    private static void ModelDepreciacaoReavaliacao(this ModelBuilder modelBuilder)
    {
        var depreciacaoGerencial =  modelBuilder.Entity<PatrimonialReavaliacao>();
        depreciacaoGerencial.ToTable("CT_MOVPATRI");
        depreciacaoGerencial.Property(ce => ce.CodigoBem)
            .HasColumnName("COD_BEM");    
        depreciacaoGerencial.Property(ce => ce.Data)
            .HasColumnName("DATA")            
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8);   
        depreciacaoGerencial.Property(ce => ce.Valor)
            .HasColumnName("VALOR");
        depreciacaoGerencial.Property(ce => ce.Operacao)
            .HasColumnName("OPERACAO")
            .HasColumnType("VARCHAR")
            .HasConversion(new BoolToStringConverter("-","+"));
        depreciacaoGerencial.ApplyDecimalMappingForEntity();
    }
    private static void ModelDepreciacaoConfiguracao(this ModelBuilder modelBuilder)
    {
        var depreciacaoConfiguracao =  modelBuilder.Entity<DepreciacaoConfiguracao>();
        depreciacaoConfiguracao.ToTable("CT_DEPRECIACAO_CONFIGURACAO");
        depreciacaoConfiguracao.Property(ce => ce.LegacyIdBem)
            .HasColumnName("RECNO_BEM");    
        depreciacaoConfiguracao.Property(ce => ce.TipoContabilidade)
            .HasColumnName("GERA_CONTABILIDADE");     
    }
    
    private static void ModelFornecedor(this ModelBuilder modelBuilder)
    {
        var fornecedor =  modelBuilder.Entity<Fornecedor>();
        fornecedor.ToTable("FORNECED");
        fornecedor.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");    
        fornecedor.Property(ce => ce.Codigo)
            .HasColumnName("CODIGO");
        fornecedor.Property(ce => ce.CodigoConta)
            .HasColumnName("COD_CONTA");
        fornecedor.Property(ce => ce.ContaContabilAdiantamento)
            .HasColumnName("CONTA_CONTABIL_ADIANTAMENTO");
        fornecedor.Property(ce => ce.RazaoSocial)
            .HasColumnName("RASSOC");    
    }
    
    private static void ModelCliente(this ModelBuilder modelBuilder)
    {
        var cliente =  modelBuilder.Entity<Cliente>();
        cliente.ToTable("CLIENTES");
        cliente.Property(ce => ce.Codigo)
            .HasColumnName("CODIGO");
        cliente.Property(ce => ce.CodigoConta)
            .HasColumnName("COD_CONTA");
        cliente.Property(ce => ce.ContaContabilAdiantamento)
            .HasColumnName("CONTA_CONTABIL_ADIANTAMENTO");
        cliente.Property(ce => ce.RazaoSocial)
            .HasColumnName("RASSOC");    
    }
    
    private static void ModelRegistroF100Contas(this ModelBuilder modelBuilder)
    {
        var registroF100Contas =  modelBuilder.Entity<RegistroF100Contas>();
        registroF100Contas.ToTable("EFDPC_REGISTRO_F100_CONTAS");
        registroF100Contas.Property(ce => ce.CodigoConta)
            .HasColumnName("CONTA");
        registroF100Contas.Property(ce => ce.LegacyRegistroF100)
            .HasColumnName("RECNO_F100");
        registroF100Contas.Property(ce => ce.Operacao)
            .HasColumnName("OPERACAO");
    }
    
    private static void ModelRegistroF100(this ModelBuilder modelBuilder)
    {
        var registroF100 =  modelBuilder.Entity<RegistroF100>();
        registroF100.ToTable("EFDPC_REGISTRO_F100");
        registroF100.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        registroF100.Property(ce => ce.LegacyCompanyId)
            .HasColumnName("EMPRESA_RECNO");
        registroF100.Property(ce => ce.DescricaoOperacao)
            .HasColumnName("DESCRICAO_OPE");
        registroF100.Property(ce => ce.ValorPis)
            .HasColumnName("VL_PIS");
        registroF100.Property(ce => ce.ValorCofins)
            .HasColumnName("VL_COFINS");
        registroF100.Property(ce => ce.AliquotaPis)
            .HasColumnName("ALIQ_PIS");
        registroF100.Property(ce => ce.AliquotaCofins)
            .HasColumnName("ALIQ_COFINS");
        registroF100.Property(ce => ce.Data)
            .HasColumnName("DT_OPER")            
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8);
        registroF100.Property(ce => ce.IndicadorOperacao)
            .HasColumnName("IND_OPER");
        registroF100.Property(ce => ce.ParametrizacaoLancamento)
            .HasColumnName("PARAMETRIZACAO")
            .HasColumnType("VARCHAR")
            .HasConversion(new BoolToStringConverter("N","S"));
        registroF100.Property(ce => ce.CodigoFornecedor)
            .HasColumnName("COD_PART");
        registroF100.ApplyDecimalMappingForEntity();  
    }
    
    private static void ModelPatrimonialGrupoBem(this ModelBuilder modelBuilder)
    {
        var patrimonialGrupo =  modelBuilder.Entity<PatrimonialGrupoBem>();
        patrimonialGrupo.ToTable("CT_GRUPO_BEM");
        patrimonialGrupo.Property(ce => ce.Codigo)
            .HasColumnName("CODIGO");
    }
    
    private static void ModelPatrimonialBens(this ModelBuilder modelBuilder)
    {
        var patrimonial =  modelBuilder.Entity<PatrimonialBens>();
        patrimonial.ToTable("CT_BENS");
        patrimonial.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        patrimonial.Property(ce => ce.LegacyCompanyId)
            .HasColumnName("EMPRESA_RECNO");
        patrimonial.Property(ce => ce.CodigoBem)
            .HasColumnName("CODIGO");
        patrimonial.Property(ce => ce.Nome)
            .HasColumnName("NOME");
        patrimonial.Property(ce => ce.NumeroNota)
            .HasColumnName("NF_ENTRADA");
        patrimonial.Property(ce => ce.DataEntrada)
            .HasColumnName("DT_ENTRADA")            
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8); 
        patrimonial.Property(ce => ce.DataSaida)
            .HasColumnName("DT_SAIDA")            
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8); 
        patrimonial.Property(ce => ce.Fornecedor)
            .HasColumnName("FORNECEDOR");
        patrimonial.Property(ce => ce.RazaoSocial)
            .HasColumnName("FORNECEDOR_RASSOC");  
        patrimonial.Property(ce => ce.Valor)
            .HasColumnName("VALOR");
        patrimonial.Property(ce => ce.ValorSaida)
            .HasColumnName("VALOR_SAIDA");        
        patrimonial.Property(ce => ce.CodigoGrupoBem)
            .HasColumnName("CODGRUPO_BEM");
        patrimonial.Property(ce => ce.EfdpcGerarBem)
            .HasColumnName("EFDPC_GERAR_BEM")
            .HasColumnType("VARCHAR")
            .HasConversion(new BoolToStringConverter("N","S"));
        patrimonial.Property(ce => ce.OpcaoCreditoEfd)
            .HasColumnName("OPCAO_CREDITO_EFD");
        patrimonial.ApplyDecimalMappingForEntity();
    }  
    private static void ModelPatrimonialItensVinculados(this ModelBuilder modelBuilder)
    {
        var patrimonial =  modelBuilder.Entity<PatrimonialItensVinculados>();
        patrimonial.ToTable("CT_BENS_ITENS_VINCULADOS");
        patrimonial.Property(ce => ce.LegacIdBem)
            .HasColumnName("RECNO_CT_BENS");
        patrimonial.Property(ce => ce.Data)
            .HasColumnName("DATA")            
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8); 
        patrimonial.Property(ce => ce.Valor)
            .HasColumnName("VL_ITEM_CARREGADO");
        patrimonial.Property(ce => ce.CodigoFornecedorCliente)
            .HasColumnName("PARTICIPANTE");
        patrimonial.Property(ce => ce.RazaoFornecedorCliente)
            .HasColumnName("RAZAO");        
        patrimonial.ApplyDecimalMappingForEntity();
    }  
    private static void ModelFaturamentoNotaFiscalCaixa(this ModelBuilder modelBuilder)
    {
        var notaSaida =  modelBuilder.Entity<FaturamentoNotaFiscalNfceCaixa>();
        notaSaida.ToTable("NFCE_CAIXA");
        notaSaida.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        notaSaida.Property(ce => ce.LegacyCompanyId)
            .HasColumnName("EMPRESA_RECNO");   
        notaSaida.ApplyDecimalMappingForEntity();
    } 
    private static void ModelFaturamentoNotaFiscal(this ModelBuilder modelBuilder)
    {
        var notaSaida =  modelBuilder.Entity<FaturamentoNotaFiscal>();
        notaSaida.ToTable("NOTAFISC");
        notaSaida.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        notaSaida.Property(ce => ce.Data)
            .HasColumnName("EMI")
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8); 
        notaSaida.Property(ce => ce.Documento)
            .HasColumnName("NUMERO");
        notaSaida.Property(ce => ce.CodigoCliente)
            .HasColumnName("CLI");  
        notaSaida.Property(ce => ce.RazaoSocialCliente)
            .HasColumnName("RAZAO");   
        notaSaida.Property(ce => ce.Valor)
            .HasColumnName("VAL");  
        notaSaida.Property(ce => ce.Devolucao)
            .HasColumnName("NOTADEV")  
            .HasColumnType("VARCHAR")
            .HasConversion(new BoolToStringConverter("N","S"));
        notaSaida.Property(ce => ce.Situacao)
            .HasColumnName("SITUACAO");  
        notaSaida.Property(ce => ce.LegacyCompanyId)
            .HasColumnName("EMPRESA_RECNO");   
        notaSaida.ApplyDecimalMappingForEntity();
    }  
    
    private static void ModelFaturamentoNotaFiscalNfce(this ModelBuilder modelBuilder)
    {
        var notaSaida =  modelBuilder.Entity<FaturamentoNotaFiscalNfce>();
        notaSaida.ToTable("NFCE_NOTA");
        notaSaida.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        notaSaida.Property(ce => ce.Data)
            .HasColumnName("DATA_EMISSAO");
        notaSaida.Property(ce => ce.Documento)
            .HasColumnName("NUMERO");
        notaSaida.Property(ce => ce.CodigoCliente)
            .HasColumnName("CODIGO_CLIENTE");  
        notaSaida.Property(ce => ce.RazaoSocialCliente)
            .HasColumnName("NOME_CLIENTE");   
        notaSaida.Property(ce => ce.Valor)
            .HasColumnName("TOTAL_BRUTO");  
        notaSaida.Property(ce => ce.Situacao)
            .HasColumnName("SITUACAO");  
        notaSaida.Property(ce => ce.LegacyFaturamentoCaixaId)
            .HasColumnName("RECNO_NFCE_CAIXA");   
        notaSaida.ApplyDecimalMappingForEntity();
    }  
    
    private static void ModelCabOutrosLancamentosFiscais(this ModelBuilder modelBuilder)
    {
        var outrosLancamento =  modelBuilder.Entity<CabecalhoOutrosLancamentosFiscais>();
        outrosLancamento.ToTable("CT_CABOUTROS");
        outrosLancamento.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        outrosLancamento.Property(ce => ce.Codigo)
            .HasColumnName("CODIGO");
        outrosLancamento.Property(ce => ce.Ano)
            .HasColumnName("ANO");
        outrosLancamento.Property(ce => ce.Mes)
            .HasColumnName("MES");
        outrosLancamento.Property(ce => ce.LegacyCompanyId)
            .HasColumnName("EMPRESA_RECNO");
    }  

    private static void ModelOutrosLancamentosFiscais(this ModelBuilder modelBuilder)
    {
        var outrosLancamento =  modelBuilder.Entity<OutrosLancamentosFiscais>();
        outrosLancamento.ToTable("CT_OUTROS");
        outrosLancamento.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        outrosLancamento.Property(ce => ce.Imposto)
            .HasColumnName("IMPOSTO");
        outrosLancamento.Property(ce => ce.CreditoDebito)
            .HasColumnName("OUTROS");
        outrosLancamento.Property(ce => ce.Historico)
            .HasColumnName("HISTORICO");
        outrosLancamento.Property(ce => ce.DataLancamento)
            .HasColumnName("DATA_LANC")
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8); 
        outrosLancamento.Property(ce => ce.Valor)
            .HasColumnName("VALOR");
        outrosLancamento.Property(ce => ce.LegacyIdCabecalhoOutros)
            .HasColumnName("CODIGO");
        outrosLancamento.ApplyDecimalMappingForEntity();
    }  
    
    private static void ModelOutrosLancamentosNotaFiscal(this ModelBuilder modelBuilder)
    {
        var notaEntrada =  modelBuilder.Entity<OutrosLancamentosNotaFiscal>();
        notaEntrada.ToTable("CT_OUTROS_NOTA_FISCAL");
        notaEntrada.Property(ce => ce.LegacyOutrosLancamentosId)
            .HasColumnName("RECNO_CT_OUTROS");
        notaEntrada.Property(ce => ce.LegacyNotaEntradaId)
            .HasColumnName("RECNO_CT_NOTAENT");        
        notaEntrada.Property(ce => ce.LegacyNotaSaidaId)
            .HasColumnName("RECNO_CT_NOTASAI");         
    }  

    private static void ModelItemNotaEntrada(this ModelBuilder modelBuilder)
    {
        var itemNotaEntrada =  modelBuilder.Entity<FiscalItemNotaEntrada>();
        itemNotaEntrada.ToTable("CT_ITENSENT");
        itemNotaEntrada.Property(ce => ce.IdNotaEntrada)
            .HasColumnName("ESCRITA_RECNO");
        itemNotaEntrada.Property(ce => ce.ValorIcms)
            .HasColumnName("VALORICMS");
        itemNotaEntrada.Property(ce => ce.ValorPis)
            .HasColumnName("VALORPIS");
        itemNotaEntrada.Property(ce => ce.ValorCofins)
            .HasColumnName("VALORCOFINS");  
        itemNotaEntrada.Property(ce => ce.ValorIpi)
            .HasColumnName("VALORIPI");          
        itemNotaEntrada.ApplyDecimalMappingForEntity();
    }  
    
    private static void ModelNotaEntrada(this ModelBuilder modelBuilder)
    {
        var notaEntrada =  modelBuilder.Entity<FiscalNotaEntrada>();
        notaEntrada.ToTable("CT_NOTAENT");
        notaEntrada.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        notaEntrada.Property(ce => ce.Data)
            .HasColumnName("ENTRADA")
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8); 
        notaEntrada.Property(ce => ce.Documento)
            .HasColumnName("NOTAFISCAL");
        notaEntrada.Property(ce => ce.Serie)
            .HasColumnName("SERIE");
        notaEntrada.Property(ce => ce.CodigoFornecedor)
            .HasColumnName("FORNECEDOR");  
        notaEntrada.Property(ce => ce.RazaoSocial)
            .HasColumnName("RASSOC");   
        notaEntrada.Property(ce => ce.Situacao)
            .HasColumnName("SITUACAO"); 
        notaEntrada.Property(ce => ce.LegacyCompanyId)
            .HasColumnName("EMPRESA_RECNO");   
        notaEntrada.ApplyDecimalMappingForEntity();
    }  
    
    private static void ModelItemNotaSaida(this ModelBuilder modelBuilder)
    {
        var itemNotaSaida =  modelBuilder.Entity<FiscalItemNotaSaida>();
        itemNotaSaida.ToTable("CT_ITENSSAI");
        itemNotaSaida.Property(ce => ce.IdNotaSaida)
            .HasColumnName("ESCRITA_RECNO");
        itemNotaSaida.Property(ce => ce.ValorIcms)
            .HasColumnName("VALORICMS");
        itemNotaSaida.Property(ce => ce.ValorIcmsSt)
            .HasColumnName("VALORICMSST");
        itemNotaSaida.Property(ce => ce.ValorPis)
            .HasColumnName("VALORPIS");
        itemNotaSaida.Property(ce => ce.ValorCofins)
            .HasColumnName("VALORCOFINS");  
        itemNotaSaida.Property(ce => ce.ValorIpi)
            .HasColumnName("VALORIPI");          
        itemNotaSaida.Property(ce => ce.ValorIss)
            .HasColumnName("VALORISS");
        itemNotaSaida.ApplyDecimalMappingForEntity();
    }  
    
    private static void ModelNotaSaida(this ModelBuilder modelBuilder)
    {
        var notaSaida =  modelBuilder.Entity<FiscalNotaSaida>();
        notaSaida.ToTable("CT_NOTASAI");
        notaSaida.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        notaSaida.Property(ce => ce.Data)
            .HasColumnName("ENTRADA")
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8); 
        notaSaida.Property(ce => ce.Documento)
            .HasColumnName("NUMERO");
        notaSaida.Property(ce => ce.Serie)
            .HasColumnName("SERIE");        
        notaSaida.Property(ce => ce.CodigoCliente)
            .HasColumnName("CLIENTE");  
        notaSaida.Property(ce => ce.RazaoSocial)
            .HasColumnName("RASSOC");   
        notaSaida.Property(ce => ce.LegacyCompanyId)
            .HasColumnName("EMPRESA_RECNO");   
        notaSaida.Property(ce => ce.Situacao)
            .HasColumnName("SITUACAO"); 
        notaSaida.Property(ce => ce.Servico)
            .HasColumnName("SERVICO")    
            .HasColumnType("VARCHAR")
            .HasConversion(new BoolToStringConverter("N","S"));
        notaSaida.ApplyDecimalMappingForEntity();
    }  
    
    private static void ModelContasPagar(this ModelBuilder modelBuilder)
    {
        var contasPagar =  modelBuilder.Entity<ContasPagar>();
        contasPagar.ToTable("CONTASP");
        contasPagar.Property(ce => ce.DataEntrada)
            .HasColumnName("DATAENT")
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8); 
        contasPagar.Property(ce => ce.DataPagamento)
            .HasColumnName("DATAPAGO")
            .HasConversion(new DateOnlyNullableConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8); 
        contasPagar.Property(ce => ce.Documento)
            .HasColumnName("DOCUMENTO");
        contasPagar.Property(ce => ce.RazaoSocial)
            .HasColumnName("RASSOC");
        contasPagar.Property(ce => ce.Valor)
            .HasColumnName("VALOR");
        contasPagar.Property(ce => ce.Juros)
            .HasColumnName("JUROS"); 
        contasPagar.Property(ce => ce.Multa)
            .HasColumnName("MULTA"); 
        contasPagar.Property(ce => ce.Desconto)
            .HasColumnName("DESCONTO"); 
        contasPagar.Property(ce => ce.Retencao)
            .HasColumnName("RETENCAO");
        contasPagar.Property(ce => ce.Codigo)
            .HasColumnName("CODIGO");
        contasPagar.Property(ce => ce.Status)
            .HasColumnName("STATUS");
        contasPagar.Property(ce => ce.LegacyCompanyId)
            .HasColumnName("EMPRESA_RECNO");
        contasPagar.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");   
        contasPagar.Property(ce => ce.LegacyIdOrigem)
            .HasColumnName("RECNO_TITULO_ORIGEM");  
        contasPagar.Property(ce => ce.IdDctf)
            .HasColumnName("RECNO_DCTF_DATA"); 
        contasPagar.Property(ce => ce.Parcela)
            .HasColumnName("PARCELA"); 
        contasPagar.Property(ce => ce.NumeroExtrato)
            .HasColumnName("LANCEXTRATO"); 
        contasPagar.Property(ce => ce.Adiantamento)
            .HasColumnName("ADIANTAMENTO")
            .HasColumnType("VARCHAR")
            .HasConversion(new BoolToStringConverter("N","S"));
        contasPagar.ApplyDecimalMappingForEntity();
    }  
    
    private static void ModelExtrato(this ModelBuilder modelBuilder)
    {
        var extrato =  modelBuilder.Entity<Extrato>();
        extrato.ToTable("EXTRATO_BANCARIO");
        extrato.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        extrato.Property(ce => ce.Data)
            .HasColumnName("DATA")
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8); 
        extrato.Property(ce => ce.Valor)
            .HasColumnName("VALOR");
        extrato.Property(ce => ce.DebitoCredito)
            .HasColumnName("DC");        
        extrato.ApplyDecimalMappingForEntity();
    }  
    
    private static void ModelContasReceber(this ModelBuilder modelBuilder)
    {
        var contasReceber =  modelBuilder.Entity<ContasReceber>();
        contasReceber.ToTable("CONTASR");
        contasReceber.Property(ce => ce.DataEntrada)
            .HasColumnName("DATAENT")
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8); 
        contasReceber.Property(ce => ce.DataPagamento)
            .HasColumnName("DATAPAGO")
            .HasConversion(new DateOnlyNullableConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8); 
        contasReceber.Property(ce => ce.Documento)
            .HasColumnName("DOCUMENTO");
        contasReceber.Property(ce => ce.RazaoSocial)
            .HasColumnName("RASSOC");
        contasReceber.Property(ce => ce.Valor)
            .HasColumnName("VALOR");
        contasReceber.Property(ce => ce.Juros)
            .HasColumnName("JUROS"); 
        contasReceber.Property(ce => ce.Multa)
            .HasColumnName("MULTA"); 
        contasReceber.Property(ce => ce.Desconto)
            .HasColumnName("DESCONTO"); 
        contasReceber.Property(ce => ce.Retencao)
            .HasColumnName("RETENCAO");
        contasReceber.Property(ce => ce.Codigo)
            .HasColumnName("CODIGO");
        contasReceber.Property(ce => ce.Status)
            .HasColumnName("STATUS");
        contasReceber.Property(ce => ce.LegacyCompanyId)
            .HasColumnName("EMPRESA_RECNO");
        contasReceber.Property(ce => ce.Parcela)
            .HasColumnName("PARCELA"); 
        contasReceber.Property(ce => ce.NumeroExtrato)
            .HasColumnName("LANCEXTRATO");     
        contasReceber.Property(ce => ce.Adiantamento)
            .HasColumnName("ADIANTAMENTO")
            .HasColumnType("VARCHAR")
            .HasConversion(new BoolToStringConverter("N","S"));
        contasReceber.ApplyDecimalMappingForEntity();
    }  
    
    private static void ModelPlanoConta(this ModelBuilder modelBuilder)
    {
        var planoConta =  modelBuilder.Entity<PlanoConta>();
        planoConta.ToTable("CT_PLANO_CONTAS");
        planoConta.Property(ce => ce.Codigo)
            .HasColumnName("CODIGO");
        planoConta.Property(ce => ce.Descricao)
            .HasColumnName("NOME"); 
    }  
    
    private static void ModelEmpresa(this ModelBuilder modelBuilder)
    {
        var empresa =  modelBuilder.Entity<Empresa>();
        empresa.ToTable("EMPRESA");
        empresa.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        empresa.Property(ce => ce.Nome)
            .HasColumnName("FANTASIA");
    }  

    private static void ModelTipoLancamento(this ModelBuilder modelBuilder)
    {
        var tipoLancamento =  modelBuilder.Entity<TipoLancamento>();
        tipoLancamento.ToTable("CT_TIPO_LANCAMENTO");
        tipoLancamento.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        tipoLancamento.Property(ce => ce.Codigo)
            .HasColumnName("CODIGO");
        tipoLancamento.Property(ce => ce.Descricao)
            .HasColumnName("DESCRICAO");
        tipoLancamento.ApplyDecimalMappingForEntity();
    }   
    
    private static void ModelCabecalhoLancamentoContabil(this ModelBuilder modelBuilder)
    {
        var lancamentoCabContabilContabil =  modelBuilder.Entity<CabecalhoLancamentoContabil>();
        lancamentoCabContabilContabil.ToTable("CT_CABLANCAMENTOS");
        lancamentoCabContabilContabil.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        lancamentoCabContabilContabil.Property(ce => ce.NumeroLancamento)
            .HasColumnName("NLANC");     
        lancamentoCabContabilContabil.Property(ce => ce.CodigoCliente)
            .HasColumnName("CLI");      
        lancamentoCabContabilContabil.Property(ce => ce.CodigoFornecedor)
            .HasColumnName("FORN");              
        lancamentoCabContabilContabil.Property(ce => ce.Data)
            .HasColumnName("DTLANC")
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8); 
        lancamentoCabContabilContabil.Property(ce => ce.LegacyCompanyId)
            .HasColumnName("EMPRESA_RECNO");
        lancamentoCabContabilContabil.Property(ce => ce.TipoLancamento)
            .HasColumnName("TIPOLANC");   
        lancamentoCabContabilContabil.ApplyDecimalMappingForEntity();
    }   
    private static void ModelLancamentoContabil(this ModelBuilder modelBuilder)
    {
        var lancamentoContabilContabil =  modelBuilder.Entity<LancamentoContabil>();
        lancamentoContabilContabil.ToTable("CT_LANCAMENTOS");
        lancamentoContabilContabil.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        lancamentoContabilContabil.Property(ce => ce.LegacyCompanyId)
            .HasColumnName("EMPRESA_RECNO");
        lancamentoContabilContabil.Property(ce => ce.NumeroLancamento)
            .HasColumnName("NLANC");
        lancamentoContabilContabil.Property(ce => ce.Historico)
            .HasColumnName("OBS");
        lancamentoContabilContabil.Property(ce => ce.ValorDebito)
            .HasColumnName("VALORDEBITO");
        lancamentoContabilContabil.Property(ce => ce.ValorCredito)
            .HasColumnName("VALORCREDITO");
        lancamentoContabilContabil.Property(ce => ce.CodigoConta)
            .HasColumnName("COD_CONTA");
        lancamentoContabilContabil.ApplyDecimalMappingForEntity();
    }
    
    private static void ModelConciliacaoContabilApuracao(this ModelBuilder modelBuilder)
    {
        var conciliacaoContabilApuracao =  modelBuilder.Entity<ConciliacaoContabilApuracao>();
        conciliacaoContabilApuracao.ToTable("CT_CONCILIACAO_CONTABIL_APURACAO");
        conciliacaoContabilApuracao.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        conciliacaoContabilApuracao.Property(ce => ce.Data)
            .HasColumnName("DATA")
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8); 
        conciliacaoContabilApuracao.Property(ce => ce.Documento)
            .HasColumnName("DOCUMENTO");
        conciliacaoContabilApuracao.Property(ce => ce.Parcela)
            .HasColumnName("PARCELA");
        conciliacaoContabilApuracao.Property(ce => ce.CodigoFornecedorCliente)
            .HasColumnName("CODIGO_FORNECEDOR_CLIENTE");
        conciliacaoContabilApuracao.Property(ce => ce.RazaoSocialFornecedorCliente)
            .HasColumnName("RASSOC_FORNECEDOR_CLIENTE");
        conciliacaoContabilApuracao.Property(ce => ce.Valor)
            .HasColumnName("VALOR");
        conciliacaoContabilApuracao.Property(ce => ce.Conciliado)
            .HasColumnName("CONCILIADO")
            .HasColumnType("VARCHAR")
            .HasConversion(new BoolToStringConverter("N","S"));
        conciliacaoContabilApuracao.Property(ce => ce.TipoValorApuracao)
            .HasColumnName("TIPO");
        conciliacaoContabilApuracao.Property(ce => ce.DescricaoTipoValorApuracao)
            .HasColumnName("TIPO_DESCRICAO");
        conciliacaoContabilApuracao.Property(ce => ce.LegacyCompanyId)
            .HasColumnName("EMPRESA_RECNO");
        conciliacaoContabilApuracao.Property(ce => ce.Chave)
            .HasColumnName("CHAVE");
        conciliacaoContabilApuracao.Property(ce => ce.IdConciliacaoContabil)
            .HasColumnName("RECNO_CT_CONCILIACAO_CAB"); 
        conciliacaoContabilApuracao
            .HasMany(c => c.ApuracoesDetalhamento)
            .WithOne()
            .HasForeignKey(detalhamento => detalhamento.IdConciliacaoContabilApuracao)
            .HasPrincipalKey(c => c.LegacyId)
            .OnDelete(DeleteBehavior.Cascade);
        conciliacaoContabilApuracao.ApplyDecimalMappingForEntity();
    }
    private static void ModelConciliacaoContabilApuracaoDetalhamento(this ModelBuilder modelBuilder)
    {
        var conciliacaoContabilApuracaoDetalhamento =  modelBuilder.Entity<ConciliacaoContabilApuracaoDetalhamento>();
        conciliacaoContabilApuracaoDetalhamento.ToTable("CT_CONCILIACAO_CONTABIL_APURACAO_DETALHAMENTO");
        conciliacaoContabilApuracaoDetalhamento.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        conciliacaoContabilApuracaoDetalhamento.Property(ce => ce.Data)
            .HasColumnName("DATA")
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8);    
        conciliacaoContabilApuracaoDetalhamento.Property(ce => ce.NumeroLancamento)
            .HasColumnName("NUMERO_LANCAMENTO");
        conciliacaoContabilApuracaoDetalhamento.Property(ce => ce.Historico)
            .HasColumnName("HISTORICO");
        conciliacaoContabilApuracaoDetalhamento.Property(ce => ce.Valor)
            .HasColumnName("VALOR");
        conciliacaoContabilApuracaoDetalhamento.Property(ce => ce.CodigoConta)
            .HasColumnName("COD_CONTA");
        conciliacaoContabilApuracaoDetalhamento.Property(ce => ce.LegacyCompanyId)
            .HasColumnName("EMPRESA_RECNO");
        conciliacaoContabilApuracaoDetalhamento.Property(ce => ce.CodigoFornecedorCliente)
            .HasColumnName("CODIGO_FORNECEDOR_CLIENTE");        
        conciliacaoContabilApuracaoDetalhamento.Property(ce => ce.IdConciliacaoContabilApuracao)
            .HasColumnName("RECNO_CT_CONCILIACAO_CONTABIL_APURACAO");    
        conciliacaoContabilApuracaoDetalhamento.Property(ce => ce.IdTipoLancamento)
            .HasColumnName("RECNO_TIPO_LANCAMENTO");    
        conciliacaoContabilApuracaoDetalhamento.ApplyDecimalMappingForEntity();
    }
    
      private static void ModelConciliacaoContabilLancamento(this ModelBuilder modelBuilder)
    {
        var conciliacaoContabilLancamentos =  modelBuilder.Entity<ConciliacaoContabilLancamento>();
        conciliacaoContabilLancamentos.ToTable("CT_CONCILIACAO_CONTABIL_LANCAMENTO");
        conciliacaoContabilLancamentos.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        conciliacaoContabilLancamentos.Property(ce => ce.Data)
            .HasColumnName("DATA")
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8);    
        conciliacaoContabilLancamentos.Property(ce => ce.NumeroLancamento)
            .HasColumnName("NUMERO_LANCAMENTO");
        conciliacaoContabilLancamentos.Property(ce => ce.Historico)
            .HasColumnName("HISTORICO");
        conciliacaoContabilLancamentos.Property(ce => ce.Valor)
            .HasColumnName("VALOR");
        conciliacaoContabilLancamentos.Property(ce => ce.CodigoConta)
            .HasColumnName("COD_CONTA");
        conciliacaoContabilLancamentos.Property(ce => ce.CodigoFornecedorCliente)
            .HasColumnName("CODIGO_FORNECEDOR_CLIENTE");        
        conciliacaoContabilLancamentos.Property(ce => ce.Conciliado)
            .HasColumnName("CONCILIADO")
            .HasColumnType("VARCHAR")
            .HasConversion(new BoolToStringConverter("N","S"));
        conciliacaoContabilLancamentos.Property(ce => ce.LegacyCompanyId)
            .HasColumnName("EMPRESA_RECNO");
        conciliacaoContabilLancamentos.Property(ce => ce.Chave)
            .HasColumnName("CHAVE");  
        conciliacaoContabilLancamentos.Property(ce => ce.IdConciliacaoContabil)
            .HasColumnName("RECNO_CT_CONCILIACAO_CAB");
        conciliacaoContabilLancamentos.Property(ce => ce.IdTipoLancamento)
            .HasColumnName("RECNO_TIPO_LANCAMENTO");        
        conciliacaoContabilLancamentos
            .HasMany(c => c.LancamentosDetalhamento)
            .WithOne()
            .HasForeignKey(detalhamento => detalhamento.IdConciliacaoContabilLancamento)
            .HasPrincipalKey(c => c.LegacyId)
            .OnDelete(DeleteBehavior.Cascade);        
        conciliacaoContabilLancamentos.ApplyDecimalMappingForEntity();

    }
    private static void ModelConciliacaoContabilLancamentoDetalhamento(this ModelBuilder modelBuilder)
    {
        var conciliacaoContabilLancamentosDetalhamento =  modelBuilder.Entity<ConciliacaoContabilLancamentoDetalhamento>();
        conciliacaoContabilLancamentosDetalhamento.ToTable("CT_CONCILIACAO_CONTABIL_LANCAMENTO_DETALHAMENTO");
        conciliacaoContabilLancamentosDetalhamento.Property(ce => ce.LegacyId).
            HasColumnName("R_E_C_N_O_");
        conciliacaoContabilLancamentosDetalhamento.Property(ce => ce.Data).
            HasColumnName("DATA")
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8);    
        conciliacaoContabilLancamentosDetalhamento.Property(ce => ce.Documento).
            HasColumnName("DOCUMENTO"); 
        conciliacaoContabilLancamentosDetalhamento.Property(ce => ce.Parcela).
            HasColumnName("PARCELA"); 
        conciliacaoContabilLancamentosDetalhamento.Property(ce => ce.CodigoFornecedorCliente).
            HasColumnName("CODIGO_FORNECEDOR_CLIENTE"); 
        conciliacaoContabilLancamentosDetalhamento.Property(ce => ce.RazaoSocialFornecedorCliente).
            HasColumnName("RASSOC_FORNECEDOR_CLIENTE"); 
        conciliacaoContabilLancamentosDetalhamento.Property(ce => ce.Valor).
            HasColumnName("VALOR"); 
        conciliacaoContabilLancamentosDetalhamento.Property(ce => ce.TipoValorApuracao).
            HasColumnName("TIPO"); 
        conciliacaoContabilLancamentosDetalhamento.Property(ce => ce.DescricaoTipoValorApuracao).
            HasColumnName("TIPO_DESCRICAO"); 
        conciliacaoContabilLancamentosDetalhamento.Property(ce => ce.LegacyCompanyId).
            HasColumnName("EMPRESA_RECNO");
        conciliacaoContabilLancamentosDetalhamento.Property(ce => ce.IdConciliacaoContabilLancamento)
            .HasColumnName("RECNO_CT_CONCILIACAO_CONTABIL_LANCAMENTO");
        conciliacaoContabilLancamentosDetalhamento.ApplyDecimalMappingForEntity();
    }
    
    private static void ModelConciliacaoContabilEmpresa(this ModelBuilder modelBuilder)
    {
        var conciliacaoEmpresa =  modelBuilder.Entity<ConciliacaoContabilEmpresa>();
        conciliacaoEmpresa.ToTable("CT_CONCILIACAO_CONTABIL_EMPRESA");
        conciliacaoEmpresa.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        conciliacaoEmpresa.Property(ce => ce.IdConciliacaoContabil)
            .HasColumnName("RECNO_CONCILIACAO_CAB");
        conciliacaoEmpresa.Property(ce => ce.LegacyCompanyId).
            HasColumnName("EMPRESA_RECNO");
    }
    private static void ModelTipoConciliacaoContabilConta(this ModelBuilder modelBuilder)
    {
        var tipoConciliacaoContabil =  modelBuilder.Entity<TipoConciliacaoContabilConta>();
        tipoConciliacaoContabil.ToTable("CT_CONCILIACAO_CONTABIL_CONFIG_RATEIO_CONTAS");
        tipoConciliacaoContabil.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        tipoConciliacaoContabil.Property(ce => ce.CodigoConta)
            .HasColumnName("CODIGO");      
        tipoConciliacaoContabil.Property(ce => ce.IdTipoConciliacaoContabil)
            .HasColumnName("RECNO_RATEIO");
        tipoConciliacaoContabil.Property(ce => ce.Descricao)
            .HasColumnName("DESCRICAO");
    }
    private static void ModelTipoConciliacaoContabil(this ModelBuilder modelBuilder)
    {
        var tipoConciliacaoContabil =  modelBuilder.Entity<TipoConciliacaoContabil>();
        tipoConciliacaoContabil.ToTable("CT_CONCILIACAO_CONTABIL_CONFIG_RATEIO");
        tipoConciliacaoContabil.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        tipoConciliacaoContabil.Property(ce => ce.Descricao)
            .HasColumnName("DESCRICAO");      
        tipoConciliacaoContabil.Property(ce => ce.TipoApuracao)
            .HasColumnName("TIPO_APURACAO");
        tipoConciliacaoContabil.Property(ce => ce.Ativo)
            .HasColumnName("ATIVO")
            .HasColumnType("VARCHAR")
            .HasConversion(new BoolToStringConverter("N","S"));
        tipoConciliacaoContabil
            .HasMany(c => c.ConciliacaoContabilContas)
            .WithOne()
            .HasForeignKey(tipo => tipo.IdTipoConciliacaoContabil)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ModelConciliacaoContabilEtapa(this ModelBuilder modelBuilder)
    {
        var notaSaida =  modelBuilder.Entity<ConciliacaoContabilEtapa>();
        notaSaida.ToTable("CT_CONCILIACAO_CONTABIL_ETAPA");
        notaSaida.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        notaSaida.Property(ce => ce.IdConciliacaoContabil)
            .HasColumnName("RECNO_CT_CONCILIACAO_CONTABIL_CAB");
        notaSaida.Property(ce => ce.ProcessoGeracao)
            .HasColumnName("ETAPA");
        notaSaida.ApplyDecimalMappingForEntity();
    }  
    
    private static void ModelConciliacaoContabil(this ModelBuilder modelBuilder)
    {
        var conciliacao =  modelBuilder.Entity<Domain.ConciliacaoContabil.ConciliacaoContabil>();
        conciliacao.ToTable("CT_CONCILIACAO_CONTABIL_CAB");
        conciliacao.Property(ce => ce.LegacyId)
            .HasColumnName("R_E_C_N_O_");
        conciliacao.Property(ce => ce.Descricao)
            .HasColumnName("DESCRICAO");
        conciliacao.Property(ce => ce.Usuario)
            .HasColumnName("USUARIO");
        conciliacao.Property(ce => ce.DataHora)
            .HasColumnName("DATAHORA"); 
        conciliacao.Property(ce => ce.DataInicial)
            .HasColumnName("DATA_INICIAL")
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8);        
        conciliacao.Property(ce => ce.DataFinal)
            .HasColumnName("DATA_FINAL")
            .HasConversion(new DateOnlyConverter())
            .HasColumnType("VARCHAR")
            .HasMaxLength(8);             
        conciliacao.Property(ce => ce.Conciliado)
            .HasColumnName("CONCILIADO")
            .HasColumnType("VARCHAR")
            .HasConversion(new BoolToStringConverter("N","S"));
        conciliacao.Property(ce => ce.Status)
            .HasColumnName("STATUS");
        conciliacao.Property(ce => ce.Erro)
            .HasColumnName("ERRO"); 
        conciliacao.Property(ce => ce.IdTipoConciliacaoContabil)
            .HasColumnName("RECNO_CT_CONCILIACAO_CONTABIL_CONFIG_RATEIO");
        conciliacao
            .HasOne(c => c.TipoConciliacaoContabil)
            .WithOne()
            .HasForeignKey<Domain.ConciliacaoContabil.ConciliacaoContabil>(tipo => tipo.IdTipoConciliacaoContabil);
        conciliacao
            .HasMany(c => c.Etapas)
            .WithOne()
            .HasForeignKey(etapa => etapa.IdConciliacaoContabil)
            .HasPrincipalKey(c => c.LegacyId)
            .OnDelete(DeleteBehavior.Cascade);
        conciliacao
            .HasMany(c => c.Empresas)
            .WithOne()
            .HasForeignKey(empresa => empresa.IdConciliacaoContabil)
            .HasPrincipalKey(c => c.LegacyId)
            .OnDelete(DeleteBehavior.Cascade);
        conciliacao
            .HasMany(c => c.Lancamentos)
            .WithOne()
            .HasForeignKey(lancamento => lancamento.IdConciliacaoContabil)
            .HasPrincipalKey(c => c.LegacyId)
            .OnDelete(DeleteBehavior.Cascade);
        conciliacao
            .HasMany(c => c.Apuracoes)
            .WithOne()
            .HasForeignKey(apuracao => apuracao.IdConciliacaoContabil)
            .HasPrincipalKey(c => c.LegacyId)
            .OnDelete(DeleteBehavior.Cascade);        
    }
    private static void ModelCtParametros(this ModelBuilder modelBuilder)
    {
        var ctParametros =  modelBuilder.Entity<CtParametros>();
        ctParametros.ToTable("CT_PARAMETROS");
        ctParametros.Property(cp => cp.LegacyId).HasColumnName("R_E_C_N_O_");
        ctParametros.Property(cp => cp.Banco).HasColumnName("BANCO");
        ctParametros.Property(cp => cp.Estoque).HasColumnName("ESTOQUE");
        ctParametros.Property(cp => cp.Fornecedor).HasColumnName("FORNECEDOR");
        ctParametros.Property(cp => cp.PlanoCliente).HasColumnName("CLIENTE");
        ctParametros.Property(cp => cp.ContaContabilAdiantamentoFornecedor).HasColumnName("CONTA_CONTABIL_ADIANTAMENTO_FORNECEDOR");
        ctParametros.Property(cp => cp.ContaContabilAdiantamentoCliente).HasColumnName("CONTA_CONTABIL_ADIANTAMENTO_CLIENTE");
    }
    private static void ModelBanco(this ModelBuilder modelBuilder)
    {
        var bancos =  modelBuilder.Entity<Banco>();
        bancos.ToTable("BANCOS");
        bancos.Property(b => b.Id).HasColumnName("ID");
        bancos.Property(b => b.CtaCredito).HasColumnName("CTACREDITO");
        bancos.Property(b => b.CtaDebito).HasColumnName("CTADEBITO");
    }
    private static void ModelEstoque(this ModelBuilder modelBuilder)
    {
        var estoque =  modelBuilder.Entity<Estoque>();
        estoque.ToTable("ESTOQUE");
        estoque.Property(e => e.Codigo).HasColumnName("CODIGO");
        estoque.Property(e => e.CodigoConta).HasColumnName("COD_CONTA");
    }
}