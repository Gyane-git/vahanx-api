using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Marketplace;

/// <summary>
/// Dealer response DTO.
/// </summary>
public class DealerResponse
{
    public Guid Id { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string? LegalName { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? Description { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Website { get; set; }
    public Guid? LocationId { get; set; }
    public string? LocationName { get; set; }
    public VerificationStatus VerificationStatus { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Create dealer request.
/// </summary>
public class CreateDealerRequest
{
    public string BusinessName { get; set; } = string.Empty;
    public string? LegalName { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? Description { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Website { get; set; }
    public Guid? LocationId { get; set; }
}

/// <summary>
/// Update dealer request.
/// </summary>
public class UpdateDealerRequest
{
    public string BusinessName { get; set; } = string.Empty;
    public string? LegalName { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? Description { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Website { get; set; }
    public Guid? LocationId { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Dealer branch response DTO.
/// </summary>
public class DealerBranchResponse
{
    public Guid Id { get; set; }
    public Guid DealerId { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public Guid? LocationId { get; set; }
    public string? LocationName { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Create dealer branch request.
/// </summary>
public class CreateDealerBranchRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public Guid? LocationId { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}

/// <summary>
/// Update dealer branch request.
/// </summary>
public class UpdateDealerBranchRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public Guid? LocationId { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool IsActive { get; set; } = true;
}
