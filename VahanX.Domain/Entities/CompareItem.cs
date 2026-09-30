using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// An item in a user's compare list.
/// Links a compare list to a vehicle listing.
/// </summary>
public class CompareItem : BaseEntity
{
    public Guid CompareListId { get; set; }

    public CompareList? CompareList { get; set; }

    public Guid ListingId { get; set; }

    public VehicleListing? Listing { get; set; }
}
