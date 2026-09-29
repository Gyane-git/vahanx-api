using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Variant.
/// </summary>
public class VariantConfiguration : BaseEntityConfiguration<Variant>
{
    public override void Configure(EntityTypeBuilder<Variant> builder)
    {
        base.Configure(builder);

        builder.ToTable("Variants");

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(e => e.ModelYearFrom)
            .IsRequired();

        builder.Property(e => e.ModelYearTo)
            .IsRequired(false);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.DisplayOrder)
            .HasDefaultValue(0);

        builder.HasOne(e => e.Generation)
            .WithMany(e => e.Variants)
            .HasForeignKey(e => e.GenerationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.GenerationId);

        builder.HasIndex(e => new { e.GenerationId, e.Code })
            .IsUnique();

        builder.HasIndex(e => e.ModelYearFrom);

        builder.HasIndex(e => e.IsActive);
    }
}
