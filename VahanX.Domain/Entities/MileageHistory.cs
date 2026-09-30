using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Vehicle mileage history record.
/// Append-only.
/// </summary>
public class MileageHistory : BaseEntity
{
    public Guid VehicleId { get; set; }

    public Vehicle? Vehicle { get; set; }

    public int Mileage { get; set; }

    public string MileageUnit { get; set; } = "km";

    public DateTime RecordedAt { get; set; }

    public HistorySource Source { get; set; } = HistorySource.SellerProvided;

    public Guid? RecordedBy { get; set; }

    public string? Notes { get; set; }
}
