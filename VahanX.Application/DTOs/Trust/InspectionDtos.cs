using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Trust;

/// <summary>
/// Inspection response DTO.
/// </summary>
public class InspectionResponse
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public Guid? ListingId { get; set; }
    public InspectionType InspectionType { get; set; }
    public InspectionStatus Status { get; set; }
    public Guid? InspectorUserId { get; set; }
    public DateTime? InspectionDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public decimal? OverallScore { get; set; }
    public string? Summary { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Create inspection request.
/// </summary>
public class CreateInspectionRequest
{
    public Guid VehicleId { get; set; }
    public Guid? ListingId { get; set; }
    public InspectionType InspectionType { get; set; }
    public DateTime? InspectionDate { get; set; }
}

/// <summary>
/// Update inspection request.
/// </summary>
public class UpdateInspectionRequest
{
    public InspectionType? InspectionType { get; set; }
    public string? Summary { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Inspection item response DTO.
/// </summary>
public class InspectionItemResponse
{
    public Guid Id { get; set; }
    public Guid InspectionId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public InspectionItemCondition Condition { get; set; }
    public decimal? Score { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Create inspection item request.
/// </summary>
public class CreateInspectionItemRequest
{
    public string Category { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public InspectionItemCondition Condition { get; set; } = InspectionItemCondition.NotInspected;
    public decimal? Score { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Update inspection item request.
/// </summary>
public class UpdateInspectionItemRequest
{
    public InspectionItemCondition? Condition { get; set; }
    public decimal? Score { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Inspection report response DTO.
/// </summary>
public class InspectionReportResponse
{
    public Guid Id { get; set; }
    public Guid InspectionId { get; set; }
    public decimal? OverallScore { get; set; }
    public string? Summary { get; set; }
    public string? Recommendations { get; set; }
    public ReportStatus ReportStatus { get; set; }
    public string? ReportReference { get; set; }
    public DateTime? GeneratedAt { get; set; }
}
