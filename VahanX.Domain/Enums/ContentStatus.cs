namespace VahanX.Domain.Enums;

/// <summary>
/// CMS content lifecycle status.
/// </summary>
public enum ContentStatus
{
    Draft = 0,
    PendingReview = 1,
    Published = 2,
    Unpublished = 3,
    Archived = 4
}
