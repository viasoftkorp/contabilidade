using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Financeiro;

public class ContaFinanceiroRepositorioAdapter: IContaFinanceiroRepositorio
{
    private readonly ContaTipoFinanceiro _contaTipoFinanceiro;
    private readonly IContasPagarFinanceiroRepositorio _pagarFinanceiroRepositorio;
    private readonly IContasReceberFinanceiroRepositorio _receberFinanceiroRepositorio;

    
    public ContaFinanceiroRepositorioAdapter(ContaTipoFinanceiro contaTipoFinanceiro, IContasPagarFinanceiroRepositorio pagarFinanceiroRepositorio, IContasReceberFinanceiroRepositorio receberFinanceiroRepositorio)
    {
        _contaTipoFinanceiro = contaTipoFinanceiro;
        _pagarFinanceiroRepositorio = pagarFinanceiroRepositorio;
        _receberFinanceiroRepositorio = receberFinanceiroRepositorio;
    }
    
    public Task<IReadOnlyCollection<IContasFinanceiroDto>> ListarContas(ConciliacaoContabil conciliacaoContabil)
    {
        return _contaTipoFinanceiro switch
        {
            ContaTipoFinanceiro.Pagamento => _pagarFinanceiroRepositorio.ListarContaPagamento(conciliacaoContabil),
            ContaTipoFinanceiro.Recebimento => _receberFinanceiroRepositorio.ListarContaRecebimento(conciliacaoContabil),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}