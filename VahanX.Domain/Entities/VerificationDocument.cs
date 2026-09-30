using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Supporting document for a verification.
/// Uses media reference, not binary storage.
/// </summary>
public class VerificationDocument : BaseEntity
{
    public Guid VerificationId { get; set; }

    public VehicleVerification? Verification { get; set; }

    public string DocumentType { get; set; } = string.Empty;

    public string? DocumentNumber { get; set; }

    public string? DocumentMediaReference { get; set; }

    public DateTime? IssuedDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? Status { get; set; }

    public string? Notes { get; set; }
}
