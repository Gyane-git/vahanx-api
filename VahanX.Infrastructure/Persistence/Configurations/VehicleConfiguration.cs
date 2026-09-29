using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Vehicle.
/// </summary>
public class VehicleConfiguration : BaseEntityConfiguration<Vehicle>
{
    public override void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        base.Configure(builder);

        builder.ToTable("Vehicles");

        builder.Property(e => e.Description)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(e => e.Variant)
            .WithMany(e => e.Vehicles)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.BodyType)
            .WithMany(e => e.Vehicles)
            .HasForeignKey(e => e.BodyTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.FuelType)
            .WithMany(e => e.Vehicles)
            .HasForeignKey(e => e.FuelTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.TransmissionType)
            .WithMany(e => e.Vehicles)
            .HasForeignKey(e => e.TransmissionTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.DriveType)
            .WithMany(e => e.Vehicles)
            .HasForeignKey(e => e.DriveTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.EngineType)
            .WithMany(e => e.Vehicles)
            .HasForeignKey(e => e.EngineTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.VariantId);

        builder.HasIndex(e => e.BodyTypeId);

        builder.HasIndex(e => e.FuelTypeId);

        builder.HasIndex(e => e.TransmissionTypeId);

        builder.HasIndex(e => e.DriveTypeId);

        builder.HasIndex(e => e.EngineTypeId);

        builder.HasIndex(e => e.IsActive);
    }
}
