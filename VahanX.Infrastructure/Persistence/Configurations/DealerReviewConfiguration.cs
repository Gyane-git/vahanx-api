using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for DealerReview.
/// </summary>
public class DealerReviewConfiguration : BaseEntityConfiguration<DealerReview>
{
    public override void Configure(EntityTypeBuilder<DealerReview> builder)
    {
        base.Configure(builder);

        builder.ToTable("DealerReviews");

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

        builder.HasOne(e => e.Dealer)
            .WithMany()
            .HasForeignKey(e => e.DealerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.DealerId);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.Status);
    }
}
