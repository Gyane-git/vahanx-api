using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for WishlistItem.
/// </summary>
public class WishlistItemConfiguration : BaseEntityConfiguration<WishlistItem>
{
    public override void Configure(EntityTypeBuilder<WishlistItem> builder)
    {
        base.Configure(builder);

        builder.ToTable("WishlistItems");

        builder.HasOne(e => e.Wishlist)
            .WithMany(e => e.Items)
            .HasForeignKey(e => e.WishlistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Listing)
            .WithMany(e => e.WishlistItems)
            .HasForeignKey(e => e.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.WishlistId);

        builder.HasIndex(e => e.ListingId);

        builder.HasIndex(e => new { e.WishlistId, e.ListingId })
            .IsUnique();
    }
}
