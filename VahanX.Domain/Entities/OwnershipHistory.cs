using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Vehicle ownership history record.
/// Append-only.
/// </summary>
public class OwnershipHistory : BaseEntity
{
    public Guid VehicleId { get; set; }

    public Vehicle? Vehicle { get; set; }

    public int OwnerSequence { get; set; }

    public OwnershipType OwnershipType { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public HistorySource Source { get; set; } = HistorySource.SellerProvided;

    public string? Notes { get; set; }
}
