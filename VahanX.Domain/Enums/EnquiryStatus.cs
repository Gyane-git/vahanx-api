namespace VahanX.Domain.Enums;

/// <summary>
/// Enquiry lifecycle status.
/// </summary>
public enum EnquiryStatus
{
    New = 0,
    Open = 1,
    InProgress = 2,
    Responded = 3,
    Closed = 4,
    Cancelled = 5
}
