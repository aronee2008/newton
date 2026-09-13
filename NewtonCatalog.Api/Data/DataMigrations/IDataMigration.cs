namespace NewtonCatalog.Api.Data.DataMigrations;

public interface IDataMigration
{
    int Version { get; }

    Task MigrateAsync(CatalogueDbContext db, CancellationToken ct);
}
