using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for InsuranceHistory.
/// </summary>
public class InsuranceHistoryConfiguration : BaseEntityConfiguration<InsuranceHistory>
{
    public override void Configure(EntityTypeBuilder<InsuranceHistory> builder)
    {
        base.Configure(builder);

        builder.ToTable("InsuranceHistory");

        builder.Property(e => e.Provider)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(e => e.PolicyReference)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.Source)
            .IsRequired();

        builder.HasOne(e => e.Vehicle)
            .WithMany()
            .HasForeignKey(e => e.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.VehicleId);
    }
}
