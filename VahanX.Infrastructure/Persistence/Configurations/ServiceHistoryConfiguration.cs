using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ServiceHistory.
/// </summary>
public class ServiceHistoryConfiguration : BaseEntityConfiguration<ServiceHistory>
{
    public override void Configure(EntityTypeBuilder<ServiceHistory> builder)
    {
        base.Configure(builder);

        builder.ToTable("ServiceHistory");

        builder.Property(e => e.ServiceProvider)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(e => e.Description)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.Cost)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.Property(e => e.ServiceType)
            .IsRequired();

        builder.Property(e => e.Source)
            .IsRequired();

        builder.Property(e => e.Notes)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.HasOne(e => e.Vehicle)
            .WithMany()
            .HasForeignKey(e => e.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.VehicleId);
        builder.HasIndex(e => e.ServiceDate);
    }
}
