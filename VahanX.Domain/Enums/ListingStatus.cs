namespace VahanX.Domain.Enums;

/// <summary>
/// Listing lifecycle status.
/// Supports future Admin moderation workflow.
/// </summary>
public enum ListingStatus
{
    Draft = 0,
    PendingReview = 1,
    Published = 2,
    Paused = 3,
    Sold = 4,
    Expired = 5,
    Rejected = 6,
    Archived = 7
}
