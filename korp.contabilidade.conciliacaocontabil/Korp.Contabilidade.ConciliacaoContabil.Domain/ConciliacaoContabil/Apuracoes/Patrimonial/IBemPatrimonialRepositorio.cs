using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Patrimonial.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Patrimonial;

public interface IBemPatrimonialRepositorio
{
    Task<IReadOnlyCollection<PatrimonialDto>> ListarValorBensPatrimonial(ConciliacaoContabil conciliacaoContabil);
}