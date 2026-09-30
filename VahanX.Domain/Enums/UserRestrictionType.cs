namespace VahanX.Domain.Enums;

/// <summary>
/// Types of user restrictions for moderation.
/// </summary>
public enum UserRestrictionType
{
    Warning = 0,
    ListingRestriction = 1,
    MessagingRestriction = 2,
    AdvertisementRestriction = 3,
    SellVehicleRestriction = 4,
    FullPlatformRestriction = 5
}
