using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for VehicleSpecification.
/// </summary>
public class VehicleSpecificationConfiguration : BaseEntityConfiguration<VehicleSpecification>
{
    public override void Configure(EntityTypeBuilder<VehicleSpecification> builder)
    {
        base.Configure(builder);

        builder.ToTable("VehicleSpecifications");

        builder.Property(e => e.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(e => e.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.Unit)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.DataType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.SpecGroup)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.DisplayOrder)
            .HasDefaultValue(0);

        builder.HasIndex(e => e.Code)
            .IsUnique();

        builder.HasIndex(e => e.SpecGroup);

        builder.HasIndex(e => e.IsActive);
    }
}
