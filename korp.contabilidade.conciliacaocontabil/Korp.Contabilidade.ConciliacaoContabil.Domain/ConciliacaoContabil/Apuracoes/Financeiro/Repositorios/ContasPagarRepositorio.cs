using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Financeiro.Repositorios;

public class ContasPagarRepositorio : IContasPagarFinanceiroRepositorio
{
    private readonly IRepository<ContasPagar> _contasPagar;

    public ContasPagarRepositorio(IRepository<ContasPagar> contasPagar)
    {
        _contasPagar = contasPagar;
    }

    public async Task<IReadOnlyCollection<IContasFinanceiroDto>> ListarContaPagamento(ConciliacaoContabil conciliacaoContabil)
    {
        var contasPagar = _contasPagar.AsQueryable().AsNoTracking();

        var contasPagarOrigem = from c in contasPagar.FiltrarPeriodoEntradaFinanceiro(conciliacaoContabil)
            where c.Status != "I"
            select  new ContasPagar { LegacyId = c.LegacyId };
        var cPagarOrigem = await contasPagarOrigem.AsNoTracking().ToListAsync();
        var contasPagarRetencao = from c in contasPagar
            where c.Status != "I" && cPagarOrigem.Select(l => l.LegacyId).Contains(c.LegacyIdOrigem)
                && c.IdDctf > 0
            group c by new
            {   
                c.LegacyIdOrigem  
            } into g
            select new ContasPagar
            {
                LegacyIdOrigem = g.Key.LegacyIdOrigem,
                Valor = g.Sum(x => x.Valor)
                
            };
        
        var contasPagarEntrada = from c in contasPagar.FiltrarPeriodoEntradaFinanceiro(conciliacaoContabil)
            where c.Status != "I"
            let retencao = contasPagarRetencao.FirstOrDefault(r => r.LegacyIdOrigem == c.LegacyId)
            select new ContasFinanceiroDto
            {
                Data = c.DataEntrada,
                Documento = c.Documento,
                RazaoSocial = c.RazaoSocial,
                Valor = ((c.Retencao.HasValue ? c.Valor - c.Retencao.Value : c.Valor) 
                        + (retencao != null ? retencao.Valor : 0)) * -1, // Soma o valor do contasPagarRetencao, se houver correspondência
                Codigo = c.Codigo,
                Status = c.Status,
                LegacyCompanyId = c.LegacyCompanyId,
                TipoValorApuracaoConciliacaoContabil = TipoValorApuracaoConciliacaoContabil.EntradaPagamentoFinanceiro,
                Parcela = c.Parcela
            };
        
        var contasPagarPagamento = from c in contasPagar.FiltrarPeriodoPagamentoFinanceiro(conciliacaoContabil)
            where c.Status == "P"
            select new ContasFinanceiroDto
            {
                // ReSharper disable once PossibleInvalidOperationException
                // aqui pegamos somentes os pagos
                Data = c.DataPagamento.Value,
                Documento = c.Documento,
                RazaoSocial = c.RazaoSocial,
                Valor = c.Retencao.HasValue ? c.Valor - c.Retencao.Value : c.Valor,
                Codigo = c.Codigo,
                Status = c.Status,
                LegacyCompanyId = c.LegacyCompanyId,
                TipoValorApuracaoConciliacaoContabil = TipoValorApuracaoConciliacaoContabil.PagamentoFinanceiro,
                Parcela = c.Parcela,
            };
        
        var apuracoesContasPagar = await contasPagarEntrada.AsNoTracking().ToListAsync();
        apuracoesContasPagar.AddRange(await contasPagarPagamento.AsNoTracking().ToListAsync());
        
        return apuracoesContasPagar;
    }
}