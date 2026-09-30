using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Line item within an invoice.
/// </summary>
public class InvoiceItem : BaseEntity
{
    public Guid InvoiceId { get; set; }

    public Invoice? Invoice { get; set; }

    public string Description { get; set; } = string.Empty;

    public int Quantity { get; set; } = 1;

    public decimal UnitPrice { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TotalAmount { get; set; }
}
