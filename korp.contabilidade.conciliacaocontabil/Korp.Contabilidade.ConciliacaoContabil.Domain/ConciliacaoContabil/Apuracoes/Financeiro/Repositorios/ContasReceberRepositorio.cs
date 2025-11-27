using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Financeiro.Repositorios;

public class ContasReceberRepositorio : IContasReceberFinanceiroRepositorio
{
    private readonly IRepository<ContasReceber> _contasReceber;

    public ContasReceberRepositorio(IRepository<ContasReceber> contasReceber)
    {
        _contasReceber = contasReceber;
    }
    
    public async Task<IReadOnlyCollection<IContasFinanceiroDto>> ListarContaRecebimento(ConciliacaoContabil conciliacaoContabil)
    {
        var contasReceber = _contasReceber.AsQueryable().AsNoTracking();

        var contasReceberEntrada = from c in contasReceber.FiltrarPeriodoEntradaFinanceiro(conciliacaoContabil)
            where c.Status != "I"
            select new ContasFinanceiroDto
            {
                Data = c.DataEntrada,
                Documento = c.Documento,
                RazaoSocial = c.RazaoSocial,
                Valor = c.Retencao.HasValue ? c.Valor - c.Retencao.Value : c.Valor,
                Codigo = c.Codigo,
                Status = c.Status,
                LegacyCompanyId = c.LegacyCompanyId,
                Parcela = c.Parcela,
                TipoValorApuracaoConciliacaoContabil = TipoValorApuracaoConciliacaoContabil.EntradaRecebimentoFinanceiro,
            };
        
        var contasReceberPagamento = from c in contasReceber.FiltrarPeriodoPagamentoFinanceiro(conciliacaoContabil)
            where c.Status == "R"
            select new ContasFinanceiroDto
            {
                Data = c.DataPagamento.Value,
                Documento = c.Documento,
                RazaoSocial = c.RazaoSocial,
                Valor = (c.Retencao.HasValue ? c.Valor - c.Retencao.Value : c.Valor) * -1,
                Codigo = c.Codigo,
                Status = c.Status,
                LegacyCompanyId = c.LegacyCompanyId,
                Parcela = c.Parcela,
                TipoValorApuracaoConciliacaoContabil = TipoValorApuracaoConciliacaoContabil.RecebimentoFinanceiro
            };
        
        var apuracoesContasReceber = await contasReceberEntrada.AsNoTracking().ToListAsync();
        apuracoesContasReceber.AddRange(await contasReceberPagamento.AsNoTracking().ToListAsync());
        
        return apuracoesContasReceber;
    }
}