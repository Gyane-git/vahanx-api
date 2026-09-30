using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Vehicle service history record.
/// Append-only.
/// </summary>
public class ServiceHistory : BaseEntity
{
    public Guid VehicleId { get; set; }

    public Vehicle? Vehicle { get; set; }

    public DateTime ServiceDate { get; set; }

    public int? Mileage { get; set; }

    public ServiceType ServiceType { get; set; }

    public string? ServiceProvider { get; set; }

    public string? Description { get; set; }

    public decimal? Cost { get; set; }

    public HistorySource Source { get; set; } = HistorySource.SellerProvided;

    public string? Notes { get; set; }
}
