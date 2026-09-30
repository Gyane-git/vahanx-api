using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Feature.
/// </summary>
public class FeatureConfiguration : BaseEntityConfiguration<Feature>
{
    public override void Configure(EntityTypeBuilder<Feature> builder)
    {
        base.Configure(builder);

        builder.ToTable("Features");

        builder.Property(e => e.Code)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(e => e.Unit)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(e => e.FeatureType)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(e => e.Code)
            .IsUnique();

        builder.HasIndex(e => e.IsActive);
    }
}
