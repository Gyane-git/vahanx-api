using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for CompareItem.
/// </summary>
public class CompareItemConfiguration : BaseEntityConfiguration<CompareItem>
{
    public override void Configure(EntityTypeBuilder<CompareItem> builder)
    {
        base.Configure(builder);

        builder.ToTable("CompareItems");

        builder.HasOne(e => e.CompareList)
            .WithMany(e => e.Items)
            .HasForeignKey(e => e.CompareListId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Listing)
            .WithMany(e => e.CompareItems)
            .HasForeignKey(e => e.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.CompareListId);

        builder.HasIndex(e => e.ListingId);

        builder.HasIndex(e => new { e.CompareListId, e.ListingId })
            .IsUnique();
    }
}
