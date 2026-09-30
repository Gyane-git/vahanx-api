using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Services;

/// <summary>
/// Service booking response DTO.
/// </summary>
public class ServiceBookingResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ServiceCenterBranchId { get; set; }
    public string ServiceCenterBranchName { get; set; } = string.Empty;
    public Guid? VehicleId { get; set; }
    public string? VehicleName { get; set; }
    public Guid? AutoServiceTypeId { get; set; }
    public string? AutoServiceTypeName { get; set; }
    public Guid? ServicePackageId { get; set; }
    public string? ServicePackageName { get; set; }
    public string BookingReference { get; set; } = string.Empty;
    public DateTime RequestedDate { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public ServiceBookingStatus Status { get; set; }
    public string? CustomerNotes { get; set; }
    public string? CenterNotes { get; set; }
    public decimal? EstimatedPrice { get; set; }
    public decimal? FinalPrice { get; set; }
    public string? Currency { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Create service booking request.
/// </summary>
public class CreateServiceBookingRequest
{
    public Guid ServiceCenterBranchId { get; set; }
    public Guid? VehicleId { get; set; }
    public Guid? AutoServiceTypeId { get; set; }
    public Guid? ServicePackageId { get; set; }
    public DateTime RequestedDate { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? CustomerNotes { get; set; }
}

/// <summary>
/// Update service booking request.
/// </summary>
public class UpdateServiceBookingRequest
{
    public DateTime? RequestedDate { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? CustomerNotes { get; set; }
}

/// <summary>
/// Reschedule service booking request.
/// </summary>
public class RescheduleServiceBookingRequest
{
    public DateTime NewStartTime { get; set; }
    public DateTime NewEndTime { get; set; }
}
