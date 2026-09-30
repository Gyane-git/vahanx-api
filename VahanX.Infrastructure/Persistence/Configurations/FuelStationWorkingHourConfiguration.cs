using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for FuelStationWorkingHour.
/// </summary>
public class FuelStationWorkingHourConfiguration : BaseEntityConfiguration<FuelStationWorkingHour>
{
    public override void Configure(EntityTypeBuilder<FuelStationWorkingHour> builder)
    {
        base.Configure(builder);

        builder.ToTable("FuelStationWorkingHours");

        builder.Property(e => e.DayOfWeek)
            .IsRequired();

        builder.Property(e => e.OpeningTime)
            .IsRequired();

        builder.Property(e => e.ClosingTime)
            .IsRequired();

        builder.Property(e => e.IsClosed)
            .HasDefaultValue(false);

        builder.HasOne(e => e.FuelStation)
            .WithMany(e => e.WorkingHours)
            .HasForeignKey(e => e.FuelStationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.FuelStationId, e.DayOfWeek });
    }
}
