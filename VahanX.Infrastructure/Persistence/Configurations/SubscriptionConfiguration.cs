using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Subscription.
/// </summary>
public class SubscriptionConfiguration : BaseEntityConfiguration<Subscription>
{
    public override void Configure(EntityTypeBuilder<Subscription> builder)
    {
        base.Configure(builder);

        builder.ToTable("Subscriptions");

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.Property(e => e.AutoRenew)
            .HasDefaultValue(true);

        builder.Property(e => e.CancellationReason)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.ExternalSubscriptionReference)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.HasOne(e => e.SubscriptionPlan)
            .WithMany()
            .HasForeignKey(e => e.SubscriptionPlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Seller)
            .WithMany()
            .HasForeignKey(e => e.SellerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Dealer)
            .WithMany()
            .HasForeignKey(e => e.DealerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.SellerId);
        builder.HasIndex(e => e.DealerId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.EndDate);
    }
}
