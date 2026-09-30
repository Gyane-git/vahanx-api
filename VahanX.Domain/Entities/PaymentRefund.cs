using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Payment refund record.
/// </summary>
public class PaymentRefund : BaseEntity
{
    public Guid PaymentId { get; set; }

    public Payment? Payment { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "NPR";

    public string Reason { get; set; } = string.Empty;

    public RefundStatus Status { get; set; } = RefundStatus.Requested;

    public string? ProviderRefundReference { get; set; }

    public DateTime? CompletedAt { get; set; }
}
