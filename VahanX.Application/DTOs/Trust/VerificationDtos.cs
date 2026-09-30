using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Trust;

/// <summary>
/// Verification response DTO (internal).
/// </summary>
public class VerificationResponse
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public Guid? ListingId { get; set; }
    public VehicleVerificationStatus Status { get; set; }
    public VerificationType VerificationType { get; set; }
    public Guid? VerifiedByUserId { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public DateTime? ExpiryAt { get; set; }
    public string? VerificationReference { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Public verification response DTO.
/// Excludes sensitive internal information.
/// </summary>
public class PublicVerificationResponse
{
    public Guid Id { get; set; }
    public VehicleVerificationStatus Status { get; set; }
    public VerificationType VerificationType { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public DateTime? ExpiryAt { get; set; }
    public string? VerificationReference { get; set; }
    public bool IsVerified => Status == VehicleVerificationStatus.Verified;
}

/// <summary>
/// Create verification request.
/// </summary>
public class CreateVerificationRequest
{
    public Guid VehicleId { get; set; }
    public Guid? ListingId { get; set; }
    public VerificationType VerificationType { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Update verification request.
/// </summary>
public class UpdateVerificationRequest
{
    public VerificationType? VerificationType { get; set; }
    public string? Notes { get; set; }
    public DateTime? ExpiryAt { get; set; }
}

/// <summary>
/// Verification document response DTO.
/// </summary>
public class VerificationDocumentResponse
{
    public Guid Id { get; set; }
    public Guid VerificationId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string? DocumentMediaReference { get; set; }
    public DateTime? IssuedDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Create verification document request.
/// </summary>
public class CreateVerificationDocumentRequest
{
    public string DocumentType { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; }
    public string? DocumentMediaReference { get; set; }
    public DateTime? IssuedDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
}
