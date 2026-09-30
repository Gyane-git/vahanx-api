using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// User/business subscription.
/// </summary>
public class Subscription : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid? SellerId { get; set; }

    public Seller? Seller { get; set; }

    public Guid? DealerId { get; set; }

    public Dealer? Dealer { get; set; }

    public Guid SubscriptionPlanId { get; set; }

    public SubscriptionPlan? SubscriptionPlan { get; set; }

    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Pending;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? TrialStartDate { get; set; }

    public DateTime? TrialEndDate { get; set; }

    public bool AutoRenew { get; set; } = true;

    public DateTime? CancelledAt { get; set; }

    public string? CancellationReason { get; set; }

    public DateTime CurrentPeriodStart { get; set; }

    public DateTime CurrentPeriodEnd { get; set; }

    public string? ExternalSubscriptionReference { get; set; }

    public ICollection<SubscriptionUsage> Usage { get; set; } = new List<SubscriptionUsage>();

    public ICollection<SubscriptionChangeHistory> ChangeHistory { get; set; } = new List<SubscriptionChangeHistory>();
}
