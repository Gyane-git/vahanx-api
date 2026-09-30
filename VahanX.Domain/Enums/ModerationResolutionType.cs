namespace VahanX.Domain.Enums;

/// <summary>
/// Resolution types for moderation cases.
/// </summary>
public enum ModerationResolutionType
{
    NoAction = 0,
    WarningIssued = 1,
    ContentEdited = 2,
    ContentHidden = 3,
    ContentRejected = 4,
    ContentRemoved = 5,
    UserRestricted = 6,
    UserSuspended = 7,
    ListingSuspended = 8,
    AdvertisementRejected = 9,
    Escalated = 10
}
