using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Financeiro.Repositorios;

public class ExtratoRepositorio: IContasPagarFinanceiroRepositorio
{
    private readonly IRepository<Extrato> _extrato;
    private readonly IRepository<ContasReceber> _contasReceber;
    private readonly IRepository<ContasPagar> _contasPagar;

    public ExtratoRepositorio(IRepository<ContasReceber> contasReceber, IRepository<ContasPagar> contasPagar, IRepository<Extrato> extrato)
    {
        _contasReceber = contasReceber;
        _contasPagar = contasPagar;
        _extrato = extrato;
    }

    public async Task<IReadOnlyCollection<IContasFinanceiroDto>> ListarContaPagamento(ConciliacaoContabil conciliacaoContabil)
    {
        var extrato = _extrato.AsQueryable().AsNoTracking();
        var contasPagar = _contasPagar.AsQueryable().AsNoTracking();
        var contasReceber = _contasReceber.AsQueryable().AsNoTracking();  
        var empresas = conciliacaoContabil.ListarEmpresas();
        
        var extratoEntradaContasR = from e in extrato
            join c in contasReceber on e.LegacyId equals c.NumeroExtrato
            where e.Data >= conciliacaoContabil.DataInicial
                  && e.Data <= conciliacaoContabil.DataFinal
                  && empresas.Contains(c.LegacyCompanyId)
            select new ContasFinanceiroDto
            {
                Data = e.Data,
                Documento = c.Documento,
                RazaoSocial = c.RazaoSocial,
                Valor = e.Valor,
                Codigo = c.Codigo,
                Status = c.Status,
                LegacyCompanyId = c.LegacyCompanyId,
                Parcela = c.Parcela,
                Adiantamento = c.Adiantamento,
                TipoValorApuracaoConciliacaoContabil = TipoValorApuracaoConciliacaoContabil.ExtratoFinanceiroRecebimento
            };
        
        var extratoEntradaContasP = from e in extrato
            join c in contasPagar  on e.LegacyId equals c.NumeroExtrato
            where e.Data >= conciliacaoContabil.DataInicial
                  && e.Data <= conciliacaoContabil.DataFinal
                  && empresas.Contains(c.LegacyCompanyId)
            select new ContasFinanceiroDto
            {
                Data = e.Data,
                Documento = c.Documento,
                RazaoSocial = c.RazaoSocial,
                Valor = e.Valor * -1,
                Codigo = c.Codigo,
                Status = c.Status,
                LegacyCompanyId = c.LegacyCompanyId,
                Parcela = c.Parcela,
                Adiantamento = c.Adiantamento,
                TipoValorApuracaoConciliacaoContabil = TipoValorApuracaoConciliacaoContabil.ExtratoFinanceiroPagamento
            };
      
        var apuracoesExtrato = await extratoEntradaContasR.AsNoTracking().ToListAsync();
        apuracoesExtrato.AddRange(await extratoEntradaContasP.AsNoTracking().ToListAsync());

        return apuracoesExtrato;
    }
}