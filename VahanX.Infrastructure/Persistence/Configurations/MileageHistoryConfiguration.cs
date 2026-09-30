using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for MileageHistory.
/// </summary>
public class MileageHistoryConfiguration : BaseEntityConfiguration<MileageHistory>
{
    public override void Configure(EntityTypeBuilder<MileageHistory> builder)
    {
        base.Configure(builder);

        builder.ToTable("MileageHistory");

        builder.Property(e => e.MileageUnit)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(e => e.RecordedAt)
            .IsRequired();

        builder.Property(e => e.Source)
            .IsRequired();

        builder.Property(e => e.Notes)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.HasOne(e => e.Vehicle)
            .WithMany()
            .HasForeignKey(e => e.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.VehicleId);
        builder.HasIndex(e => e.RecordedAt);
    }
}
