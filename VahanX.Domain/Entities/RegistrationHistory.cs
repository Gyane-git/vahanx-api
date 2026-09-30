using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Vehicle registration history record.
/// Append-only.
/// </summary>
public class RegistrationHistory : BaseEntity
{
    public Guid VehicleId { get; set; }

    public Vehicle? Vehicle { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public RegistrationStatus RegistrationStatus { get; set; } = RegistrationStatus.Unknown;

    public string? RegistrationArea { get; set; }

    public string? RegistrationReference { get; set; }

    public HistorySource Source { get; set; } = HistorySource.SellerProvided;

    public string? Notes { get; set; }
}
