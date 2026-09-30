namespace VahanX.Domain.Exceptions;

/// <summary>
/// Exception thrown when an analytics date range is invalid.
/// </summary>
public class AnalyticsRangeInvalidException : DomainException
{
    public AnalyticsRangeInvalidException(string message)
        : base(message, "ANALYTICS_RANGE_INVALID")
    {
    }
}
