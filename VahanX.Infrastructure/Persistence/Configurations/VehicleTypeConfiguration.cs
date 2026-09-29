using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for VehicleType.
/// </summary>
public class VehicleTypeConfiguration : BaseEntityConfiguration<VehicleType>
{
    public override void Configure(EntityTypeBuilder<VehicleType> builder)
    {
        base.Configure(builder);

        builder.ToTable("VehicleTypes");

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

        builder.HasIndex(e => e.Code)
            .IsUnique();

        builder.HasIndex(e => e.Name);

        builder.HasIndex(e => e.IsActive);
    }
}
