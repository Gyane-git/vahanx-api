namespace VahanX.Domain.Enums;

/// <summary>
/// Source of history information.
/// </summary>
public enum HistorySource
{
    SellerProvided = 0,
    DealerProvided = 1,
    VahanXVerified = 2,
    GovernmentRecord = 3,
    InsuranceProvider = 4,
    ServiceCenter = 5,
    PreviousOwner = 6,
    Other = 99
}
