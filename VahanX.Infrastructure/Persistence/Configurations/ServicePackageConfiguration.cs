using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ServicePackage.
/// </summary>
public class ServicePackageConfiguration : BaseEntityConfiguration<ServicePackage>
{
    public override void Configure(EntityTypeBuilder<ServicePackage> builder)
    {
        base.Configure(builder);

        builder.ToTable("ServicePackages");

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.Currency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(e => e.ServiceCenterBranch)
            .WithMany(e => e.Packages)
            .HasForeignKey(e => e.ServiceCenterBranchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.ServiceCenterBranchId);
        builder.HasIndex(e => e.IsActive);
    }
}
