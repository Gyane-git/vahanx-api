using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Advertisement.
/// </summary>
public class AdvertisementConfiguration : BaseEntityConfiguration<Advertisement>
{
    public override void Configure(EntityTypeBuilder<Advertisement> builder)
    {
        base.Configure(builder);

        builder.ToTable("Advertisements");

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Headline)
            .HasMaxLength(300)
            .IsRequired(false);

        builder.Property(e => e.Description)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.DestinationType)
            .IsRequired();

        builder.Property(e => e.DestinationUrl)
            .HasMaxLength(2048)
            .IsRequired(false);

        builder.Property(e => e.MediaReference)
            .HasMaxLength(2048)
            .IsRequired(false);

        builder.Property(e => e.Status)
            .IsRequired();

        builder.HasOne(e => e.Campaign)
            .WithMany(e => e.Advertisements)
            .HasForeignKey(e => e.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.VehicleListing)
            .WithMany()
            .HasForeignKey(e => e.VehicleListingId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.CampaignId);
        builder.HasIndex(e => e.Status);
    }
}
