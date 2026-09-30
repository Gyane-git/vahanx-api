using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ServicePackageItem.
/// </summary>
public class ServicePackageItemConfiguration : BaseEntityConfiguration<ServicePackageItem>
{
    public override void Configure(EntityTypeBuilder<ServicePackageItem> builder)
    {
        base.Configure(builder);

        builder.ToTable("ServicePackageItems");

        builder.Property(e => e.Notes)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.HasOne(e => e.ServicePackage)
            .WithMany(e => e.Items)
            .HasForeignKey(e => e.ServicePackageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.AutoServiceType)
            .WithMany()
            .HasForeignKey(e => e.AutoServiceTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.ServicePackageId, e.AutoServiceTypeId })
            .IsUnique();
    }
}
