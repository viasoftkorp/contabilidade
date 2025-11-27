using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Pis.Debitar;

public class PisOutrosLancamentos: IPisDebitarRepositorio
{
    private readonly IRepository<OutrosLancamentosFiscais> _lancamentosFiscais;
    private readonly IRepository<CabecalhoOutrosLancamentosFiscais> _cabecalhoOutrosLancamentos;

    public PisOutrosLancamentos(IRepository<CabecalhoOutrosLancamentosFiscais> cabecalhoOutrosLancamentos, IRepository<OutrosLancamentosFiscais> lancamentosFiscais)
    {
        _cabecalhoOutrosLancamentos = cabecalhoOutrosLancamentos;
        _lancamentosFiscais = lancamentosFiscais;
    }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarImpostoDebitar(ConciliacaoContabil conciliacaoContabil)
    {
        var lancamentoFiscais = _lancamentosFiscais.AsQueryable().AsNoTracking();
        var cabecalhoOutrosLancamentos = _cabecalhoOutrosLancamentos.AsQueryable().AsNoTracking();

        var outrosNotaEntradaEntrada = from l in lancamentoFiscais
            join c in cabecalhoOutrosLancamentos.FiltrarEmpresaContabil(conciliacaoContabil) on l.LegacyIdCabecalhoOutros equals c.Codigo
            where l.Imposto == "PIS" && l.CreditoDebito == "D" &&
                  c.Ano == conciliacaoContabil.DataInicial.Year && c.Mes == conciliacaoContabil.DataInicial.Month
            select new ImpostoDto
            {
                Data = new DateOnly(c.Ano, c.Mes, DateTime.DaysInMonth(c.Ano, c.Mes)),
                Documento = l.Historico,
                Valor = l.Valor * -1,
                LegacyCompanyId = c.LegacyCompanyId,
                Parcela = "1",
            };

        return await outrosNotaEntradaEntrada.AsNoTracking().ToListAsync();
    }
}