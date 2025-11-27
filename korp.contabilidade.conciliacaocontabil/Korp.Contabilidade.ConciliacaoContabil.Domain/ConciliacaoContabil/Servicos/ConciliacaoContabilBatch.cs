using System.Collections.Generic;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Viasoft.Core.EntityFrameworkCore.Context.DbContextProvider;
using Viasoft.Core.EntityFrameworkCore.PropertyNormalizer;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Data.PropertyNormalizer;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Servicos;

public class ConciliacaoContabilBatch: IConciliacaoContabilBatch, ITransientDependency
{
    private readonly IDbContextProviderFactory _contextProviderFactory;
    private readonly IApplyPropertyNormalizerInEntities _applyPropertyNormalizerInEntities;
    
    public ConciliacaoContabilBatch(IDbContextProviderFactory contextProviderFactory, IApplyPropertyNormalizerInEntities applyPropertyNormalizerInEntities)
    {
        _contextProviderFactory = contextProviderFactory;
        _applyPropertyNormalizerInEntities = applyPropertyNormalizerInEntities;
    }
    
    public async Task InserirBatch<TEntity>(List<TEntity> entidades) where TEntity: class
    {
       ApplyPropertyNormalizerInEntitiesForInsert(entidades);
       var dbContextProvider = _contextProviderFactory.CreateDbContextProviderFromEntityType(typeof(TEntity));
       var dbContext = dbContextProvider.GetDbContext();
       await DbContextBulkExtensions.BulkInsertAsync(dbContext, entidades, config =>
       {
           config.BatchSize = 100000;
           config.BulkCopyTimeout = 0;
       });
    }
    
    private void ApplyPropertyNormalizerInEntitiesForInsert<TEntity>(IEnumerable<TEntity> entities) where TEntity: class
    {
        var propertyNormalizers = new[] { new NormalizePropertiesStates { Entities = entities, State = EntityStateToBeNormalized.Creation } };
        _applyPropertyNormalizerInEntities.ApplyPropertyNormalizer(propertyNormalizers);
    }
}