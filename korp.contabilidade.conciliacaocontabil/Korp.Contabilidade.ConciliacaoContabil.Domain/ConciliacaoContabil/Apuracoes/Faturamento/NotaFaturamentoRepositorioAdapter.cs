using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Faturamento;

public class NotaFaturamentoRepositorioAdapter: INotaFaturamentoRepositorio
{
    private readonly IFaturamentoRepositorio _faturamentoRepositorio;
    
    public NotaFaturamentoRepositorioAdapter(IFaturamentoRepositorio faturamentoRepositorio)
    {
        _faturamentoRepositorio = faturamentoRepositorio;
    }
    
    public Task<IReadOnlyCollection<IImpostoDto>> ListarNotasFiscais(ConciliacaoContabil conciliacaoContabil)
    {
        return _faturamentoRepositorio.ListarNota(conciliacaoContabil);
    }
}