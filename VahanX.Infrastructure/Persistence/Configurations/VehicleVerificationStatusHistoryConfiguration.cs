using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for VehicleVerificationStatusHistory.
/// </summary>
public class VehicleVerificationStatusHistoryConfiguration : BaseEntityConfiguration<VehicleVerificationStatusHistory>
{
    public override void Configure(EntityTypeBuilder<VehicleVerificationStatusHistory> builder)
    {
        base.Configure(builder);

        builder.ToTable("VehicleVerificationStatusHistory");

        builder.Property(e => e.OldStatus)
            .IsRequired();

        builder.Property(e => e.NewStatus)
            .IsRequired();

        builder.Property(e => e.ChangedAt)
            .IsRequired();

        builder.Property(e => e.Reason)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.HasOne(e => e.Verification)
            .WithMany(e => e.StatusHistory)
            .HasForeignKey(e => e.VerificationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.VerificationId);
        builder.HasIndex(e => e.ChangedAt);
    }
}
