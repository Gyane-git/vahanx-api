using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Seller.
/// </summary>
public class SellerConfiguration : BaseEntityConfiguration<Seller>
{
    public override void Configure(EntityTypeBuilder<Seller> builder)
    {
        base.Configure(builder);

        builder.ToTable("Sellers");

        builder.Property(e => e.DisplayName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Phone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.Email)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(e => e.ProfileImageUrl)
            .HasMaxLength(2048)
            .IsRequired(false);

        builder.Property(e => e.Description)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.PreferredContactMethod)
            .IsRequired();

        builder.Property(e => e.SellerType)
            .IsRequired();

        builder.Property(e => e.VerificationStatus)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(e => e.Location)
            .WithMany()
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.UserId)
            .IsUnique();

        builder.HasIndex(e => e.Phone);

        builder.HasIndex(e => e.IsActive);
    }
}
