using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Physical/technical inspection of a vehicle.
/// </summary>
public class Inspection : BaseEntity
{
    public Guid VehicleId { get; set; }

    public Vehicle? Vehicle { get; set; }

    public Guid? ListingId { get; set; }

    public VehicleListing? Listing { get; set; }

    public InspectionType InspectionType { get; set; }

    public InspectionStatus Status { get; set; } = InspectionStatus.Scheduled;

    public Guid? InspectorUserId { get; set; }

    public DateTime? InspectionDate { get; set; }

    public DateTime? CompletedAt { get; set; }

    public decimal? OverallScore { get; set; }

    public string? Summary { get; set; }

    public string? Notes { get; set; }

    public ICollection<InspectionItem> Items { get; set; } = new List<InspectionItem>();

    public ICollection<InspectionMedia> Media { get; set; } = new List<InspectionMedia>();

    public InspectionReport? Report { get; set; }
}
