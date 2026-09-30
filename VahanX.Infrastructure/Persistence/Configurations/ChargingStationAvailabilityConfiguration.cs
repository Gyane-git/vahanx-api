using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ChargingStationAvailability.
/// </summary>
public class ChargingStationAvailabilityConfiguration : BaseEntityConfiguration<ChargingStationAvailability>
{
    public override void Configure(EntityTypeBuilder<ChargingStationAvailability> builder)
    {
        base.Configure(builder);

        builder.ToTable("ChargingStationAvailability");

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        builder.HasOne(e => e.ChargingStation)
            .WithMany(e => e.Availability)
            .HasForeignKey(e => e.ChargingStationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Connector)
            .WithMany()
            .HasForeignKey(e => e.ConnectorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.ChargingStationId);
        builder.HasIndex(e => e.ConnectorId);
        builder.HasIndex(e => e.Status);
    }
}
