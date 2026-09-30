using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// An item in a user's wishlist.
/// Links a wishlist to a vehicle listing.
/// </summary>
public class WishlistItem : BaseEntity
{
    public Guid WishlistId { get; set; }

    public Wishlist? Wishlist { get; set; }

    public Guid ListingId { get; set; }

    public VehicleListing? Listing { get; set; }
}
