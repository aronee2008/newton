namespace NewtonCatalog.Api.Data.DataMigrations;

public static class DataMigrationRunner
{
    private static readonly IDataMigration[] Migrations =
    [
        new DM1_SeedInitialCatalogue()
    ];

    public static async Task RunAsync(CatalogueDbContext db, CancellationToken ct = default)
    {
        foreach (var migration in Migrations.OrderBy(m => m.Version))
        {
            await migration.MigrateAsync(db, ct);
        }
    }
}
