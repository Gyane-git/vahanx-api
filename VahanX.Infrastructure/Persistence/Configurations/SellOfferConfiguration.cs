using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for SellOffer.
/// </summary>
public class SellOfferConfiguration : BaseEntityConfiguration<SellOffer>
{
    public override void Configure(EntityTypeBuilder<SellOffer> builder)
    {
        base.Configure(builder);

        builder.ToTable("SellOffers");

        builder.Property(e => e.OfferedAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.Currency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(e => e.Notes)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.Status)
            .IsRequired();

        builder.HasOne(e => e.SellRequest)
            .WithMany(e => e.Offers)
            .HasForeignKey(e => e.SellRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.SellRequestId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.CreatedAt);
    }
}
