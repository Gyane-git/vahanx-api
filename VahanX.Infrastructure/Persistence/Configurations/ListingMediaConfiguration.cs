using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ListingMedia.
/// </summary>
public class ListingMediaConfiguration : BaseEntityConfiguration<ListingMedia>
{
    public override void Configure(EntityTypeBuilder<ListingMedia> builder)
    {
        base.Configure(builder);

        builder.ToTable("ListingMedia");

        builder.Property(e => e.MediaUrl)
            .HasMaxLength(2048)
            .IsRequired();

        builder.Property(e => e.MediaType)
            .IsRequired();

        builder.Property(e => e.DisplayOrder)
            .HasDefaultValue(0);

        builder.Property(e => e.IsPrimary)
            .HasDefaultValue(false);

        builder.Property(e => e.Caption)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.HasOne(e => e.Listing)
            .WithMany(e => e.Media)
            .HasForeignKey(e => e.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.ListingId);

        builder.HasIndex(e => new { e.ListingId, e.IsPrimary })
            .IsUnique()
            .HasFilter("[IsPrimary] = 1");
    }
}
