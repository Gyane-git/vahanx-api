using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Service booking at a service center branch.
/// </summary>
public class ServiceBooking : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid ServiceCenterBranchId { get; set; }

    public ServiceCenterBranch? ServiceCenterBranch { get; set; }

    public Guid? VehicleId { get; set; }

    public Vehicle? Vehicle { get; set; }

    public Guid? AutoServiceTypeId { get; set; }

    public AutoServiceType? AutoServiceType { get; set; }

    public Guid? ServicePackageId { get; set; }

    public ServicePackage? ServicePackage { get; set; }

    public string BookingReference { get; set; } = string.Empty;

    public DateTime RequestedDate { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public ServiceBookingStatus Status { get; set; } = ServiceBookingStatus.Requested;

    public string? CustomerNotes { get; set; }

    public string? CenterNotes { get; set; }

    public decimal? EstimatedPrice { get; set; }

    public decimal? FinalPrice { get; set; }

    public string? Currency { get; set; }

    public DateTime? ConfirmedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public string? CancellationReason { get; set; }
}
