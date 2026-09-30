using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ServiceWorkingHour.
/// </summary>
public class ServiceWorkingHourConfiguration : BaseEntityConfiguration<ServiceWorkingHour>
{
    public override void Configure(EntityTypeBuilder<ServiceWorkingHour> builder)
    {
        base.Configure(builder);

        builder.ToTable("ServiceWorkingHours");

        builder.Property(e => e.DayOfWeek)
            .IsRequired();

        builder.Property(e => e.OpeningTime)
            .IsRequired();

        builder.Property(e => e.ClosingTime)
            .IsRequired();

        builder.Property(e => e.IsClosed)
            .HasDefaultValue(false);

        builder.HasOne(e => e.ServiceCenterBranch)
            .WithMany(e => e.WorkingHours)
            .HasForeignKey(e => e.ServiceCenterBranchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.ServiceCenterBranchId, e.DayOfWeek });
    }
}
