using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Audit trail for verification status changes.
/// Append-only record.
/// </summary>
public class VehicleVerificationStatusHistory : BaseEntity
{
    public Guid VerificationId { get; set; }

    public VehicleVerification? Verification { get; set; }

    public VehicleVerificationStatus OldStatus { get; set; }

    public VehicleVerificationStatus NewStatus { get; set; }

    public Guid? ChangedBy { get; set; }

    public DateTime ChangedAt { get; set; }

    public string? Reason { get; set; }
}
