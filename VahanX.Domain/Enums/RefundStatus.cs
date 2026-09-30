namespace VahanX.Domain.Enums;

/// <summary>
/// Refund status.
/// </summary>
public enum RefundStatus
{
    Requested = 0,
    Processing = 1,
    Succeeded = 2,
    Failed = 3,
    Cancelled = 4
}
