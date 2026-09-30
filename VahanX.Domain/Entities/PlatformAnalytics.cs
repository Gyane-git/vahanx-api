using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Daily platform-wide analytics summary.
/// </summary>
public class PlatformAnalytics : BaseEntity
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
