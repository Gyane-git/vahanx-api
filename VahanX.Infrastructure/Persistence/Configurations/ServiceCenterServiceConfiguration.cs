using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ServiceCenterService.
/// </summary>
public class ServiceCenterServiceConfiguration : BaseEntityConfiguration<ServiceCenterService>
{
    public override void Configure(EntityTypeBuilder<ServiceCenterService> builder)
    {
        base.Configure(builder);

        builder.ToTable("ServiceCenterServices");

        builder.Property(e => e.PriceFrom)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.Property(e => e.PriceTo)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.Property(e => e.IsAvailable)
            .HasDefaultValue(true);

        builder.Property(e => e.Description)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.HasOne(e => e.ServiceCenterBranch)
            .WithMany(e => e.Services)
            .HasForeignKey(e => e.ServiceCenterBranchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.AutoServiceType)
            .WithMany()
            .HasForeignKey(e => e.AutoServiceTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.ServiceCenterBranchId, e.AutoServiceTypeId })
            .IsUnique();
    }
}
