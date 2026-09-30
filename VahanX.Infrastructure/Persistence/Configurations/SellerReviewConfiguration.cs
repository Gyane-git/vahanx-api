using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for SellerReview.
/// </summary>
public class SellerReviewConfiguration : BaseEntityConfiguration<SellerReview>
{
    public override void Configure(EntityTypeBuilder<SellerReview> builder)
    {
        base.Configure(builder);

        builder.ToTable("SellerReviews");

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

        builder.HasOne(e => e.Seller)
            .WithMany()
            .HasForeignKey(e => e.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.SellerId);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.Status);
    }
}
