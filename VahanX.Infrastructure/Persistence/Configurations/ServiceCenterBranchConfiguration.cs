using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ServiceCenterBranch.
/// </summary>
public class ServiceCenterBranchConfiguration : BaseEntityConfiguration<ServiceCenterBranch>
{
    public override void Configure(EntityTypeBuilder<ServiceCenterBranch> builder)
    {
        base.Configure(builder);

        builder.ToTable("ServiceCenterBranches");

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.ContactPhone)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(e => e.ContactEmail)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(e => e.Status)
            .IsRequired();

        builder.HasOne(e => e.ServiceCenter)
            .WithMany(e => e.Branches)
            .HasForeignKey(e => e.ServiceCenterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Location)
            .WithMany()
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.ServiceCenterId);
        builder.HasIndex(e => e.LocationId);
    }
}
