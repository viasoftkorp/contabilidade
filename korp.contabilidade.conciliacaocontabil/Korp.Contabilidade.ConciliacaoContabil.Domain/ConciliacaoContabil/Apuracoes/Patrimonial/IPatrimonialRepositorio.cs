using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Patrimonial.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Patrimonial;

public interface IPatrimonialRepositorio
{
    Task<IReadOnlyCollection<PatrimonialDto>> ListarBensPatrimonial(ConciliacaoContabil conciliacaoContabil);
}