using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ServiceBooking.
/// </summary>
public class ServiceBookingConfiguration : BaseEntityConfiguration<ServiceBooking>
{
    public override void Configure(EntityTypeBuilder<ServiceBooking> builder)
    {
        base.Configure(builder);

        builder.ToTable("ServiceBookings");

        builder.Property(e => e.BookingReference)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.RequestedDate)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.CustomerNotes)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.CenterNotes)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.EstimatedPrice)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.Property(e => e.FinalPrice)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.Property(e => e.Currency)
            .HasMaxLength(3)
            .IsRequired(false);

        builder.Property(e => e.CancellationReason)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.HasOne(e => e.ServiceCenterBranch)
            .WithMany(e => e.Bookings)
            .HasForeignKey(e => e.ServiceCenterBranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Vehicle)
            .WithMany()
            .HasForeignKey(e => e.VehicleId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.AutoServiceType)
            .WithMany()
            .HasForeignKey(e => e.AutoServiceTypeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.ServicePackage)
            .WithMany()
            .HasForeignKey(e => e.ServicePackageId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.ServiceCenterBranchId);
        builder.HasIndex(e => e.VehicleId);
        builder.HasIndex(e => e.RequestedDate);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.BookingReference)
            .IsUnique();
    }
}
