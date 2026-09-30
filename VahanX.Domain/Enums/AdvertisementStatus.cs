namespace VahanX.Domain.Enums;

/// <summary>
/// Advertisement status.
/// </summary>
public enum AdvertisementStatus
{
    Draft = 0,
    PendingReview = 1,
    Approved = 2,
    Active = 3,
    Paused = 4,
    Rejected = 5,
    Expired = 6
}
