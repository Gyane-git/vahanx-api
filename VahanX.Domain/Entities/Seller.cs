using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Marketplace seller profile.
/// Represents an individual seller, separate from User authentication.
/// </summary>
public class Seller : BaseEntity
{
    public Guid UserId { get; set; }

    public string DisplayName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? ProfileImageUrl { get; set; }

    public string? Description { get; set; }

    public Guid? LocationId { get; set; }

    public Location? Location { get; set; }

    public ContactPreference PreferredContactMethod { get; set; } = ContactPreference.Phone;

    public SellerType SellerType { get; set; } = SellerType.Individual;

    public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Unverified;

    public bool IsActive { get; set; } = true;

    public ICollection<VehicleListing> Listings { get; set; } = new List<VehicleListing>();
}
