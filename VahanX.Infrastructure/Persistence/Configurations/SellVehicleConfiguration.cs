using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for SellVehicle.
/// </summary>
public class SellVehicleConfiguration : BaseEntityConfiguration<SellVehicle>
{
    public override void Configure(EntityTypeBuilder<SellVehicle> builder)
    {
        base.Configure(builder);

        builder.ToTable("SellVehicles");

        builder.Property(e => e.MileageUnit)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(e => e.AskingPrice)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.Property(e => e.Description)
            .HasMaxLength(5000)
            .IsRequired(false);

        builder.Property(e => e.Condition)
            .IsRequired();

        builder.HasOne(e => e.SellRequest)
            .WithOne(e => e.SellVehicle)
            .HasForeignKey<SellVehicle>(e => e.SellRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Vehicle)
            .WithMany()
            .HasForeignKey(e => e.VehicleId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.VehicleType)
            .WithMany()
            .HasForeignKey(e => e.VehicleTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Brand)
            .WithMany()
            .HasForeignKey(e => e.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Model)
            .WithMany()
            .HasForeignKey(e => e.ModelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Variant)
            .WithMany()
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.FuelType)
            .WithMany()
            .HasForeignKey(e => e.FuelTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.TransmissionType)
            .WithMany()
            .HasForeignKey(e => e.TransmissionTypeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Location)
            .WithMany()
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.SellRequestId);
        builder.HasIndex(e => e.VehicleId);
    }
}
