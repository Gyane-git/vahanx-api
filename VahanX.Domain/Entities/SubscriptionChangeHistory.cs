using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Audit trail for subscription changes. Append-only.
/// </summary>
public class SubscriptionChangeHistory : BaseEntity
{
    public Guid SubscriptionId { get; set; }

    public Subscription? Subscription { get; set; }

    public Guid? OldPlanId { get; set; }

    public Guid? NewPlanId { get; set; }

    public SubscriptionStatus? OldStatus { get; set; }

    public SubscriptionStatus NewStatus { get; set; }

    public string ChangeType { get; set; } = string.Empty;

    public string? Reason { get; set; }

    public DateTime ChangedAt { get; set; }

    public Guid? ChangedByUserId { get; set; }
}
