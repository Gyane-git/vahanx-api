using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for SubscriptionPlan.
/// </summary>
public class SubscriptionPlanConfiguration : BaseEntityConfiguration<SubscriptionPlan>
{
    public override void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        base.Configure(builder);

        builder.ToTable("SubscriptionPlans");

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.Currency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(e => e.TargetType)
            .IsRequired();

        builder.Property(e => e.BillingCycle)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.IsPublic)
            .HasDefaultValue(true);

        builder.HasIndex(e => e.Code)
            .IsUnique();

        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => e.TargetType);
    }
}
