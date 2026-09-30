using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Marketplace price history for a listing.
/// Append-only. Never overwrite historical price records.
/// </summary>
public class PriceHistory : BaseEntity
{
    public Guid ListingId { get; set; }

    public VehicleListing? Listing { get; set; }

    public decimal OldPrice { get; set; }

    public decimal NewPrice { get; set; }

    public string Currency { get; set; } = "NPR";

    public DateTime ChangedAt { get; set; }

    public Guid? ChangedBy { get; set; }

    public string? Reason { get; set; }
}
