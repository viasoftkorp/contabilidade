using System;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Faturamento;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Faturamento.Repositorio;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Financeiro;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Financeiro.Repositorios;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Cofins;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Pis;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Patrimonial;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Patrimonial.Repositorio;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Microsoft.Extensions.DependencyInjection;
using Viasoft.Core.IoC.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes;

public class ApuracaoFactory : IApuracaoFactory, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public ApuracaoFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IApuracao CriarApuracao(ConciliacaoContabil apuracaoContabil)
    {
        switch (apuracaoContabil.TipoConciliacaoContabil.TipoApuracao)
        {
            case TipoApuracaoConciliacaoContabil.ExtratoBancarioContasPagasRecebidas:
                return CriarApuracaoContasPagarFinanceiro<ExtratoRepositorio>(ContaTipoFinanceiro.Pagamento); 
            case TipoApuracaoConciliacaoContabil.ContasReceberFinanceiro:
                return CriarApuracaoContasReceberFinanceiro<ContasReceberRepositorio>(ContaTipoFinanceiro.Recebimento); 
            case TipoApuracaoConciliacaoContabil.IcmsCreditarFiscal:
                return CriarApuracaoImpostoFiscal<IcmsRepositorio>(TipoValorApuracaoConciliacaoContabil.IcmsCreditarFiscal, TipoFiscal.Credito); 
            case TipoApuracaoConciliacaoContabil.PisCreditarFiscal:
                return CriarApuracaoImpostoFiscal<PisRepositorio>(TipoValorApuracaoConciliacaoContabil.PisCreditarFiscal, TipoFiscal.Credito); 
            case TipoApuracaoConciliacaoContabil.CofinsCreditarFiscal:
                return CriarApuracaoImpostoFiscal<CofinsRepositorio>(TipoValorApuracaoConciliacaoContabil.CofinsCreditarFiscal, TipoFiscal.Credito); 
            case TipoApuracaoConciliacaoContabil.IssCreditarFiscal:
                break;
            case TipoApuracaoConciliacaoContabil.IcmsstCreditarFiscal:
                break;
            case TipoApuracaoConciliacaoContabil.ValorDosImobilizadosPatrimonial:
                return CriarApuracaoPatrimonial<ValorImobilizadoPatrimonial>();
            case TipoApuracaoConciliacaoContabil.AdiantamentoFornecedoresFinanceiro:
                return CriarApuracaoContasPagarFinanceiro<AdiantamentoFornecedoresRepositorio>(ContaTipoFinanceiro.Pagamento); 
            case TipoApuracaoConciliacaoContabil.ContasPagarFinanceiro:
                return CriarApuracaoContasPagarFinanceiro<ContasPagarRepositorio>(ContaTipoFinanceiro.Pagamento); 
            case TipoApuracaoConciliacaoContabil.IcmsRecolherFiscal:
                return CriarApuracaoImpostoFiscal<IcmsRepositorio>(TipoValorApuracaoConciliacaoContabil.IcmsRecolherFiscal, TipoFiscal.Debito); 
            case TipoApuracaoConciliacaoContabil.PisRecolherFiscal:
                return CriarApuracaoImpostoFiscal<PisRepositorio>(TipoValorApuracaoConciliacaoContabil.PisRecolherFiscal, TipoFiscal.Debito); 
            case TipoApuracaoConciliacaoContabil.CofinsRecolherFiscal:
                return CriarApuracaoImpostoFiscal<CofinsRepositorio>(TipoValorApuracaoConciliacaoContabil.CofinsRecolherFiscal, TipoFiscal.Debito); 
            case TipoApuracaoConciliacaoContabil.IssRecolherFiscal:
                return CriarApuracaoImpostoFiscal<IssRepositorio>(TipoValorApuracaoConciliacaoContabil.IssRecolherFiscal, TipoFiscal.Debito); 
            case TipoApuracaoConciliacaoContabil.IcmsstRecolherFiscal:
                return CriarApuracaoImpostoFiscal<IcmsstRepositorio>(TipoValorApuracaoConciliacaoContabil.IcmsStRecolherFiscal, TipoFiscal.Debito); 
            case TipoApuracaoConciliacaoContabil.AdiantamentoClientesFinanceiro:
                return CriarApuracaoContasReceberFinanceiro<AdiantamentoClientesRepositorio>(ContaTipoFinanceiro.Recebimento); 
            case TipoApuracaoConciliacaoContabil.JurosPagosFinanceiro:
                return CriarApuracaoContasPagarFinanceiro<JurosPagosRepositorio>(ContaTipoFinanceiro.Pagamento); 
            case TipoApuracaoConciliacaoContabil.MultasPagasFinanceiro:
                return CriarApuracaoContasPagarFinanceiro<MultasPagasRepositorio>(ContaTipoFinanceiro.Pagamento); 
            case TipoApuracaoConciliacaoContabil.DescontosObtidosFinanceiro:
                return CriarApuracaoContasPagarFinanceiro<DescontosObtidosRepositorio>(ContaTipoFinanceiro.Pagamento); 
            case TipoApuracaoConciliacaoContabil.JurosRecebidosFinanceiro:
                return CriarApuracaoContasReceberFinanceiro<JurosRecebidosRepositorio>(ContaTipoFinanceiro.Recebimento); 
            case TipoApuracaoConciliacaoContabil.MultasRecebidasFinanceiro:
                return CriarApuracaoContasReceberFinanceiro<MultasRecebidasRepositorio>(ContaTipoFinanceiro.Recebimento); 
            case TipoApuracaoConciliacaoContabil.DescontosConcedidosFinanceiro:
                return CriarApuracaoContasReceberFinanceiro<DescontosConcedidosRepositorio>(ContaTipoFinanceiro.Recebimento); 
            case TipoApuracaoConciliacaoContabil.ValorBrutoDeVendasFaturamento:
                return CriarApuracaoFaturamento<ValorBrutoVendasFaturamento>();
            case TipoApuracaoConciliacaoContabil.IcmsSobreVendasFaturamento:
                break;
            case TipoApuracaoConciliacaoContabil.PisSobreVendaFaturamento:
                break;
            case TipoApuracaoConciliacaoContabil.CofinsSobreVendaFaturamento:
                break;
            case TipoApuracaoConciliacaoContabil.IssSobreVendasFaturamento:
                break;
            case TipoApuracaoConciliacaoContabil.DepreciacoesPatrimonial:
                break;
            case TipoApuracaoConciliacaoContabil.IpiCreditarFiscal:
                return CriarApuracaoImpostoFiscal<IpiRepositorio>(TipoValorApuracaoConciliacaoContabil.IpiCreditarFiscal, TipoFiscal.Credito); 
            case TipoApuracaoConciliacaoContabil.IpiRecolherFiscal:
                return CriarApuracaoImpostoFiscal<IpiRepositorio>(TipoValorApuracaoConciliacaoContabil.IpiRecolherFiscal, TipoFiscal.Debito);
        }
        
        throw new ArgumentOutOfRangeException(nameof(apuracaoContabil), apuracaoContabil, null);
    }

    private IApuracao CriarApuracaoImpostoFiscal<T>(TipoValorApuracaoConciliacaoContabil tipoValorApuracao, TipoFiscal tipoFiscal) where T: IImpostoRepositorio
    {
        var repositorio = ActivatorUtilities.CreateInstance<T>(_serviceProvider);
        var adapter = new NotaFiscalImpostoRepositorioAdapter(tipoFiscal, repositorio);
        
        return new ApuracaoImposto(adapter, tipoValorApuracao);
    }
    
    private IApuracao CriarApuracaoContasReceberFinanceiro<T>(ContaTipoFinanceiro contaTipoFinanceiro) where T: IContasReceberFinanceiroRepositorio
    {
        var repositorio = ActivatorUtilities.CreateInstance<T>(_serviceProvider);
        var adapter = new ContaFinanceiroRepositorioAdapter(contaTipoFinanceiro,null, repositorio);
        
        return new ApuracaoFinanceiro(adapter);
    }
    
    private IApuracao CriarApuracaoContasPagarFinanceiro<T>(ContaTipoFinanceiro contaTipoFinanceiro) where T: IContasPagarFinanceiroRepositorio
    {
        var repositorio = ActivatorUtilities.CreateInstance<T>(_serviceProvider);
        var adapter = new ContaFinanceiroRepositorioAdapter(contaTipoFinanceiro, repositorio,null);
        
        return new ApuracaoFinanceiro(adapter);
    }
    
    private IApuracao CriarApuracaoFaturamento<T>() where T: IFaturamentoRepositorio
    {
        var repositorio = ActivatorUtilities.CreateInstance<T>(_serviceProvider);
        var adapter = new NotaFaturamentoRepositorioAdapter(repositorio);
        
        return new ApuracaoFaturamento(adapter);
    }
    
    private IApuracao CriarApuracaoPatrimonial<T>() where T: IPatrimonialRepositorio
    {
        var repositorio = ActivatorUtilities.CreateInstance<T>(_serviceProvider);
        var adapter = new BemPatrimonialRepositorioAdapter(repositorio);
        
        return new ApuracaoPatrimonial(adapter);
    }
}