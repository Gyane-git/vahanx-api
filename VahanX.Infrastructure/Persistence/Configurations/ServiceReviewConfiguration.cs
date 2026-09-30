using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ServiceReview.
/// </summary>
public class ServiceReviewConfiguration : BaseEntityConfiguration<ServiceReview>
{
    public override void Configure(EntityTypeBuilder<ServiceReview> builder)
    {
        base.Configure(builder);

        builder.ToTable("ServiceReviews");

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

        builder.HasOne(e => e.ServiceCenter)
            .WithMany()
            .HasForeignKey(e => e.ServiceCenterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ServiceCenterBranch)
            .WithMany()
            .HasForeignKey(e => e.ServiceCenterBranchId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.ServiceBooking)
            .WithMany()
            .HasForeignKey(e => e.ServiceBookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.ServiceCenterId);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.CreatedAt);
    }
}
