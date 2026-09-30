namespace VahanX.Domain.Enums;

/// <summary>
/// Type of notification.
/// </summary>
public enum NotificationType
{
    NewEnquiry = 0,
    EnquiryResponse = 1,
    NewMessage = 2,
    ListingPublished = 3,
    ListingRejected = 4,
    ListingPriceChanged = 5,
    WishlistPriceChanged = 6,
    TestDriveRequested = 7,
    TestDriveConfirmed = 8,
    TestDriveRescheduled = 9,
    TestDriveCancelled = 10,
    VerificationCompleted = 11,
    InspectionCompleted = 12,
    ReviewPublished = 13,
    System = 99
}
