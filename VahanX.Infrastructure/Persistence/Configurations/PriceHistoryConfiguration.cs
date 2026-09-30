using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for PriceHistory.
/// </summary>
public class PriceHistoryConfiguration : BaseEntityConfiguration<PriceHistory>
{
    public override void Configure(EntityTypeBuilder<PriceHistory> builder)
    {
        base.Configure(builder);

        builder.ToTable("PriceHistory");

        builder.Property(e => e.OldPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.NewPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.Currency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(e => e.ChangedAt)
            .IsRequired();

        builder.Property(e => e.Reason)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.HasOne(e => e.Listing)
            .WithMany()
            .HasForeignKey(e => e.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.ListingId);
        builder.HasIndex(e => e.ChangedAt);
    }
}
