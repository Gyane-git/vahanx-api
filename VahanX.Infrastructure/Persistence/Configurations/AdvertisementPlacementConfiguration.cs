using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for AdvertisementPlacement.
/// </summary>
public class AdvertisementPlacementConfiguration : BaseEntityConfiguration<AdvertisementPlacement>
{
    public override void Configure(EntityTypeBuilder<AdvertisementPlacement> builder)
    {
        base.Configure(builder);

        builder.ToTable("AdvertisementPlacements");

        builder.Property(e => e.Code)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(e => e.PlacementType)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(e => e.Code)
            .IsUnique();

        builder.HasIndex(e => e.IsActive);
    }
}
