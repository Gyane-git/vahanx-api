namespace VahanX.Domain.Enums;

/// <summary>
/// Invoice status.
/// </summary>
public enum InvoiceStatus
{
    Draft = 0,
    Issued = 1,
    Paid = 2,
    Void = 3,
    Overdue = 4,
    Refunded = 5
}
