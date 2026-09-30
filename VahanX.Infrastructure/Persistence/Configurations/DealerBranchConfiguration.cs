using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for DealerBranch.
/// </summary>
public class DealerBranchConfiguration : BaseEntityConfiguration<DealerBranch>
{
    public override void Configure(EntityTypeBuilder<DealerBranch> builder)
    {
        base.Configure(builder);

        builder.ToTable("DealerBranches");

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Phone)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(e => e.Email)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(e => e.Latitude)
            .IsRequired(false);

        builder.Property(e => e.Longitude)
            .IsRequired(false);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(e => e.Dealer)
            .WithMany(e => e.Branches)
            .HasForeignKey(e => e.DealerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Location)
            .WithMany()
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.DealerId);

        builder.HasIndex(e => e.IsActive);
    }
}
