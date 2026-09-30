namespace VahanX.Application.DTOs.Analytics;

/// <summary>
/// Analytics overview DTO.
/// </summary>
public class AnalyticsOverviewDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public int NewUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int NewListings { get; set; }
    public int PublishedListings { get; set; }
    public int Enquiries { get; set; }
    public int TestDriveRequests { get; set; }
    public int ServiceBookings { get; set; }
    public int ActiveAdvertisements { get; set; }
    public int ActiveSubscriptions { get; set; }
    public int SellRequests { get; set; }
    public decimal GrossRevenue { get; set; }
    public decimal Refunds { get; set; }
    public decimal NetRevenue { get; set; }
}

/// <summary>
/// User analytics DTO.
/// </summary>
public class UserAnalyticsDto
{
    public DateTime Date { get; set; }
    public int NewUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int TotalUsers { get; set; }
}

/// <summary>
/// Listing analytics DTO.
/// </summary>
public class ListingAnalyticsDto
{
    public DateTime Date { get; set; }
    public int NewListings { get; set; }
    public int PublishedListings { get; set; }
    public int SoldListings { get; set; }
    public long TotalViews { get; set; }
    public long UniqueViews { get; set; }
    public long WishlistAdds { get; set; }
    public long Enquiries { get; set; }
}

/// <summary>
/// Vehicle analytics DTO.
/// </summary>
public class VehicleAnalyticsDto
{
    public string Dimension { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public int VehicleViews { get; set; }
    public int ListingCount { get; set; }
    public int WishlistCount { get; set; }
    public int EnquiryCount { get; set; }
    public int TestDriveCount { get; set; }
    public decimal? AverageListingPrice { get; set; }
}

/// <summary>
/// Search analytics DTO.
/// </summary>
public class SearchAnalyticsDto
{
    public string SearchQuery { get; set; } = string.Empty;
    public int SearchCount { get; set; }
    public int AverageResultCount { get; set; }
    public int ZeroResultCount { get; set; }
}

/// <summary>
/// Enquiry analytics DTO.
/// </summary>
public class EnquiryAnalyticsDto
{
    public DateTime Date { get; set; }
    public int EnquiriesCreated { get; set; }
    public int EnquiriesResponded { get; set; }
    public int EnquiriesClosed { get; set; }
    public double? AverageResponseTimeHours { get; set; }
}

/// <summary>
/// Test drive analytics DTO.
/// </summary>
public class TestDriveAnalyticsDto
{
    public DateTime Date { get; set; }
    public int Requested { get; set; }
    public int Confirmed { get; set; }
    public int Completed { get; set; }
    public int Cancelled { get; set; }
    public int NoShow { get; set; }
    public double? ConversionRate { get; set; }
}

/// <summary>
/// Service analytics DTO.
/// </summary>
public class ServiceAnalyticsDto
{
    public DateTime Date { get; set; }
    public int BookingsCreated { get; set; }
    public int BookingsCompleted { get; set; }
    public int BookingsCancelled { get; set; }
    public decimal? AverageBookingValue { get; set; }
}

/// <summary>
/// Advertisement analytics DTO.
/// </summary>
public class AdvertisementAnalyticsDto
{
    public DateTime Date { get; set; }
    public int Impressions { get; set; }
    public int Clicks { get; set; }
    public double? CTR { get; set; }
    public int ActiveCampaigns { get; set; }
    public int CompletedCampaigns { get; set; }
    public decimal Spend { get; set; }
    public double? BudgetUtilization { get; set; }
}

/// <summary>
/// Subscription analytics DTO.
/// </summary>
public class SubscriptionAnalyticsDto
{
    public DateTime Date { get; set; }
    public int ActiveSubscriptions { get; set; }
    public int NewSubscriptions { get; set; }
    public int Renewals { get; set; }
    public int Cancellations { get; set; }
    public int Expired { get; set; }
    public decimal SubscriptionRevenue { get; set; }
}

/// <summary>
/// Sell vehicle analytics DTO.
/// </summary>
public class SellVehicleAnalyticsDto
{
    public DateTime Date { get; set; }
    public int RequestsSubmitted { get; set; }
    public int RequestsCompleted { get; set; }
    public int RequestsCancelled { get; set; }
    public int InspectionsCompleted { get; set; }
    public int OffersCreated { get; set; }
    public int OffersAccepted { get; set; }
    public decimal? AverageOfferAmount { get; set; }
}

/// <summary>
/// Revenue analytics DTO.
/// </summary>
public class RevenueAnalyticsDto
{
    public DateTime Date { get; set; }
    public decimal GrossRevenue { get; set; }
    public decimal Refunds { get; set; }
    public decimal NetRevenue { get; set; }
    public decimal SubscriptionRevenue { get; set; }
    public decimal AdvertisementRevenue { get; set; }
    public decimal ListingPromotionRevenue { get; set; }
    public decimal OtherRevenue { get; set; }
}

/// <summary>
/// Platform analytics DTO.
/// </summary>
public class PlatformAnalyticsDto
{
    public DateTime Date { get; set; }
    public int NewUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int TotalListings { get; set; }
    public int PublishedListings { get; set; }
    public int Enquiries { get; set; }
    public int ServiceBookings { get; set; }
    public int ActiveAdvertisements { get; set; }
    public int ActiveSubscriptions { get; set; }
    public int SellRequests { get; set; }
    public decimal Revenue { get; set; }
}

/// <summary>
/// Analytics query parameters.
/// </summary>
public class AnalyticsQueryParams
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string? Period { get; set; }
    public string? GroupBy { get; set; }
}
