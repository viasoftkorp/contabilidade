using System.Collections.Generic;
using System.Threading.Tasks;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Servicos;

public interface IConciliacaoContabilBatch
{
    Task InserirBatch<TEntity>(List<TEntity> entidades) where TEntity: class;
}