namespace VahanX.Domain.Enums;

/// <summary>
/// Subscription lifecycle status.
/// </summary>
public enum SubscriptionStatus
{
    Pending = 0,
    Trialing = 1,
    Active = 2,
    PastDue = 3,
    Paused = 4,
    Cancelled = 5,
    Expired = 6,
    Suspended = 7
}
