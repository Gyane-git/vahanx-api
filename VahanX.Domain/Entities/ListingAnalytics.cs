using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Aggregated analytics for a vehicle listing.
/// </summary>
public class ListingAnalytics : BaseEntity
{
    public Guid ListingId { get; set; }
    public long Views { get; set; }
    public long UniqueViews { get; set; }
    public long Shares { get; set; }
    public long WishlistAdds { get; set; }
    public long CompareAdds { get; set; }
    public long Enquiries { get; set; }
    public long TestDriveRequests { get; set; }
    public long ContactRequests { get; set; }
    public double AverageDailyViews { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime? LastViewedAt { get; set; }
}
