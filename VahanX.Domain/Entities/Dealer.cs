using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Business/showroom dealer entity.
/// A dealer can have multiple branches and staff.
/// </summary>
public class Dealer : BaseEntity
{
    public string BusinessName { get; set; } = string.Empty;

    public string? LegalName { get; set; }

    public string? RegistrationNumber { get; set; }

    public string? Description { get; set; }

    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Website { get; set; }

    public Guid? LocationId { get; set; }

    public Location? Location { get; set; }

    public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Unverified;

    public bool IsActive { get; set; } = true;

    public ICollection<DealerBranch> Branches { get; set; } = new List<DealerBranch>();

    public ICollection<DealerStaff> Staff { get; set; } = new List<DealerStaff>();

    public ICollection<VehicleListing> Listings { get; set; } = new List<VehicleListing>();
}
