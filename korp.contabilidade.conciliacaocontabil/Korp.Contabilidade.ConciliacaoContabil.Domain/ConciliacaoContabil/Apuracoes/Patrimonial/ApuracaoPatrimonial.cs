using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Patrimonial;

public class ApuracaoPatrimonial: IApuracao
{
    private readonly IBemPatrimonialRepositorio _repositorio;
    
    public ApuracaoPatrimonial(IBemPatrimonialRepositorio repositorio)
    {
        _repositorio = repositorio;
    }
    
    public async Task<List<ConciliacaoContabilApuracao>> Apurar(ConciliacaoContabil conciliacaoContabil)
    {
        var bensPatrimonial = await _repositorio.ListarValorBensPatrimonial(conciliacaoContabil);
        var apuracoes = new List<ConciliacaoContabilApuracao>(bensPatrimonial.Count);
        foreach (var bem in bensPatrimonial)
        {
            var apuracao = new ConciliacaoContabilApuracao
            {
                LegacyCompanyId = bem.LegacyCompanyId,
                Documento = bem.Documento,
                Parcela = bem.Parcela,
                CodigoFornecedorCliente = bem.Codigo,
                RazaoSocialFornecedorCliente = bem.RazaoSocial,
                Conciliado = false,
                IdConciliacaoContabil = conciliacaoContabil.LegacyId,
                Data = bem.Data,
                TipoValorApuracao = bem.TipoValorApuracao,
                Valor = bem.Valor
            };

            apuracao.DescricaoTipoValorApuracao = apuracao.TipoValorApuracao.Descricao();
            apuracao.GerarChaveConciliacao(apuracao.Valor);
            
            apuracoes.Add(apuracao);
        }
        
        return apuracoes;
    }
}