using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for SellRequest.
/// </summary>
public class SellRequestConfiguration : BaseEntityConfiguration<SellRequest>
{
    public override void Configure(EntityTypeBuilder<SellRequest> builder)
    {
        base.Configure(builder);

        builder.ToTable("SellRequests");

        builder.Property(e => e.PreferredContactTime)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.Notes)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.PreferredContactMethod)
            .IsRequired();

        builder.HasOne(e => e.SellVehicle)
            .WithOne()
            .HasForeignKey<SellRequest>(e => e.SellVehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Location)
            .WithMany()
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.CreatedAt);
    }
}
