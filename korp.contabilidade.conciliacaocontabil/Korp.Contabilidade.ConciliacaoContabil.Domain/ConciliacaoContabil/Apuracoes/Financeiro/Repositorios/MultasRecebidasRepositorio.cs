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

public class MultasRecebidasRepositorio: IContasReceberFinanceiroRepositorio
{
    private readonly IRepository<ContasReceber> _contasReceber;

    public MultasRecebidasRepositorio(IRepository<ContasReceber> contasReceber)
    {
        _contasReceber = contasReceber;
    }
    

    public async Task<IReadOnlyCollection<IContasFinanceiroDto>> ListarContaRecebimento(ConciliacaoContabil conciliacaoContabil)
    {
        var contasReceber = _contasReceber.AsQueryable().AsNoTracking();
        
        var contasReceberPagamento = from c in contasReceber.FiltrarPeriodoPagamentoFinanceiro(conciliacaoContabil)
            where c.Status == "R" && c.Multa > 0
            select new ContasFinanceiroDto
            {
                Data = c.DataPagamento.Value,
                Documento = c.Documento,
                RazaoSocial = c.RazaoSocial,
                Valor = c.Multa,
                Codigo = c.Codigo,
                Status = c.Status,
                LegacyCompanyId = c.LegacyCompanyId,
                Parcela = c.Parcela,
                TipoValorApuracaoConciliacaoContabil = TipoValorApuracaoConciliacaoContabil.MultasRecebimentoFinanceiro
            };
        
        var apuracoesContasReceber = await contasReceberPagamento.AsNoTracking().ToListAsync();
        
        return apuracoesContasReceber;
    }
}