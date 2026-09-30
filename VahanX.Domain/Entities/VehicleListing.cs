using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Marketplace vehicle listing.
/// Represents a specific offer for a vehicle, distinct from the canonical Vehicle definition.
/// A single Vehicle may have multiple listings over time.
/// </summary>
public class VehicleListing : BaseEntity
{
    public Guid VehicleId { get; set; }

    public Vehicle? Vehicle { get; set; }

    public Guid? SellerId { get; set; }

    public Seller? Seller { get; set; }

    public Guid? DealerId { get; set; }

    public Dealer? Dealer { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = "NPR";

    public int Mileage { get; set; }

    public string MileageUnit { get; set; } = "km";

    public int ManufactureYear { get; set; }

    public int? RegistrationYear { get; set; }

    public Condition Condition { get; set; } = Condition.Used;

    public ListingStatus Status { get; set; } = ListingStatus.Draft;

    public Guid? LocationId { get; set; }

    public Location? Location { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public ContactPreference ContactPreference { get; set; } = ContactPreference.Phone;

    public bool IsNegotiable { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public ICollection<ListingMedia> Media { get; set; } = new List<ListingMedia>();

    public ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();

    public ICollection<CompareItem> CompareItems { get; set; } = new List<CompareItem>();
}
