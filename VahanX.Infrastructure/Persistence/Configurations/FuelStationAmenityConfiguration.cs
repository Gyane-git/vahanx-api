using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for FuelStationAmenity.
/// </summary>
public class FuelStationAmenityConfiguration : BaseEntityConfiguration<FuelStationAmenity>
{
    public override void Configure(EntityTypeBuilder<FuelStationAmenity> builder)
    {
        base.Configure(builder);

        builder.ToTable("FuelStationAmenities");

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

        builder.HasOne(e => e.FuelStation)
            .WithMany(e => e.Amenities)
            .HasForeignKey(e => e.FuelStationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.FuelStationId);
    }
}
