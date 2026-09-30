using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for PaymentItem.
/// </summary>
public class PaymentItemConfiguration : BaseEntityConfiguration<PaymentItem>
{
    public override void Configure(EntityTypeBuilder<PaymentItem> builder)
    {
        base.Configure(builder);

        builder.ToTable("PaymentItems");

        builder.Property(e => e.ItemType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.ReferenceId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.TotalPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasOne(e => e.Payment)
            .WithMany(e => e.Items)
            .HasForeignKey(e => e.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.PaymentId);
    }
}
