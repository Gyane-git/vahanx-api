using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for PaymentTransaction.
/// </summary>
public class PaymentTransactionConfiguration : BaseEntityConfiguration<PaymentTransaction>
{
    public override void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        base.Configure(builder);

        builder.ToTable("PaymentTransactions");

        builder.Property(e => e.Provider)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.TransactionType)
            .IsRequired();

        builder.Property(e => e.ProviderTransactionReference)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.Currency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(e => e.Status)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.RawResponseReference)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.ProcessedAt)
            .IsRequired();

        builder.HasOne(e => e.Payment)
            .WithMany(e => e.Transactions)
            .HasForeignKey(e => e.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.PaymentId);
        builder.HasIndex(e => e.ProviderTransactionReference);
    }
}
