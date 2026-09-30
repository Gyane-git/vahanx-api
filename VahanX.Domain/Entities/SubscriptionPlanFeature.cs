using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Feature configuration within a subscription plan.
/// </summary>
public class SubscriptionPlanFeature : BaseEntity
{
    public Guid SubscriptionPlanId { get; set; }

    public SubscriptionPlan? SubscriptionPlan { get; set; }

    public Guid FeatureId { get; set; }

    public Feature? Feature { get; set; }

    public bool? BooleanValue { get; set; }

    public decimal? NumericValue { get; set; }

    public int? LimitValue { get; set; }
}
