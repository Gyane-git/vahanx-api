using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Subscription plan definition.
/// </summary>
public class SubscriptionPlan : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public SubscriptionTargetType TargetType { get; set; } = SubscriptionTargetType.User;

    public BillingCycle BillingCycle { get; set; } = BillingCycle.Monthly;

    public decimal Price { get; set; }

    public string Currency { get; set; } = "NPR";

    public int TrialDays { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsPublic { get; set; } = true;

    public int DisplayOrder { get; set; }

    public ICollection<SubscriptionPlanFeature> PlanFeatures { get; set; } = new List<SubscriptionPlanFeature>();
}
