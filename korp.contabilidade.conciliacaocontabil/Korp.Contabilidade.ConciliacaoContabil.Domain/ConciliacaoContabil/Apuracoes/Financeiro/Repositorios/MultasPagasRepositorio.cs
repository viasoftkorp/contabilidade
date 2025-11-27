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

public class MultasPagasRepositorio: IContasPagarFinanceiroRepositorio
{
    private readonly IRepository<ContasPagar> _contasPagar;

    public MultasPagasRepositorio(IRepository<ContasPagar> contasPagar)
    {
        _contasPagar = contasPagar;
    }

    public async Task<IReadOnlyCollection<IContasFinanceiroDto>> ListarContaPagamento(ConciliacaoContabil conciliacaoContabil)
    {
        var contasPagar = _contasPagar.AsQueryable().AsNoTracking();
        
        var contasPagarPagamento = from c in contasPagar.FiltrarPeriodoPagamentoFinanceiro(conciliacaoContabil)
            where c.Status == "P" && c.Multa > 0
            select new ContasFinanceiroDto
            {
                // ReSharper disable once PossibleInvalidOperationException
                // aqui pegamos somentes os pagos
                Data = c.DataPagamento.Value,
                Documento = c.Documento,
                RazaoSocial = c.RazaoSocial,
                Valor = c.Multa * -1,
                Codigo = c.Codigo,
                Status = c.Status,
                LegacyCompanyId = c.LegacyCompanyId,
                TipoValorApuracaoConciliacaoContabil = TipoValorApuracaoConciliacaoContabil.MultasPagamentoFinanceiro,
                Parcela = c.Parcela,
            };
        
        var apuracoesContasPagar = await contasPagarPagamento.AsNoTracking().ToListAsync();
        
        return apuracoesContasPagar;
    }
}