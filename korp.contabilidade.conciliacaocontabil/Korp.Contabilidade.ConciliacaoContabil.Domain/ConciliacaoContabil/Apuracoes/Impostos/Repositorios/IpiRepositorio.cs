using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Viasoft.Core.DDD.Repositories;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios;

public class IpiRepositorio: IImpostoRepositorio
{
    private readonly IRepository<FiscalNotaEntrada> _notaEntrada;
    private readonly IRepository<FiscalItemNotaEntrada> _itemNotaEntrada;
    private readonly IRepository<OutrosLancamentosFiscais> _lancamentosFiscais;
    private readonly IRepository<FiscalNotaSaida> _notaSaida;
    private readonly IRepository<FiscalItemNotaSaida> _itemNotaSaida;
    private readonly IRepository<CabecalhoOutrosLancamentosFiscais> _cabecalhoOutrosLancamentos;
    private readonly ILogger<IpiRepositorio> _logger;
    public IpiRepositorio(IRepository<FiscalNotaEntrada> notaEntrada, IRepository<FiscalItemNotaEntrada> itemNotaEntrada, 
        IRepository<OutrosLancamentosFiscais> lancamentosFiscais, IRepository<FiscalNotaSaida> notaSaida, IRepository<FiscalItemNotaSaida> itemNotaSaida,
        IRepository<CabecalhoOutrosLancamentosFiscais> cabecalhoOutrosLancamentos, ILogger<IpiRepositorio> logger)
    {
        _notaEntrada = notaEntrada;
        _itemNotaEntrada = itemNotaEntrada;
        _lancamentosFiscais = lancamentosFiscais;
        _notaSaida = notaSaida;
        _itemNotaSaida = itemNotaSaida;
        _cabecalhoOutrosLancamentos = cabecalhoOutrosLancamentos;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarNotaEntrada(ConciliacaoContabil conciliacaoContabil)
    {
        var notaEntrada = _notaEntrada.AsQueryable().AsNoTracking();
        var itemNotaEntrada = _itemNotaEntrada.AsQueryable().AsNoTracking();
        var lancamentoFiscais = _lancamentosFiscais.AsQueryable().AsNoTracking();
        var cabecalhoOutrosLancamentos = _cabecalhoOutrosLancamentos.AsQueryable().AsNoTracking();

        var notaEntradaEntrada = from e in notaEntrada.FiltrarPeriodoContabil(conciliacaoContabil)
            join c in itemNotaEntrada on e.LegacyId equals c.IdNotaEntrada
            where c.ValorIpi > 0
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
                Valor = g.Sum(x => x.ValorIpi.Value),
                Codigo = g.Key.CodigoFornecedor,
                LegacyCompanyId = g.Key.LegacyCompanyId,
                Parcela = "1",
            };
       
        var outrosNotaEntrada = from l in lancamentoFiscais
            join c in cabecalhoOutrosLancamentos.FiltrarEmpresaContabil(conciliacaoContabil) on l.LegacyIdCabecalhoOutros equals c.Codigo
            where l.Imposto == "IPI" && l.CreditoDebito == "C"  &&
                  c.Ano == conciliacaoContabil.DataInicial.Year && c.Mes == conciliacaoContabil.DataInicial.Month 
            select new ImpostoDto
            {
                Data = new DateOnly(c.Ano, c.Mes, DateTime.DaysInMonth(c.Ano, c.Mes)),
                Documento = l.Historico,
                Valor = l.Valor,
                LegacyCompanyId = c.LegacyCompanyId,
                Parcela = "1",
            };  
        
        var listaImpostoDto = new List<ImpostoDto>();
        listaImpostoDto.AddRange(await notaEntradaEntrada.AsNoTracking().ToListAsync());
        listaImpostoDto.AddRange(await outrosNotaEntrada.AsNoTracking().ToListAsync());
        
        return listaImpostoDto;
    }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarNotaSaida(ConciliacaoContabil conciliacaoContabil)
    {
        var notaSaida = _notaSaida.AsQueryable().AsNoTracking();
        var itemNotaSaida = _itemNotaSaida.AsQueryable().AsNoTracking();
        var lancamentoFiscais = _lancamentosFiscais.AsQueryable().AsNoTracking();
        var cabecalhoOutrosLancamentos = _cabecalhoOutrosLancamentos.AsQueryable().AsNoTracking();

        var notaSaidaSaida = from e in notaSaida.FiltrarPeriodoContabil(conciliacaoContabil)
            join c in itemNotaSaida on e.LegacyId equals c.IdNotaSaida
            where c.ValorIpi > 0
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
                Valor = g.Sum(x => x.ValorIpi.Value) * - 1,
                Codigo = g.Key.CodigoCliente,
                LegacyCompanyId = g.Key.LegacyCompanyId,
                Parcela = "1",
            };
        var outrosNotaSaida = from l in lancamentoFiscais
            join c in cabecalhoOutrosLancamentos.FiltrarEmpresaContabil(conciliacaoContabil) on l.LegacyIdCabecalhoOutros equals c.Codigo
            where l.Imposto == "IPI" && l.CreditoDebito == "D"  &&
                  c.Ano == conciliacaoContabil.DataInicial.Year && c.Mes == conciliacaoContabil.DataInicial.Month 
            select new NotaSaidaDto
            {
                Data = new DateOnly(c.Ano, c.Mes, DateTime.DaysInMonth(c.Ano, c.Mes)),
                Documento = l.Historico,
                Valor = l.Valor * -1,
                LegacyCompanyId = c.LegacyCompanyId,
                Parcela = "1",
            };
        
        var listaImpostoDto = new List<NotaSaidaDto>();
        listaImpostoDto.AddRange(await notaSaidaSaida.AsNoTracking().ToListAsync());
        listaImpostoDto.AddRange(await outrosNotaSaida.AsNoTracking().ToListAsync());
        
        return listaImpostoDto;
    }
}