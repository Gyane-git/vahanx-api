using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for SubscriptionPlanFeature.
/// </summary>
public class SubscriptionPlanFeatureConfiguration : BaseEntityConfiguration<SubscriptionPlanFeature>
{
    public override void Configure(EntityTypeBuilder<SubscriptionPlanFeature> builder)
    {
        base.Configure(builder);

        builder.ToTable("SubscriptionPlanFeatures");

        builder.Property(e => e.BooleanValue)
            .IsRequired(false);

        builder.Property(e => e.NumericValue)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.HasOne(e => e.SubscriptionPlan)
            .WithMany(e => e.PlanFeatures)
            .HasForeignKey(e => e.SubscriptionPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Feature)
            .WithMany()
            .HasForeignKey(e => e.FeatureId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.SubscriptionPlanId, e.FeatureId })
            .IsUnique();
    }
}
