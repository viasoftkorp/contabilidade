using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Faturamento.Repositorio;

public class ValorBrutoVendasFaturamento: IFaturamentoRepositorio
{
    private readonly IRepository<FaturamentoNotaFiscal> _faturamentoNotaFiscal;
    private readonly IRepository<FaturamentoNotaFiscalNfce> _faturamentoNotaFiscalNfce;
    private readonly IRepository<FaturamentoNotaFiscalNfceCaixa> _faturamentoNotaFiscalNfceCaixa;

    public ValorBrutoVendasFaturamento(IRepository<FaturamentoNotaFiscal> faturamentoNotaFiscal, IRepository<FaturamentoNotaFiscalNfce> faturamentoNotaFiscalNfce, IRepository<FaturamentoNotaFiscalNfceCaixa> faturamentoNotaFiscalNfceCaixa)
    {
        _faturamentoNotaFiscal = faturamentoNotaFiscal;
        _faturamentoNotaFiscalNfce = faturamentoNotaFiscalNfce;
        _faturamentoNotaFiscalNfceCaixa = faturamentoNotaFiscalNfceCaixa;
    }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarNota(ConciliacaoContabil conciliacaoContabil)
    {
        var notaFiscal = _faturamentoNotaFiscal.AsQueryable().AsNoTracking();
        var notaFiscalNfce = _faturamentoNotaFiscalNfce.AsQueryable().AsNoTracking();
        var notaFiscalNfceCaixa = _faturamentoNotaFiscalNfceCaixa.AsQueryable().AsNoTracking();
        var empresas = conciliacaoContabil.ListarEmpresas();
        var notasEmitidas = from n in notaFiscal.FiltrarPeriodoContabil(conciliacaoContabil)
            where n.Situacao == "IMPRESSA" 
            select new NotaSaidaDto
            {
                Data = n.Data,
                Documento = n.Documento.ToString(),
                RazaoSocial = n.RazaoSocialCliente,
                Valor = n.Devolucao ? n.Valor * -1 : n.Valor,
                Codigo = n.CodigoCliente,
                LegacyCompanyId = n.LegacyCompanyId,
                Parcela = "1"
            };
        
        var notasEmitidasNfce = from n in notaFiscalNfce
            join c in notaFiscalNfceCaixa on n.LegacyFaturamentoCaixaId equals c.LegacyId
            where n.Situacao == "EMITIDA" 
                  && n.Data >= conciliacaoContabil.DataInicial.ToDateTime(TimeOnly.MinValue) && n.Data <= conciliacaoContabil.DataFinal.ToDateTime(TimeOnly.MinValue)
                  && empresas.Contains(c.LegacyCompanyId)
            select new NotaSaidaDto
            {
                Data =  DateOnly.FromDateTime(n.Data),
                Documento = n.Documento.ToString(),
                RazaoSocial = n.RazaoSocialCliente,
                Valor = n.Valor,
                Codigo = n.CodigoCliente,
                LegacyCompanyId = c.LegacyCompanyId,
                Parcela = "1"
            };
        
        var apuracoesContasReceber = await notasEmitidas.AsNoTracking().ToListAsync();
        apuracoesContasReceber.AddRange(await notasEmitidasNfce.AsNoTracking().ToListAsync());
        
        return apuracoesContasReceber;
    }
}