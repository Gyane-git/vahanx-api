using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ChargingStationAmenity.
/// </summary>
public class ChargingStationAmenityConfiguration : BaseEntityConfiguration<ChargingStationAmenity>
{
    public override void Configure(EntityTypeBuilder<ChargingStationAmenity> builder)
    {
        base.Configure(builder);

        builder.ToTable("ChargingStationAmenities");

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.IsAvailable)
            .HasDefaultValue(true);

        builder.HasOne(e => e.ChargingStation)
            .WithMany(e => e.Amenities)
            .HasForeignKey(e => e.ChargingStationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.ChargingStationId);
    }
}
