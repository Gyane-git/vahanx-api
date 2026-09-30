using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Marketplace;

/// <summary>
/// Seller response DTO.
/// </summary>
public class SellerResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? Description { get; set; }
    public Guid? LocationId { get; set; }
    public string? LocationName { get; set; }
    public ContactPreference PreferredContactMethod { get; set; }
    public SellerType SellerType { get; set; }
    public VerificationStatus VerificationStatus { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Create seller request.
/// </summary>
public class CreateSellerRequest
{
    public Guid UserId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? Description { get; set; }
    public Guid? LocationId { get; set; }
    public ContactPreference PreferredContactMethod { get; set; } = ContactPreference.Phone;
    public SellerType SellerType { get; set; } = SellerType.Individual;
}

/// <summary>
/// Update seller request.
/// </summary>
public class UpdateSellerRequest
{
    public string DisplayName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? Description { get; set; }
    public Guid? LocationId { get; set; }
    public ContactPreference PreferredContactMethod { get; set; }
    public SellerType SellerType { get; set; }
    public bool IsActive { get; set; } = true;
}
