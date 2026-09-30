using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ChargingStationConnector.
/// </summary>
public class ChargingStationConnectorConfiguration : BaseEntityConfiguration<ChargingStationConnector>
{
    public override void Configure(EntityTypeBuilder<ChargingStationConnector> builder)
    {
        base.Configure(builder);

        builder.ToTable("ChargingStationConnectors");

        builder.Property(e => e.PowerKw)
            .HasPrecision(8, 2)
            .IsRequired();

        builder.Property(e => e.Voltage)
            .HasPrecision(8, 2)
            .IsRequired(false);

        builder.Property(e => e.Amperage)
            .HasPrecision(8, 2)
            .IsRequired(false);

        builder.Property(e => e.Quantity)
            .HasDefaultValue(1);

        builder.Property(e => e.ChargingMode)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(e => e.ConnectorType)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired();

        builder.HasOne(e => e.ChargingStation)
            .WithMany(e => e.Connectors)
            .HasForeignKey(e => e.ChargingStationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.ChargingStationId);
        builder.HasIndex(e => e.ConnectorType);
        builder.HasIndex(e => e.Status);
    }
}
