using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Subscription feature usage tracking.
/// </summary>
public class SubscriptionUsage : BaseEntity
{
    public Guid SubscriptionId { get; set; }

    public Subscription? Subscription { get; set; }

    public Guid FeatureId { get; set; }

    public Feature? Feature { get; set; }

    public DateTime PeriodStart { get; set; }

    public DateTime PeriodEnd { get; set; }

    public int UsedValue { get; set; }

    public int LimitValue { get; set; }

    public DateTime LastCalculatedAt { get; set; }
}
