using Microsoft.EntityFrameworkCore;
using NewtonCatalog.Api.Generated;

namespace NewtonCatalog.Api.Data;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(game =>
        {
            game.Property(g => g.Platform).HasConversion<string>().HasMaxLength(30);
            game.Property(g => g.Genre).HasConversion<string>().HasMaxLength(30);
            game.Property(g => g.Price).HasPrecision(18, 2);
        });
    }
}
