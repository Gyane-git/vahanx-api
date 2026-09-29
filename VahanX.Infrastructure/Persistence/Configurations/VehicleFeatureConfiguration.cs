using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for VehicleFeature.
/// </summary>
public class VehicleFeatureConfiguration : BaseEntityConfiguration<VehicleFeature>
{
    public override void Configure(EntityTypeBuilder<VehicleFeature> builder)
    {
        base.Configure(builder);

        builder.ToTable("VehicleFeatures");

        builder.Property(e => e.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(e => e.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.FeatureGroup)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.DisplayOrder)
            .HasDefaultValue(0);

        builder.HasIndex(e => e.Code)
            .IsUnique();

        builder.HasIndex(e => e.FeatureGroup);

        builder.HasIndex(e => e.IsActive);
    }
}
