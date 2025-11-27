using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.LancamentoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.LancamentoContabil.Repositorio;

public class LancamentoContabilRepositorio: ILancamentoContabilRepositorio, ITransientDependency
{
    private readonly IRepository<CabecalhoLancamentoContabil> _cabecalhoLancamentoContabeis;
    private readonly IRepository<ApuracoesEntidades.LancamentoContabil> _lancamentoContabeis;
    private readonly IRepository<PlanoConta> _planoContas;
    private readonly IRepository<Empresa> _empresa;
    private readonly IRepository<TipoLancamento> _tipoLancamento;
    private readonly ILogger<LancamentoContabilRepositorio> _logger;
    
    public LancamentoContabilRepositorio(IRepository<CabecalhoLancamentoContabil> cabecalhoLancamentoContabeis, 
        IRepository<ApuracoesEntidades.LancamentoContabil> lancamentoContabeis, IRepository<PlanoConta> planoContas, 
        IRepository<Empresa> empresa, IRepository<TipoLancamento> tipoLancamento, ILogger<LancamentoContabilRepositorio> logger)
    {
        _lancamentoContabeis = lancamentoContabeis;
        _planoContas = planoContas;
        _empresa = empresa;
        _tipoLancamento = tipoLancamento;
        _logger = logger;
        _cabecalhoLancamentoContabeis = cabecalhoLancamentoContabeis;
    }

    public async Task<List<LancamentoContabilOutput>> ListarLancamentosContabeis(ConciliacaoContabil conciliacaoContabil)
    {
        _logger.LogInformation("iniciando listagem dos lançamentos contábeis para a conciliação {id} do tipo {conciliacaoContabil}", conciliacaoContabil.Id, conciliacaoContabil.TipoConciliacaoContabil.TipoApuracao.ToString());
        
        var cabLancamento = _cabecalhoLancamentoContabeis.AsQueryable().AsNoTracking();
        var lancamentos = _lancamentoContabeis.AsQueryable().AsNoTracking();
        var planoContas = _planoContas.AsQueryable().AsNoTracking();
        var empresas = _empresa.AsQueryable().AsNoTracking();
        var tipoLancamento = _tipoLancamento.AsQueryable().AsNoTracking();
        
        var contas = conciliacaoContabil.ListarCodigoContasContabeis();

        var resultadoDebito = from c in cabLancamento.FiltrarPeriodoContabil(conciliacaoContabil)
            join l in lancamentos on c.NumeroLancamento equals l.NumeroLancamento
            join p in planoContas on l.CodigoConta equals p.Codigo
            join e in empresas on l.LegacyCompanyId equals e.LegacyId
            from t in tipoLancamento.Where(p => c.TipoLancamento == p.Codigo).DefaultIfEmpty()
            where contas.Contains(p.Codigo) && l.ValorDebito > 0
            group l by new 
            {
                c.NumeroLancamento,
                c.Data,
                l.LegacyCompanyId,
                l.Historico,
                p.Codigo,
                p.Descricao,
                t.LegacyId,
                c.CodigoCliente,
                c.CodigoFornecedor
            } into g
            select new LancamentoContabilOutput
            {
                LegacyCompanyId = g.Key.LegacyCompanyId,
                DataLancamento = g.Key.Data,
                NumeroLancamento = g.Key.NumeroLancamento,
                Historico = g.Key.Historico,
                Valor =  VerificarTipoApuracao(conciliacaoContabil.TipoConciliacaoContabil.TipoApuracao) ? g.Sum(x => x.ValorDebito) : g.Sum(x => x.ValorDebito) * -1,
                CodigoConta = g.Key.Codigo,
                DescricaoConta = g.Key.Descricao,
                IdTipoLancamento = g.Key.LegacyId,
                CodigoFornecedorCliente = !string.IsNullOrEmpty(g.Key.CodigoCliente) 
                    ? g.Key.CodigoCliente 
                    : g.Key.CodigoFornecedor
            };
        
        var resultadoCredito = from c in cabLancamento.FiltrarPeriodoContabil(conciliacaoContabil)
            join l in lancamentos on c.NumeroLancamento equals l.NumeroLancamento
            join p in planoContas on l.CodigoConta equals p.Codigo
            join e in empresas on l.LegacyCompanyId equals e.LegacyId
            from t in tipoLancamento.Where(p => c.TipoLancamento == p.Codigo).DefaultIfEmpty()
            where contas.Contains(p.Codigo) && l.ValorCredito > 0
            group l by new 
            {
                c.NumeroLancamento,
                c.Data,
                l.LegacyCompanyId,
                l.Historico,
                p.Codigo,
                p.Descricao,
                t.LegacyId,
                c.TipoLancamento,
                c.CodigoCliente,
                c.CodigoFornecedor
            } into g
            select new LancamentoContabilOutput
            {
                LegacyCompanyId = g.Key.LegacyCompanyId,
                DataLancamento = g.Key.Data,
                NumeroLancamento = g.Key.NumeroLancamento,
                Historico = g.Key.Historico,
                Valor = VerificarTipoApuracao(conciliacaoContabil.TipoConciliacaoContabil.TipoApuracao) ? g.Sum(x => x.ValorCredito) * -1 : g.Sum(x => x.ValorCredito),
                CodigoConta = g.Key.Codigo,
                DescricaoConta = g.Key.Descricao,
                IdTipoLancamento = g.Key.LegacyId,
                CodigoFornecedorCliente = !string.IsNullOrEmpty(g.Key.CodigoCliente) 
                    ? g.Key.CodigoCliente 
                    : g.Key.CodigoFornecedor
            };
        var resultado = await resultadoDebito.AsNoTracking().ToListAsync();
        resultado.AddRange(await resultadoCredito.AsNoTracking().ToListAsync());
        
        _logger.LogInformation("finalizou listagem dos lançamentos contábeis para a conciliação {id} do tipo {conciliacaoContabil}", conciliacaoContabil.Id, conciliacaoContabil.TipoConciliacaoContabil.TipoApuracao.ToString());
        return resultado;
    }

    private bool VerificarTipoApuracao(TipoApuracaoConciliacaoContabil tipoApuracao)
    {
        switch (tipoApuracao)
        {
            case TipoApuracaoConciliacaoContabil.ExtratoBancarioContasPagasRecebidas:
                return true;
            case TipoApuracaoConciliacaoContabil.ContasReceberFinanceiro:
                return true;
            case TipoApuracaoConciliacaoContabil.IcmsCreditarFiscal:
                return true;
            case TipoApuracaoConciliacaoContabil.PisCreditarFiscal:
                return true;
            case TipoApuracaoConciliacaoContabil.CofinsCreditarFiscal:
                return true;
            case TipoApuracaoConciliacaoContabil.IssCreditarFiscal:
                return true;
            case TipoApuracaoConciliacaoContabil.IcmsstCreditarFiscal:
                return true;
            case TipoApuracaoConciliacaoContabil.ValorDosImobilizadosPatrimonial:
                return true;
            case TipoApuracaoConciliacaoContabil.AdiantamentoFornecedoresFinanceiro:
                return true;
            case TipoApuracaoConciliacaoContabil.ContasPagarFinanceiro:
                return true;
            case TipoApuracaoConciliacaoContabil.IcmsRecolherFiscal:
                return true;
            case TipoApuracaoConciliacaoContabil.PisRecolherFiscal:
                return true;
            case TipoApuracaoConciliacaoContabil.CofinsRecolherFiscal:
                return true;
            case TipoApuracaoConciliacaoContabil.IssRecolherFiscal:
                return true;
            case TipoApuracaoConciliacaoContabil.IcmsstRecolherFiscal:
                return true;
            case TipoApuracaoConciliacaoContabil.AdiantamentoClientesFinanceiro:
                return true;
            case TipoApuracaoConciliacaoContabil.JurosPagosFinanceiro:
                return false;
            case TipoApuracaoConciliacaoContabil.MultasPagasFinanceiro:
                return false;
            case TipoApuracaoConciliacaoContabil.DescontosObtidosFinanceiro:
                return false;
            case TipoApuracaoConciliacaoContabil.JurosRecebidosFinanceiro:
                return false;
            case TipoApuracaoConciliacaoContabil.MultasRecebidasFinanceiro:
                return false;
            case TipoApuracaoConciliacaoContabil.DescontosConcedidosFinanceiro:
                return false;
            case TipoApuracaoConciliacaoContabil.ValorBrutoDeVendasFaturamento:
                return false;
            case TipoApuracaoConciliacaoContabil.IcmsSobreVendasFaturamento:
                return true;
            case TipoApuracaoConciliacaoContabil.PisSobreVendaFaturamento:
                return true;
            case TipoApuracaoConciliacaoContabil.CofinsSobreVendaFaturamento:
                return true;
            case TipoApuracaoConciliacaoContabil.IssSobreVendasFaturamento:
                return true;
            case TipoApuracaoConciliacaoContabil.DepreciacoesPatrimonial:
                return true;
            case TipoApuracaoConciliacaoContabil.IpiCreditarFiscal:
                return true;
            case TipoApuracaoConciliacaoContabil.IpiRecolherFiscal:
                return true;
            default:
                throw new ArgumentOutOfRangeException(nameof(tipoApuracao), tipoApuracao, null);
        }
    }
}