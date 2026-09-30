using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Wishlist.
/// </summary>
public class WishlistConfiguration : BaseEntityConfiguration<Wishlist>
{
    public override void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        base.Configure(builder);

        builder.ToTable("Wishlists");

        builder.HasIndex(e => e.UserId)
            .IsUnique();
    }
}
