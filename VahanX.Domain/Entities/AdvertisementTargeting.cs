using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Targeting rule for an advertisement.
/// </summary>
public class AdvertisementTargeting : BaseEntity
{
    public Guid AdvertisementId { get; set; }

    public Advertisement? Advertisement { get; set; }

    public string TargetType { get; set; } = string.Empty;

    public string TargetValue { get; set; } = string.Empty;
}
