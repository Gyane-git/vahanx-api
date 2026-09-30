using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for RegistrationHistory.
/// </summary>
public class RegistrationHistoryConfiguration : BaseEntityConfiguration<RegistrationHistory>
{
    public override void Configure(EntityTypeBuilder<RegistrationHistory> builder)
    {
        base.Configure(builder);

        builder.ToTable("RegistrationHistory");

        builder.Property(e => e.RegistrationArea)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(e => e.RegistrationReference)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(e => e.RegistrationStatus)
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
    }
}
