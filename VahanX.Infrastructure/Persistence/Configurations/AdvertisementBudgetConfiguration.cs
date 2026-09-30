using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for AdvertisementBudget.
/// </summary>
public class AdvertisementBudgetConfiguration : BaseEntityConfiguration<AdvertisementBudget>
{
    public override void Configure(EntityTypeBuilder<AdvertisementBudget> builder)
    {
        base.Configure(builder);

        builder.ToTable("AdvertisementBudgets");

        builder.Property(e => e.BudgetType)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.SpentAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.RemainingAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.Currency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(e => e.PeriodStart)
            .IsRequired();

        builder.Property(e => e.PeriodEnd)
            .IsRequired();

        builder.HasOne(e => e.Campaign)
            .WithMany(e => e.Budgets)
            .HasForeignKey(e => e.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.CampaignId);
    }
}
