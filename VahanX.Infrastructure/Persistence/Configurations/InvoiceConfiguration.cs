using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Invoice.
/// </summary>
public class InvoiceConfiguration : BaseEntityConfiguration<Invoice>
{
    public override void Configure(EntityTypeBuilder<Invoice> builder)
    {
        base.Configure(builder);

        builder.ToTable("Invoices");

        builder.Property(e => e.InvoiceNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Subtotal)
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

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.IssuedAt)
            .IsRequired();

        builder.HasOne(e => e.Payment)
            .WithMany()
            .HasForeignKey(e => e.PaymentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.InvoiceNumber)
            .IsUnique();

        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.Status);
    }
}
