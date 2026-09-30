using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for AutoServiceType.
/// </summary>
public class AutoServiceTypeConfiguration : BaseEntityConfiguration<AutoServiceType>
{
    public override void Configure(EntityTypeBuilder<AutoServiceType> builder)
    {
        base.Configure(builder);

        builder.ToTable("ServiceTypes");

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(e => e.VehicleType)
            .WithMany()
            .HasForeignKey(e => e.VehicleTypeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.Code)
            .IsUnique();

        builder.HasIndex(e => e.IsActive);
    }
}
