namespace VahanX.Domain.Enums;

/// <summary>
/// Advertisement campaign lifecycle status.
/// </summary>
public enum AdvertisementCampaignStatus
{
    Draft = 0,
    PendingReview = 1,
    Approved = 2,
    Running = 3,
    Paused = 4,
    Completed = 5,
    Rejected = 6,
    Cancelled = 7
}
