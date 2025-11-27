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

public class IcmsstRepositorio: IImpostoRepositorio
{
    private readonly IRepository<OutrosLancamentosFiscais> _lancamentosFiscais;
    private readonly IRepository<OutrosLancamentosNotaFiscal> _lancamentosNotaFiscal;
    private readonly IRepository<FiscalNotaSaida> _notaSaida;
    private readonly IRepository<FiscalItemNotaSaida> _itemNotaSaida;
    private readonly IRepository<CabecalhoOutrosLancamentosFiscais> _cabecalhoOutrosLancamentos;
    public IcmsstRepositorio(IRepository<OutrosLancamentosFiscais> lancamentosFiscais, IRepository<OutrosLancamentosNotaFiscal> lancamentosNotaFiscal, 
        IRepository<FiscalNotaSaida> notaSaida, IRepository<FiscalItemNotaSaida> itemNotaSaida, IRepository<CabecalhoOutrosLancamentosFiscais> cabecalhoOutrosLancamentos)
    {

        _lancamentosFiscais = lancamentosFiscais;
        _lancamentosNotaFiscal = lancamentosNotaFiscal;
        _notaSaida = notaSaida;
        _itemNotaSaida = itemNotaSaida;
        _cabecalhoOutrosLancamentos = cabecalhoOutrosLancamentos;
    }

    public Task<IReadOnlyCollection<IImpostoDto>> ListarNotaEntrada(ConciliacaoContabil conciliacaoContabil)
    {
        throw new System.NotImplementedException();
    }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarNotaSaida(ConciliacaoContabil conciliacaoContabil)
    {
        var notaSaida = _notaSaida.AsQueryable().AsNoTracking();
        var itemNotaSaida = _itemNotaSaida.AsQueryable().AsNoTracking();
        var lancamentoFiscais = _lancamentosFiscais.AsQueryable().AsNoTracking();
        var cabecalhoOutrosLancamentos = _cabecalhoOutrosLancamentos.AsQueryable().AsNoTracking();

        var notaSaidaSaida = from e in notaSaida.FiltrarPeriodoContabil(conciliacaoContabil)
            join c in itemNotaSaida on e.LegacyId equals c.IdNotaSaida
            where c.ValorIcmsSt > 0
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
                Valor = g.Sum(x => x.ValorIcmsSt.Value) * -1,
                Codigo = g.Key.CodigoCliente,
                LegacyCompanyId = g.Key.LegacyCompanyId,
                Parcela = "1",
            };

        var outrosNotaSaidaSaida = from l in lancamentoFiscais
            join c in cabecalhoOutrosLancamentos.FiltrarEmpresaContabil(conciliacaoContabil) on l.LegacyIdCabecalhoOutros equals c.Codigo
            where l.Imposto == "ICMSST" && l.CreditoDebito == "D"  &&
                  c.Ano == conciliacaoContabil.DataInicial.Year && c.Mes == conciliacaoContabil.DataInicial.Month 
            select new NotaSaidaDto
            {
                Data = new DateOnly(c.Ano, c.Mes, DateTime.DaysInMonth(c.Ano, c.Mes)),
                Documento = l.Historico,
                Valor = l.Valor * -1,
                LegacyCompanyId = 1,
                Parcela = "1",
            };

        
        var apuracoesFiscalNotaSaida = await notaSaidaSaida.AsNoTracking().ToListAsync();
        apuracoesFiscalNotaSaida.AddRange(await outrosNotaSaidaSaida.AsNoTracking().ToListAsync());

        return apuracoesFiscalNotaSaida;
    }
}