using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Faturamento;

public class ApuracaoFaturamento: IApuracao
{
    private readonly INotaFaturamentoRepositorio _repositorio;
    
    public ApuracaoFaturamento(INotaFaturamentoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }
    
    public async Task<List<ConciliacaoContabilApuracao>> Apurar(ConciliacaoContabil conciliacaoContabil)
    {
        var notasFiscais = await _repositorio.ListarNotasFiscais(conciliacaoContabil);
        var apuracoes = new List<ConciliacaoContabilApuracao>(notasFiscais.Count);
        foreach (var nota in notasFiscais)
        {
            var apuracao = new ConciliacaoContabilApuracao
            {
                LegacyCompanyId = nota.LegacyCompanyId,
                Documento = nota.Documento,
                Parcela = nota.Parcela,
                CodigoFornecedorCliente = nota.Codigo,
                RazaoSocialFornecedorCliente = nota.RazaoSocial,
                Conciliado = false,
                IdConciliacaoContabil = conciliacaoContabil.LegacyId,
                Data = nota.Data,
                TipoValorApuracao = TipoValorApuracaoConciliacaoContabil.ValorBrutoVendasFaturamento,
                Valor = nota.Valor
            };

            apuracao.DescricaoTipoValorApuracao = apuracao.TipoValorApuracao.Descricao();
            apuracao.GerarChaveConciliacao(apuracao.Valor);
            
            apuracoes.Add(apuracao);
        }  
        
        return apuracoes;
    }
}