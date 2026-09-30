using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for SubscriptionChangeHistory.
/// </summary>
public class SubscriptionChangeHistoryConfiguration : BaseEntityConfiguration<SubscriptionChangeHistory>
{
    public override void Configure(EntityTypeBuilder<SubscriptionChangeHistory> builder)
    {
        base.Configure(builder);

        builder.ToTable("SubscriptionChangeHistory");

        builder.Property(e => e.ChangeType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Reason)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(e => e.ChangedAt)
            .IsRequired();

        builder.Property(e => e.NewStatus)
            .IsRequired();

        builder.HasOne(e => e.Subscription)
            .WithMany(e => e.ChangeHistory)
            .HasForeignKey(e => e.SubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.SubscriptionId);
        builder.HasIndex(e => e.ChangedAt);
    }
}
