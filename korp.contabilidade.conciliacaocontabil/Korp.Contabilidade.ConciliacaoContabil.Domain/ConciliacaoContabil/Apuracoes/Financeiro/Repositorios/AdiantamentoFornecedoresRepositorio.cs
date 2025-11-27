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

public class AdiantamentoFornecedoresRepositorio: IContasPagarFinanceiroRepositorio
{
    private readonly IRepository<ContasPagar> _contasPagar;

    public AdiantamentoFornecedoresRepositorio(IRepository<ContasPagar> contasPagar)
    {
        _contasPagar = contasPagar;
    }

    public async Task<IReadOnlyCollection<IContasFinanceiroDto>> ListarContaPagamento(ConciliacaoContabil conciliacaoContabil)
    {
        var contasPagar = _contasPagar.AsQueryable().AsNoTracking();

        var contasPagarEntrada = from c in contasPagar.FiltrarPeriodoEntradaFinanceiro(conciliacaoContabil)
            where c.Status != "I" && (c.Adiantamento ?? false) == true
            select new ContasFinanceiroDto
            {
                Data = c.DataEntrada,
                Documento = c.Documento,
                RazaoSocial = c.RazaoSocial,
                Valor = c.Retencao.HasValue ? c.Valor - c.Retencao.Value : c.Valor,
                Codigo = c.Codigo,
                Status = c.Status,
                LegacyCompanyId = c.LegacyCompanyId,
                Adiantamento = true,
                TipoValorApuracaoConciliacaoContabil = TipoValorApuracaoConciliacaoContabil.EntradaPagamentoFinanceiro,
                Parcela = c.Parcela
            };
        
        var contasPagarPagamento = from c in contasPagar.FiltrarPeriodoPagamentoFinanceiro(conciliacaoContabil)
            where c.Status == "P" && (c.Adiantamento ?? false) == true
            select new ContasFinanceiroDto
            {
                // ReSharper disable once PossibleInvalidOperationException
                // aqui pegamos somentes os pagos
                Data = c.DataPagamento.Value,
                Documento = c.Documento,
                RazaoSocial = c.RazaoSocial,
                Valor = (c.Retencao.HasValue ? c.Valor - c.Retencao.Value : c.Valor) * -1,
                Codigo = c.Codigo,
                Status = c.Status,
                LegacyCompanyId = c.LegacyCompanyId,
                Adiantamento = true,
                TipoValorApuracaoConciliacaoContabil = TipoValorApuracaoConciliacaoContabil.PagamentoFinanceiro,
                Parcela = c.Parcela,
            };
        
        var apuracoesContasPagar = await contasPagarEntrada.AsNoTracking().ToListAsync();
        apuracoesContasPagar.AddRange(await contasPagarPagamento.AsNoTracking().ToListAsync());
        
        return apuracoesContasPagar;
    }
}