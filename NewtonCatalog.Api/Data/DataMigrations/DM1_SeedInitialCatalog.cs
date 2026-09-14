using Microsoft.EntityFrameworkCore;

namespace NewtonCatalog.Api.Data.DataMigrations;

public static class DM1_SeedInitialCatalog
{
    public static async Task MigrateAsync(CatalogDbContext db)
    {
        if (await db.Games.AnyAsync())
        {
            return;
        }

        db.Games.AddRange(SeedGames.All);
        await db.SaveChangesAsync();
    }
}
