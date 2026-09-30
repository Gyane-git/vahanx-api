using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for FuelStationFuelType.
/// </summary>
public class FuelStationFuelTypeConfiguration : BaseEntityConfiguration<FuelStationFuelType>
{
    public override void Configure(EntityTypeBuilder<FuelStationFuelType> builder)
    {
        base.Configure(builder);

        builder.ToTable("FuelStationFuelTypes");

        builder.Property(e => e.FuelType)
            .IsRequired();

        builder.Property(e => e.IsAvailable)
            .HasDefaultValue(true);

        builder.HasOne(e => e.FuelStation)
            .WithMany(e => e.FuelTypes)
            .HasForeignKey(e => e.FuelStationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.FuelStationId, e.FuelType })
            .IsUnique();
    }
}
