using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Vehicle insurance history record.
/// Append-only.
/// </summary>
public class InsuranceHistory : BaseEntity
{
    public Guid VehicleId { get; set; }

    public Vehicle? Vehicle { get; set; }

    public string? Provider { get; set; }

    public string? PolicyReference { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public InsuranceStatus Status { get; set; } = InsuranceStatus.Unknown;

    public HistorySource Source { get; set; } = HistorySource.SellerProvided;
}
