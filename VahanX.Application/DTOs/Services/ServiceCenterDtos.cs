using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Services;

/// <summary>
/// Service center list DTO.
/// </summary>
public class ServiceCenterListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ContactPhone { get; set; } = string.Empty;
    public string? ContactEmail { get; set; }
    public string? WebsiteUrl { get; set; }
    public VerificationStatus VerificationStatus { get; set; }
    public ServiceCenterStatus Status { get; set; }
    public double? AverageRating { get; set; }
    public int TotalReviews { get; set; }
}

/// <summary>
/// Service center details DTO.
/// </summary>
public class ServiceCenterDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? BusinessRegistrationNumber { get; set; }
    public string ContactPhone { get; set; } = string.Empty;
    public string? ContactEmail { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? LogoMediaReference { get; set; }
    public VerificationStatus VerificationStatus { get; set; }
    public ServiceCenterStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Create service center request.
/// </summary>
public class CreateServiceCenterRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? BusinessRegistrationNumber { get; set; }
    public string ContactPhone { get; set; } = string.Empty;
    public string? ContactEmail { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? LogoMediaReference { get; set; }
}

/// <summary>
/// Update service center request.
/// </summary>
public class UpdateServiceCenterRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? BusinessRegistrationNumber { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? LogoMediaReference { get; set; }
}

/// <summary>
/// Service center branch response DTO.
/// </summary>
public class ServiceCenterBranchResponse
{
    public Guid Id { get; set; }
    public Guid ServiceCenterId { get; set; }
    public string ServiceCenterName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? LocationId { get; set; }
    public string? LocationName { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public ServiceCenterStatus Status { get; set; }
}

/// <summary>
/// Create service center branch request.
/// </summary>
public class CreateServiceCenterBranchRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? LocationId { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
}

/// <summary>
/// Update service center branch request.
/// </summary>
public class UpdateServiceCenterBranchRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid? LocationId { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
}

/// <summary>
/// Auto service type response DTO.
/// </summary>
public class AutoServiceTypeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? VehicleTypeId { get; set; }
    public string? VehicleTypeName { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
}

/// <summary>
/// Service center service response DTO.
/// </summary>
public class ServiceCenterServiceResponse
{
    public Guid Id { get; set; }
    public Guid ServiceCenterBranchId { get; set; }
    public Guid AutoServiceTypeId { get; set; }
    public string AutoServiceTypeName { get; set; } = string.Empty;
    public decimal? PriceFrom { get; set; }
    public decimal? PriceTo { get; set; }
    public int? EstimatedDurationMinutes { get; set; }
    public bool IsAvailable { get; set; }
    public string? Description { get; set; }
}

/// <summary>
/// Service package response DTO.
/// </summary>
public class ServicePackageResponse
{
    public Guid Id { get; set; }
    public Guid ServiceCenterBranchId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public int? EstimatedDurationMinutes { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// Service working hour response DTO.
/// </summary>
public class ServiceWorkingHourResponse
{
    public Guid Id { get; set; }
    public Guid ServiceCenterBranchId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
    public bool IsClosed { get; set; }
}
