using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ChargingStation.
/// </summary>
public class ChargingStationConfiguration : BaseEntityConfiguration<ChargingStation>
{
    public override void Configure(EntityTypeBuilder<ChargingStation> builder)
    {
        base.Configure(builder);

        builder.ToTable("ChargingStations");

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.OperatorName)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(e => e.ContactPhone)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(e => e.ContactEmail)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(e => e.WebsiteUrl)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.VerificationStatus)
            .IsRequired();

        builder.Property(e => e.Is24Hours)
            .HasDefaultValue(false);

        builder.HasOne(e => e.Location)
            .WithMany()
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.LocationId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.VerificationStatus);
    }
}
