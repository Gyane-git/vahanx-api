using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for SubscriptionUsage.
/// </summary>
public class SubscriptionUsageConfiguration : BaseEntityConfiguration<SubscriptionUsage>
{
    public override void Configure(EntityTypeBuilder<SubscriptionUsage> builder)
    {
        base.Configure(builder);

        builder.ToTable("SubscriptionUsage");

        builder.Property(e => e.PeriodStart)
            .IsRequired();

        builder.Property(e => e.PeriodEnd)
            .IsRequired();

        builder.Property(e => e.LastCalculatedAt)
            .IsRequired();

        builder.HasOne(e => e.Subscription)
            .WithMany(e => e.Usage)
            .HasForeignKey(e => e.SubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Feature)
            .WithMany()
            .HasForeignKey(e => e.FeatureId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.SubscriptionId, e.FeatureId, e.PeriodStart })
            .IsUnique();
    }
}
