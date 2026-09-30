using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Provider-independent payment record.
/// </summary>
public class Payment : BaseEntity
{
    public Guid UserId { get; set; }

    public string PaymentReference { get; set; } = string.Empty;

    public PaymentPurpose PaymentPurpose { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public decimal Amount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public string Currency { get; set; } = "NPR";

    public string? Provider { get; set; }

    public string? ProviderPaymentReference { get; set; }

    public string? IdempotencyKey { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime? FailedAt { get; set; }

    public string? FailureReason { get; set; }

    public ICollection<PaymentItem> Items { get; set; } = new List<PaymentItem>();

    public ICollection<PaymentTransaction> Transactions { get; set; } = new List<PaymentTransaction>();
}
