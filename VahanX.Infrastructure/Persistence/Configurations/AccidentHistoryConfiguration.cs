using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for AccidentHistory.
/// </summary>
public class AccidentHistoryConfiguration : BaseEntityConfiguration<AccidentHistory>
{
    public override void Configure(EntityTypeBuilder<AccidentHistory> builder)
    {
        base.Configure(builder);

        builder.ToTable("AccidentHistory");

        builder.Property(e => e.Description)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.RepairCost)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.Property(e => e.Severity)
            .IsRequired();

        builder.Property(e => e.RepairStatus)
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
    }
}
