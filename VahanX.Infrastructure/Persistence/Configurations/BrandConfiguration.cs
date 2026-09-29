using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Brand.
/// </summary>
public class BrandConfiguration : BaseEntityConfiguration<Brand>
{
    public override void Configure(EntityTypeBuilder<Brand> builder)
    {
        base.Configure(builder);

        builder.ToTable("Brands");

        builder.Property(e => e.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(e => e.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(e => e.LogoMediaId)
            .IsRequired(false);

        builder.Property(e => e.CountryOfOrigin)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.DisplayOrder)
            .HasDefaultValue(0);

        builder.HasIndex(e => e.Code)
            .IsUnique();

        builder.HasIndex(e => e.Name);

        builder.HasIndex(e => e.IsActive);
    }
}
