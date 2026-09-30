using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Location.
/// </summary>
public class LocationConfiguration : BaseEntityConfiguration<Location>
{
    public override void Configure(EntityTypeBuilder<Location> builder)
    {
        base.Configure(builder);

        builder.ToTable("Locations");

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Province)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.District)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.Municipality)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.Ward)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(e => e.StreetAddress)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.Latitude)
            .IsRequired(false);

        builder.Property(e => e.Longitude)
            .IsRequired(false);

        builder.HasIndex(e => e.Province);
        builder.HasIndex(e => e.District);
    }
}
