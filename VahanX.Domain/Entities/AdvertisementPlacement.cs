using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Advertisement placement configuration.
/// </summary>
public class AdvertisementPlacement : BaseEntity
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public AdvertisementPlacementType PlacementType { get; set; }

    public bool IsActive { get; set; } = true;

    public int MaxAdsPerRequest { get; set; } = 3;
}
