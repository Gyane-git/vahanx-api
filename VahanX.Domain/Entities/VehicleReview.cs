using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Review of a vehicle/listing.
/// </summary>
public class VehicleReview : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid VehicleId { get; set; }

    public Vehicle? Vehicle { get; set; }

    public Guid? ListingId { get; set; }

    public VehicleListing? Listing { get; set; }

    public int Rating { get; set; }

    public string? Title { get; set; }

    public string? Comment { get; set; }

    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
}
