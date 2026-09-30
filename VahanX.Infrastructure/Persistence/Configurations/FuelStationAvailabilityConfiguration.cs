using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for FuelStationAvailability.
/// </summary>
public class FuelStationAvailabilityConfiguration : BaseEntityConfiguration<FuelStationAvailability>
{
    public override void Configure(EntityTypeBuilder<FuelStationAvailability> builder)
    {
        base.Configure(builder);

        builder.ToTable("FuelStationAvailability");

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        builder.HasOne(e => e.FuelStation)
            .WithMany(e => e.Availability)
            .HasForeignKey(e => e.FuelStationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.FuelType)
            .WithMany()
            .HasForeignKey(e => e.FuelStationFuelTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.FuelStationId);
        builder.HasIndex(e => e.Status);
    }
}
