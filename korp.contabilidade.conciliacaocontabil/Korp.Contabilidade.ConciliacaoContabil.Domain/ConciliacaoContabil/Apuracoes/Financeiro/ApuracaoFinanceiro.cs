using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Financeiro;

public class ApuracaoFinanceiro: IApuracao
{
    private readonly IContaFinanceiroRepositorio _repositorio;
    
    public ApuracaoFinanceiro(IContaFinanceiroRepositorio repositorio)
    {
        _repositorio = repositorio;
    }
    
    public async Task<List<ConciliacaoContabilApuracao>> Apurar(ConciliacaoContabil conciliacaoContabil)
    {
        var contas = await _repositorio.ListarContas(conciliacaoContabil);
        var apuracoesAgrupada = ApurarContas(conciliacaoContabil, contas);
        var apuracoes = new List<ConciliacaoContabilApuracao>(contas.Count);

        apuracoes.AddRange(apuracoesAgrupada);
        
        return apuracoes;
    }
    
    private IEnumerable<ConciliacaoContabilApuracao> ApurarContas(ConciliacaoContabil conciliacaoContabil, IReadOnlyCollection<IContasFinanceiroDto> contas)
    {
        var contasAgrupadasEntrada = contas.GroupBy(l => new { l.Documento, l.LegacyCompanyId, l.Codigo, l.Data, l.TipoValorApuracaoConciliacaoContabil })
            .Select(l => new
            {
                ValorAgrupado = l.Sum(v => v.Valor),
                Itens = l.ToList()
            });
        
        foreach (var contasAgrupada in contasAgrupadasEntrada)
        {
            var contasApurada = RetornarApuracao(conciliacaoContabil, contasAgrupada.Itens, contasAgrupada.ValorAgrupado);
            foreach (var conta in contasApurada)
            {
                yield return conta;
            }
        }
    }

    private IEnumerable<ConciliacaoContabilApuracao> RetornarApuracao(ConciliacaoContabil conciliacaoContabil,
        IReadOnlyCollection<IContasFinanceiroDto> contas, decimal valorAgrupado)
    {
        foreach (var conta in contas)
        {
            var apuracao = new ConciliacaoContabilApuracao
            {
                LegacyCompanyId = conta.LegacyCompanyId,
                Documento = conta.Documento,
                Parcela = conta.Parcela,
                CodigoFornecedorCliente = conta.Codigo,
                RazaoSocialFornecedorCliente = conta.RazaoSocial,
                Conciliado = false,
                IdConciliacaoContabil = conciliacaoContabil.LegacyId,
                Data = conta.Data,
                TipoValorApuracao = conta.TipoValorApuracaoConciliacaoContabil,
                Valor = conta.Valor
            };

            apuracao.DescricaoTipoValorApuracao = apuracao.TipoValorApuracao.Descricao();
            if (conta.TipoValorApuracaoConciliacaoContabil ==
                TipoValorApuracaoConciliacaoContabil.ExtratoFinanceiroPagamento ||
                conta.TipoValorApuracaoConciliacaoContabil ==
                TipoValorApuracaoConciliacaoContabil.ExtratoFinanceiroRecebimento)
            {
                apuracao.GerarChaveConciliacao(conta.Valor); 
            }
            else
            {
                apuracao.GerarChaveConciliacao(conta.Adiantamento.HasValue ? conta.Adiantamento.Value ?  conta.Valor : valorAgrupado : valorAgrupado);
            }
            
            
            yield return apuracao;
        }   
    }
}