using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Advertisement click tracking.
/// </summary>
public class AdvertisementClick : BaseEntity
{
    public Guid AdvertisementId { get; set; }

    public Advertisement? Advertisement { get; set; }

    public Guid? UserId { get; set; }

    public string? SessionReference { get; set; }

    public Guid PlacementId { get; set; }

    public DateTime OccurredAt { get; set; }
}
