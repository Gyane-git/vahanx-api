using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for VehicleVerification.
/// </summary>
public class VehicleVerificationConfiguration : BaseEntityConfiguration<VehicleVerification>
{
    public override void Configure(EntityTypeBuilder<VehicleVerification> builder)
    {
        base.Configure(builder);

        builder.ToTable("VehicleVerifications");

        builder.Property(e => e.VerificationReference)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(e => e.Notes)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.VerificationType)
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
        builder.HasIndex(e => e.ListingId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.VerificationType);
    }
}
