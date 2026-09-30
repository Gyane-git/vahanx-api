namespace VahanX.Domain.Enums;

/// <summary>
/// Predefined analytics time periods.
/// </summary>
public enum AnalyticsPeriod
{
    Today = 0,
    Yesterday = 1,
    Last7Days = 2,
    Last30Days = 3,
    ThisMonth = 4,
    PreviousMonth = 5,
    Custom = 99
}
