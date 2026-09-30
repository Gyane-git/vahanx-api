using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Test drive request for a vehicle listing.
/// </summary>
public class TestDrive : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid ListingId { get; set; }

    public VehicleListing? Listing { get; set; }

    public Guid? SellerId { get; set; }

    public Seller? Seller { get; set; }

    public Guid? DealerId { get; set; }

    public Dealer? Dealer { get; set; }

    public DateTime RequestedDate { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public Guid? LocationId { get; set; }

    public Location? Location { get; set; }

    public string? Notes { get; set; }

    public TestDriveStatus Status { get; set; } = TestDriveStatus.Requested;

    public DateTime? CancelledAt { get; set; }
}
