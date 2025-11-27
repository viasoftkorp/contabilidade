using Korp.Contabil.Core.Domain.TiposItem;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.EntityFrameworkCore.Legacy;
using Viasoft.Core.Storage.Schema;

namespace Korp.Contabil.Core.Infrastructure.EntityFrameworkCore;

public class LegacyContabilDbContext : LegacyBaseDbContext
{
    public DbSet<TipoItem> TiposItem { get; set; }

    public LegacyContabilDbContext(DbContextOptions options, ISchemaNameProvider schemaNameProvider) : base(options, schemaNameProvider)
    {
    }
}
