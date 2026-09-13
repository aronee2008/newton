using Microsoft.EntityFrameworkCore;

namespace NewtonCatalog.Api.Data.DataMigrations;

public class DM1_SeedInitialCatalogue : IDataMigration
{
    public int Version => 1;

    public async Task MigrateAsync(CatalogueDbContext db, CancellationToken ct)
    {
        if (await db.Games.AnyAsync(ct))
        {
            return;
        }

        db.Games.AddRange(SeedGames.All);
        await db.SaveChangesAsync(ct);
    }
}
