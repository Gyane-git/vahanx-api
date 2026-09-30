using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Dealer.
/// </summary>
public class DealerConfiguration : BaseEntityConfiguration<Dealer>
{
    public override void Configure(EntityTypeBuilder<Dealer> builder)
    {
        base.Configure(builder);

        builder.ToTable("Dealers");

        builder.Property(e => e.BusinessName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.LegalName)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(e => e.RegistrationNumber)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.Description)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.Phone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.Email)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(e => e.Website)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.VerificationStatus)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(e => e.Location)
            .WithMany()
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.BusinessName);

        builder.HasIndex(e => e.Phone);

        builder.HasIndex(e => e.IsActive);
    }
}
