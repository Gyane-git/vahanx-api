using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Payment transaction record. Append-only.
/// </summary>
public class PaymentTransaction : BaseEntity
{
    public Guid PaymentId { get; set; }

    public Payment? Payment { get; set; }

    public string Provider { get; set; } = string.Empty;

    public PaymentTransactionType TransactionType { get; set; }

    public string ProviderTransactionReference { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "NPR";

    public string Status { get; set; } = string.Empty;

    public string? RawResponseReference { get; set; }

    public DateTime ProcessedAt { get; set; }
}
