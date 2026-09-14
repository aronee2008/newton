using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewtonCatalog.Api.Generated;

namespace NewtonCatalog.Api.Data.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.Property(g => g.Platform).HasConversion<string>().HasMaxLength(30);
        builder.Property(g => g.Genre).HasConversion<string>().HasMaxLength(30);
        builder.Property(g => g.Price).HasPrecision(18, 2);
    }
}
