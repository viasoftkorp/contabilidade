using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Patrimonial.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Patrimonial;

public class BemPatrimonialRepositorioAdapter: IBemPatrimonialRepositorio
{
    private readonly IPatrimonialRepositorio _patrimonialRepositorio;
    
    public BemPatrimonialRepositorioAdapter(IPatrimonialRepositorio patrimonialRepositorio)
    {
        _patrimonialRepositorio = patrimonialRepositorio;
    }
    
    public Task<IReadOnlyCollection<PatrimonialDto>> ListarValorBensPatrimonial(ConciliacaoContabil conciliacaoContabil)
    {
        return _patrimonialRepositorio.ListarBensPatrimonial(conciliacaoContabil);
    }
}