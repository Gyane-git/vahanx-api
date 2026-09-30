using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Advertisement impression tracking.
/// </summary>
public class AdvertisementImpression : BaseEntity
{
    public Guid AdvertisementId { get; set; }

    public Advertisement? Advertisement { get; set; }

    public Guid? UserId { get; set; }

    public string? SessionReference { get; set; }

    public Guid PlacementId { get; set; }

    public DateTime OccurredAt { get; set; }
}
