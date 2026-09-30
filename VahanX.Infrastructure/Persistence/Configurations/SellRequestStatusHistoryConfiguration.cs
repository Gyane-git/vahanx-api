using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for SellRequestStatusHistory.
/// </summary>
public class SellRequestStatusHistoryConfiguration : BaseEntityConfiguration<SellRequestStatusHistory>
{
    public override void Configure(EntityTypeBuilder<SellRequestStatusHistory> builder)
    {
        base.Configure(builder);

        builder.ToTable("SellRequestStatusHistory");

        builder.Property(e => e.Reason)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(e => e.ChangedAt)
            .IsRequired();

        builder.Property(e => e.NewStatus)
            .IsRequired();

        builder.HasOne(e => e.SellRequest)
            .WithMany(e => e.StatusHistory)
            .HasForeignKey(e => e.SellRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.SellRequestId);
        builder.HasIndex(e => e.ChangedAt);
    }
}
