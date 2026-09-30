using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Vehicle accident history record.
/// Append-only.
/// </summary>
public class AccidentHistory : BaseEntity
{
    public Guid VehicleId { get; set; }

    public Vehicle? Vehicle { get; set; }

    public DateTime? AccidentDate { get; set; }

    public AccidentSeverity Severity { get; set; } = AccidentSeverity.Unknown;

    public string? Description { get; set; }

    public RepairStatus RepairStatus { get; set; } = RepairStatus.Unknown;

    public decimal? RepairCost { get; set; }

    public HistorySource Source { get; set; } = HistorySource.SellerProvided;

    public string? Notes { get; set; }
}
