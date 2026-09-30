using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for AdvertisementTargeting.
/// </summary>
public class AdvertisementTargetingConfiguration : BaseEntityConfiguration<AdvertisementTargeting>
{
    public override void Configure(EntityTypeBuilder<AdvertisementTargeting> builder)
    {
        base.Configure(builder);

        builder.ToTable("AdvertisementTargeting");

        builder.Property(e => e.TargetType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.TargetValue)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasOne(e => e.Advertisement)
            .WithMany(e => e.Targeting)
            .HasForeignKey(e => e.AdvertisementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.AdvertisementId);
    }
}
