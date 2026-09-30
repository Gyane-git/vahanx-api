using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Creative asset for an advertisement.
/// </summary>
public class AdvertisementCreative : BaseEntity
{
    public Guid AdvertisementId { get; set; }

    public Advertisement? Advertisement { get; set; }

    public string MediaReference { get; set; } = string.Empty;

    public string MediaType { get; set; } = string.Empty;

    public string? AltText { get; set; }

    public int DisplayOrder { get; set; }
}
