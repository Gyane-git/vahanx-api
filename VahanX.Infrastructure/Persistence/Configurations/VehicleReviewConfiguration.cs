using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for VehicleReview.
/// </summary>
public class VehicleReviewConfiguration : BaseEntityConfiguration<VehicleReview>
{
    public override void Configure(EntityTypeBuilder<VehicleReview> builder)
    {
        base.Configure(builder);

        builder.ToTable("VehicleReviews");

        builder.Property(e => e.Rating)
            .IsRequired();

        builder.Property(e => e.Title)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(e => e.Comment)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.Status)
            .IsRequired();

        builder.HasOne(e => e.Vehicle)
            .WithMany()
            .HasForeignKey(e => e.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Listing)
            .WithMany()
            .HasForeignKey(e => e.ListingId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.VehicleId);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.Status);
    }
}
