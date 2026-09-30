using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Invoice for a payment.
/// </summary>
public class Invoice : BaseEntity
{
    public Guid UserId { get; set; }

    public string InvoiceNumber { get; set; } = string.Empty;

    public Guid PaymentId { get; set; }

    public Payment? Payment { get; set; }

    public decimal Subtotal { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public string Currency { get; set; } = "NPR";

    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

    public DateTime IssuedAt { get; set; }

    public DateTime? DueAt { get; set; }

    public DateTime? PaidAt { get; set; }

    public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
}
