using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios;

public class IcmsRepositorio: IImpostoRepositorio
{
    private readonly IRepository<FiscalNotaEntrada> _notaEntrada;
    private readonly IRepository<FiscalItemNotaEntrada> _itemNotaEntrada;
    private readonly IRepository<OutrosLancamentosFiscais> _lancamentosFiscais;
    private readonly IRepository<OutrosLancamentosNotaFiscal> _lancamentosNotaFiscal;
    private readonly IRepository<FiscalNotaSaida> _notaSaida;
    private readonly IRepository<FiscalItemNotaSaida> _itemNotaSaida;
    private readonly IRepository<CabecalhoOutrosLancamentosFiscais> _cabecalhoOutrosLancamentos;
    
    public IcmsRepositorio(IRepository<FiscalNotaEntrada> notaEntrada, IRepository<FiscalItemNotaEntrada> itemNotaEntrada, 
        IRepository<OutrosLancamentosFiscais> lancamentosFiscais, IRepository<OutrosLancamentosNotaFiscal> lancamentosNotaFiscal, 
        IRepository<FiscalNotaSaida> notaSaida, IRepository<FiscalItemNotaSaida> itemNotaSaida, IRepository<CabecalhoOutrosLancamentosFiscais> cabecalhoOutrosLancamentos)
    {
        _notaEntrada = notaEntrada;
        _itemNotaEntrada = itemNotaEntrada;
        _lancamentosFiscais = lancamentosFiscais;
        _lancamentosNotaFiscal = lancamentosNotaFiscal;
        _notaSaida = notaSaida;
        _itemNotaSaida = itemNotaSaida;
        _cabecalhoOutrosLancamentos = cabecalhoOutrosLancamentos;
    }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarNotaEntrada(ConciliacaoContabil conciliacaoContabil)
    {
        var notaEntrada = _notaEntrada.AsQueryable().AsNoTracking();
        var itemNotaEntrada = _itemNotaEntrada.AsQueryable().AsNoTracking();
        var lancamentoFiscais = _lancamentosFiscais.AsQueryable().AsNoTracking();
        var cabecalhoOutrosLancamentos = _cabecalhoOutrosLancamentos.AsQueryable().AsNoTracking();

        var notaEntradaEntrada = from e in notaEntrada.FiltrarPeriodoContabil(conciliacaoContabil)
            join c in itemNotaEntrada on e.LegacyId equals c.IdNotaEntrada
            where c.ValorIcms > 0
            group c by new
            {
                e.Data,
                e.Documento,
                e.Serie,
                e.RazaoSocial,
                e.CodigoFornecedor,
                e.LegacyCompanyId,
            }
            into g
            select new ImpostoDto
            {
                Data = g.Key.Data,
                Documento = g.Key.Documento.ToString()+"/"+ g.Key.Serie.ToString(),
                RazaoSocial = g.Key.RazaoSocial,
                Valor = g.Sum(x => x.ValorIcms.Value),
                Codigo = g.Key.CodigoFornecedor,
                LegacyCompanyId = g.Key.LegacyCompanyId,
                Parcela = "1",
            };
        
        var outrosNotaEntradaEntrada = from l in lancamentoFiscais
            join c in cabecalhoOutrosLancamentos.FiltrarEmpresaContabil(conciliacaoContabil) on l.LegacyIdCabecalhoOutros equals c.Codigo
            where l.Imposto == "ICMS" && l.CreditoDebito == "C"  &&
                  c.Ano == conciliacaoContabil.DataInicial.Year && c.Mes == conciliacaoContabil.DataInicial.Month 
            select new ImpostoDto
            {
                Data = new DateOnly(c.Ano, c.Mes, DateTime.DaysInMonth(c.Ano, c.Mes)),
                Documento = l.Historico,
                Valor = l.Valor,
                LegacyCompanyId = c.LegacyCompanyId,
                Parcela = "1",
            };  
        
        var apuracoesFiscalNotaEntrada = await notaEntradaEntrada.AsNoTracking().ToListAsync();
        apuracoesFiscalNotaEntrada.AddRange(await outrosNotaEntradaEntrada.AsNoTracking().ToListAsync());

        return apuracoesFiscalNotaEntrada;
    }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarNotaSaida(ConciliacaoContabil conciliacaoContabil)
    {
        var notaSaida = _notaSaida.AsQueryable().AsNoTracking();
        var itemNotaSaida = _itemNotaSaida.AsQueryable().AsNoTracking();
        var lancamentoFiscais = _lancamentosFiscais.AsQueryable().AsNoTracking();
        var cabecalhoOutrosLancamentos = _cabecalhoOutrosLancamentos.AsQueryable().AsNoTracking();

        var notaSaidaSaida = from e in notaSaida.FiltrarPeriodoContabil(conciliacaoContabil)
            join c in itemNotaSaida on e.LegacyId equals c.IdNotaSaida
            where  c.ValorIcms > 0
            group c by new
            {
                e.Data,
                e.Documento,
                e.Serie,
                e.RazaoSocial,
                e.CodigoCliente,
                e.LegacyCompanyId,
            }
            into g
            select new NotaSaidaDto
            {
                Data = g.Key.Data,
                Documento = g.Key.Documento.ToString()+"/"+ g.Key.Serie.ToString(),
                RazaoSocial = g.Key.RazaoSocial,
                Valor = g.Sum(x => x.ValorIcms.Value) * -1,
                Codigo = g.Key.CodigoCliente,
                LegacyCompanyId = g.Key.LegacyCompanyId,
                Parcela = "1",
            };

        var outrosNotaSaidaSaida = from l in lancamentoFiscais
            join c in cabecalhoOutrosLancamentos.FiltrarEmpresaContabil(conciliacaoContabil) on l.LegacyIdCabecalhoOutros equals c.Codigo
            where l.Imposto == "ICMS" && l.CreditoDebito == "D"  &&
                  c.Ano == conciliacaoContabil.DataInicial.Year && c.Mes == conciliacaoContabil.DataInicial.Month 
            select new NotaSaidaDto
            {
                Data = new DateOnly(c.Ano, c.Mes, DateTime.DaysInMonth(c.Ano, c.Mes)),
                Documento = l.Historico,
                Valor = l.Valor * -1,
                LegacyCompanyId = c.LegacyCompanyId,
                Parcela = "1",
            };
        
        var apuracoesFiscalNotaSaida = await notaSaidaSaida.AsNoTracking().ToListAsync();
        apuracoesFiscalNotaSaida.AddRange(await outrosNotaSaidaSaida.AsNoTracking().ToListAsync());

        return apuracoesFiscalNotaSaida;
    }
}