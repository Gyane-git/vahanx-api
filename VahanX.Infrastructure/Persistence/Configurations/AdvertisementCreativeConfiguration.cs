using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for AdvertisementCreative.
/// </summary>
public class AdvertisementCreativeConfiguration : BaseEntityConfiguration<AdvertisementCreative>
{
    public override void Configure(EntityTypeBuilder<AdvertisementCreative> builder)
    {
        base.Configure(builder);

        builder.ToTable("AdvertisementCreatives");

        builder.Property(e => e.MediaReference)
            .HasMaxLength(2048)
            .IsRequired();

        builder.Property(e => e.MediaType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.AltText)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.HasOne(e => e.Advertisement)
            .WithMany(e => e.Creatives)
            .HasForeignKey(e => e.AdvertisementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.AdvertisementId);
    }
}
