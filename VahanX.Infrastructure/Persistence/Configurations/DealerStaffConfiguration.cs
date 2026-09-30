using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for DealerStaff.
/// </summary>
public class DealerStaffConfiguration : BaseEntityConfiguration<DealerStaff>
{
    public override void Configure(EntityTypeBuilder<DealerStaff> builder)
    {
        base.Configure(builder);

        builder.ToTable("DealerStaff");

        builder.Property(e => e.DisplayName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Phone)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(e => e.Email)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(e => e.Role)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(e => e.Dealer)
            .WithMany(e => e.Staff)
            .HasForeignKey(e => e.DealerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.DealerId);

        builder.HasIndex(e => e.UserId);

        builder.HasIndex(e => e.IsActive);
    }
}
