using Microsoft.EntityFrameworkCore;
using NewtonCatalog.Api.Domain;

namespace NewtonCatalog.Api.Data;

public class CatalogueDbContext(DbContextOptions<CatalogueDbContext> options) : DbContext(options)
{
    public DbSet<VideoGame> Games => Set<VideoGame>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogueDbContext).Assembly);
    }
}
