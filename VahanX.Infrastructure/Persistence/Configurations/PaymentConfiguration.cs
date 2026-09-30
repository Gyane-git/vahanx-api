using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Payment.
/// </summary>
public class PaymentConfiguration : BaseEntityConfiguration<Payment>
{
    public override void Configure(EntityTypeBuilder<Payment> builder)
    {
        base.Configure(builder);

        builder.ToTable("Payments");

        builder.Property(e => e.PaymentReference)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.PaymentPurpose)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.TaxAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.DiscountAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.Currency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(e => e.Provider)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.ProviderPaymentReference)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(e => e.IdempotencyKey)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.FailureReason)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.HasIndex(e => e.PaymentReference)
            .IsUnique();

        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.CreatedAt);
    }
}
