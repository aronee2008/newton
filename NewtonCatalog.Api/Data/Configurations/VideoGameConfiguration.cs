using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewtonCatalog.Api.Domain;

namespace NewtonCatalog.Api.Data.Configurations;

public class VideoGameConfiguration : IEntityTypeConfiguration<VideoGame>
{
    public void Configure(EntityTypeBuilder<VideoGame> builder)
    {
        builder.ToTable("Games");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.Developer)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.Platform)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(g => g.Genre)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(g => g.Price)
            .HasPrecision(18, 2);

        builder.HasIndex(g => g.Title);
    }
}
