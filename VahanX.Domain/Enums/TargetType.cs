namespace VahanX.Domain.Enums;

/// <summary>
/// Target types for reports, moderation cases, banners, and promotions.
/// </summary>
public enum TargetType
{
    Listing = 0,
    User = 1,
    Seller = 2,
    Dealer = 3,
    Review = 4,
    Message = 5,
    Advertisement = 6,
    ServiceCenter = 7,
    ChargingStation = 8,
    FuelStation = 9,
    CMSContent = 10,
    Page = 11,
    ExternalUrl = 12,
    None = 99
}
