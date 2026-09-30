using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for InvoiceItem.
/// </summary>
public class InvoiceItemConfiguration : BaseEntityConfiguration<InvoiceItem>
{
    public override void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        base.Configure(builder);

        builder.ToTable("InvoiceItems");

        builder.Property(e => e.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.UnitPrice)
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

        builder.HasOne(e => e.Invoice)
            .WithMany(e => e.Items)
            .HasForeignKey(e => e.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.InvoiceId);
    }
}
