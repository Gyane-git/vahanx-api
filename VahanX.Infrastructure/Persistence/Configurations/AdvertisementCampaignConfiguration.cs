using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for AdvertisementCampaign.
/// </summary>
public class AdvertisementCampaignConfiguration : BaseEntityConfiguration<AdvertisementCampaign>
{
    public override void Configure(EntityTypeBuilder<AdvertisementCampaign> builder)
    {
        base.Configure(builder);

        builder.ToTable("AdvertisementCampaigns");

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.Property(e => e.EndDate)
            .IsRequired();

        builder.Property(e => e.BudgetType)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.BudgetAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.DailyBudget)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.Property(e => e.TotalBudget)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.Property(e => e.Currency)
            .HasMaxLength(3)
            .IsRequired();

        builder.HasOne(e => e.Seller)
            .WithMany()
            .HasForeignKey(e => e.SellerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Dealer)
            .WithMany()
            .HasForeignKey(e => e.DealerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.AdvertiserUserId);
        builder.HasIndex(e => e.SellerId);
        builder.HasIndex(e => e.DealerId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.StartDate);
        builder.HasIndex(e => e.EndDate);
    }
}
