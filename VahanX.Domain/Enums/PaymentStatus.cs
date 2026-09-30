namespace VahanX.Domain.Enums;

/// <summary>
/// Payment lifecycle status.
/// </summary>
public enum PaymentStatus
{
    Pending = 0,
    Processing = 1,
    Succeeded = 2,
    Failed = 3,
    Cancelled = 4,
    Refunded = 5,
    PartiallyRefunded = 6
}
