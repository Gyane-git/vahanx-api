using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for AdvertisementClick.
/// </summary>
public class AdvertisementClickConfiguration : BaseEntityConfiguration<AdvertisementClick>
{
    public override void Configure(EntityTypeBuilder<AdvertisementClick> builder)
    {
        base.Configure(builder);

        builder.ToTable("AdvertisementClicks");

        builder.Property(e => e.SessionReference)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(e => e.OccurredAt)
            .IsRequired();

        builder.HasOne(e => e.Advertisement)
            .WithMany()
            .HasForeignKey(e => e.AdvertisementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.AdvertisementId);
        builder.HasIndex(e => e.OccurredAt);
    }
}
