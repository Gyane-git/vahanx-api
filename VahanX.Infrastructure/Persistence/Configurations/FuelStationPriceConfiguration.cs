using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for FuelStationPrice.
/// </summary>
public class FuelStationPriceConfiguration : BaseEntityConfiguration<FuelStationPrice>
{
    public override void Configure(EntityTypeBuilder<FuelStationPrice> builder)
    {
        base.Configure(builder);

        builder.ToTable("FuelStationPrices");

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

        builder.Property(e => e.IsCurrent)
            .HasDefaultValue(true);

        builder.HasOne(e => e.FuelStation)
            .WithMany(e => e.Prices)
            .HasForeignKey(e => e.FuelStationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.FuelType)
            .WithMany()
            .HasForeignKey(e => e.FuelStationFuelTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.FuelStationId);
        builder.HasIndex(e => e.IsCurrent);
        builder.HasIndex(e => e.EffectiveFrom);
    }
}
