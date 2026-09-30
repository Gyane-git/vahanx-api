using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for VehicleListing.
/// </summary>
public class VehicleListingConfiguration : BaseEntityConfiguration<VehicleListing>
{
    public override void Configure(EntityTypeBuilder<VehicleListing> builder)
    {
        base.Configure(builder);

        builder.ToTable("VehicleListings");

        builder.Property(e => e.Title)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(5000)
            .IsRequired(false);

        builder.Property(e => e.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.Currency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(e => e.MileageUnit)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(e => e.Condition)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.ContactPreference)
            .IsRequired();

        builder.Property(e => e.IsNegotiable)
            .HasDefaultValue(false);

        builder.HasOne(e => e.Vehicle)
            .WithMany()
            .HasForeignKey(e => e.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Seller)
            .WithMany(e => e.Listings)
            .HasForeignKey(e => e.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Dealer)
            .WithMany(e => e.Listings)
            .HasForeignKey(e => e.DealerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Location)
            .WithMany()
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.VehicleId);
        builder.HasIndex(e => e.SellerId);
        builder.HasIndex(e => e.DealerId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.LocationId);
        builder.HasIndex(e => e.Price);
        builder.HasIndex(e => e.Mileage);
        builder.HasIndex(e => e.ManufactureYear);
        builder.HasIndex(e => e.CreatedAt);

        builder.HasIndex(e => new { e.Status, e.CreatedAt });
        builder.HasIndex(e => new { e.Status, e.Price });
        builder.HasIndex(e => new { e.Status, e.LocationId });
    }
}
