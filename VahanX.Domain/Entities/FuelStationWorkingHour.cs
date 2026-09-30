using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Working hours for a fuel station.
/// </summary>
public class FuelStationWorkingHour : BaseEntity
{
    public Guid FuelStationId { get; set; }

    public FuelStation? FuelStation { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public TimeSpan OpeningTime { get; set; }

    public TimeSpan ClosingTime { get; set; }

    public bool IsClosed { get; set; }
}
