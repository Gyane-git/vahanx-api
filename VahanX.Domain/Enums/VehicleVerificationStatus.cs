namespace VahanX.Domain.Enums;

/// <summary>
/// Vehicle verification lifecycle status.
/// </summary>
public enum VehicleVerificationStatus
{
    NotVerified = 0,
    Pending = 1,
    InReview = 2,
    Verified = 3,
    Rejected = 4,
    Expired = 5,
    Revoked = 6
}
