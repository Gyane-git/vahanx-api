using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Line item within a payment.
/// </summary>
public class PaymentItem : BaseEntity
{
    public Guid PaymentId { get; set; }

    public Payment? Payment { get; set; }

    public string ItemType { get; set; } = string.Empty;

    public string ReferenceId { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Quantity { get; set; } = 1;

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}
