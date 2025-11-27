using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos;

public class ApuracaoImposto: IApuracao
{
    private readonly INotaFiscalImpostoRepositorio _repositorio;
    private readonly TipoValorApuracaoConciliacaoContabil _tipoValorApuracao;
    
    public ApuracaoImposto(INotaFiscalImpostoRepositorio repositorio, TipoValorApuracaoConciliacaoContabil tipoValorApuracao)
    {
        _repositorio = repositorio;
        _tipoValorApuracao = tipoValorApuracao;
    }
    
    public async Task<List<ConciliacaoContabilApuracao>> Apurar(ConciliacaoContabil conciliacaoContabil)
    {
        var notas = await _repositorio.ListarNotasFiscais(conciliacaoContabil);
        
        var apuracoes = new List<ConciliacaoContabilApuracao>(notas.Count);
        foreach (var nota in notas)
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
                TipoValorApuracao = _tipoValorApuracao,
                Valor = nota.Valor
            };

            apuracao.DescricaoTipoValorApuracao = apuracao.TipoValorApuracao.Descricao();
            apuracao.GerarChaveConciliacao(apuracao.Valor);
            
            apuracoes.Add(apuracao);
        }
        
        return apuracoes;
    }
}