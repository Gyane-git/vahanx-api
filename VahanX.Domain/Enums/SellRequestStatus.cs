namespace VahanX.Domain.Enums;

/// <summary>
/// Sell request lifecycle status.
/// </summary>
public enum SellRequestStatus
{
    Draft = 0,
    Submitted = 1,
    UnderReview = 2,
    InspectionScheduled = 3,
    Inspected = 4,
    OfferReceived = 5,
    Negotiation = 6,
    Accepted = 7,
    Rejected = 8,
    Cancelled = 9,
    Completed = 10,
    Expired = 11
}
