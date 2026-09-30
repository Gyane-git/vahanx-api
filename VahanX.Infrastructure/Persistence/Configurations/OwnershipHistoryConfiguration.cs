using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for OwnershipHistory.
/// </summary>
public class OwnershipHistoryConfiguration : BaseEntityConfiguration<OwnershipHistory>
{
    public override void Configure(EntityTypeBuilder<OwnershipHistory> builder)
    {
        base.Configure(builder);

        builder.ToTable("OwnershipHistory");

        builder.Property(e => e.Notes)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(e => e.OwnershipType)
            .IsRequired();

        builder.Property(e => e.Source)
            .IsRequired();

        builder.HasOne(e => e.Vehicle)
            .WithMany()
            .HasForeignKey(e => e.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.VehicleId);
        builder.HasIndex(e => e.FromDate);
    }
}
