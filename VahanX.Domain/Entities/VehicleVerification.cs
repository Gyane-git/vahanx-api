using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// VahanX verification record for a vehicle/listing.
/// Represents the verification process and its outcome.
/// </summary>
public class VehicleVerification : BaseEntity
{
    public Guid VehicleId { get; set; }

    public Vehicle? Vehicle { get; set; }

    public Guid? ListingId { get; set; }

    public VehicleListing? Listing { get; set; }

    public VehicleVerificationStatus Status { get; set; } = VehicleVerificationStatus.NotVerified;

    public VerificationType VerificationType { get; set; }

    public Guid? VerifiedByUserId { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public DateTime? ExpiryAt { get; set; }

    public string? VerificationReference { get; set; }

    public string? Notes { get; set; }

    public ICollection<VerificationDocument> Documents { get; set; } = new List<VerificationDocument>();

    public ICollection<VehicleVerificationStatusHistory> StatusHistory { get; set; } = new List<VehicleVerificationStatusHistory>();
}
