using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Available time slot for test drives.
/// </summary>
public class TestDriveSlot : BaseEntity
{
    public Guid DealerId { get; set; }

    public Dealer? Dealer { get; set; }

    public DateTime Date { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int Capacity { get; set; } = 1;

    public bool IsAvailable { get; set; } = true;
}
