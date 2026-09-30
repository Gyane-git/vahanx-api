using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for TestDriveSlot.
/// </summary>
public class TestDriveSlotConfiguration : BaseEntityConfiguration<TestDriveSlot>
{
    public override void Configure(EntityTypeBuilder<TestDriveSlot> builder)
    {
        base.Configure(builder);

        builder.ToTable("TestDriveSlots");

        builder.Property(e => e.Date)
            .IsRequired();

        builder.Property(e => e.StartTime)
            .IsRequired();

        builder.Property(e => e.EndTime)
            .IsRequired();

        builder.Property(e => e.Capacity)
            .HasDefaultValue(1);

        builder.Property(e => e.IsAvailable)
            .HasDefaultValue(true);

        builder.HasOne(e => e.Dealer)
            .WithMany()
            .HasForeignKey(e => e.DealerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.DealerId);
        builder.HasIndex(e => e.Date);
    }
}
