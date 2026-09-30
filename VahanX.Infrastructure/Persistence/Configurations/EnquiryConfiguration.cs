using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Enquiry.
/// </summary>
public class EnquiryConfiguration : BaseEntityConfiguration<Enquiry>
{
    public override void Configure(EntityTypeBuilder<Enquiry> builder)
    {
        base.Configure(builder);

        builder.ToTable("Enquiries");

        builder.Property(e => e.Subject)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(e => e.Message)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(e => e.EnquiryType)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired();

        builder.HasOne(e => e.Listing)
            .WithMany()
            .HasForeignKey(e => e.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Seller)
            .WithMany()
            .HasForeignKey(e => e.SellerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Dealer)
            .WithMany()
            .HasForeignKey(e => e.DealerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.ListingId);
        builder.HasIndex(e => e.SellerId);
        builder.HasIndex(e => e.DealerId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.CreatedAt);
    }
}
