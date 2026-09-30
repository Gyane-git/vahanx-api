namespace VahanX.Domain.Enums;

/// <summary>
/// Allow-listed sort options for marketplace search.
/// Prevents arbitrary database column names from clients.
/// </summary>
public enum SortOrder
{
    Newest = 0,
    PriceLowToHigh = 1,
    PriceHighToLow = 2,
    MileageLowToHigh = 3,
    YearNewest = 4
}
