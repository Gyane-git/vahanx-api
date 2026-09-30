using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for PaymentRefund.
/// </summary>
public class PaymentRefundConfiguration : BaseEntityConfiguration<PaymentRefund>
{
    public override void Configure(EntityTypeBuilder<PaymentRefund> builder)
    {
        base.Configure(builder);

        builder.ToTable("PaymentRefunds");

        builder.Property(e => e.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.Currency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(e => e.Reason)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.ProviderRefundReference)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.HasOne(e => e.Payment)
            .WithMany()
            .HasForeignKey(e => e.PaymentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.PaymentId);
    }
}
