using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for VehicleCategory.
/// </summary>
public class VehicleCategoryConfiguration : BaseEntityConfiguration<VehicleCategory>
{
    public override void Configure(EntityTypeBuilder<VehicleCategory> builder)
    {
        base.Configure(builder);

        builder.ToTable("VehicleCategories");

        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.DisplayOrder)
            .HasDefaultValue(0);

        builder.HasOne(e => e.VehicleType)
            .WithMany(e => e.Categories)
            .HasForeignKey(e => e.VehicleTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.VehicleTypeId);

        builder.HasIndex(e => e.Code)
            .IsUnique();

        builder.HasIndex(e => new { e.VehicleTypeId, e.Name })
            .IsUnique();
    }
}
