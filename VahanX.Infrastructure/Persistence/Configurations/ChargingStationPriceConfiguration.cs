using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ChargingStationPrice.
/// </summary>
public class ChargingStationPriceConfiguration : BaseEntityConfiguration<ChargingStationPrice>
{
    public override void Configure(EntityTypeBuilder<ChargingStationPrice> builder)
    {
        base.Configure(builder);

        builder.ToTable("ChargingStationPrices");

        builder.Property(e => e.Price)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(e => e.Currency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(e => e.Unit)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.EffectiveFrom)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.PricingType)
            .IsRequired();

        builder.HasOne(e => e.ChargingStation)
            .WithMany(e => e.Prices)
            .HasForeignKey(e => e.ChargingStationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.ChargingStationId);
        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => e.EffectiveFrom);
    }
}
