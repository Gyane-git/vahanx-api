using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Advertisement within a campaign.
/// </summary>
public class Advertisement : BaseEntity
{
    public Guid CampaignId { get; set; }

    public AdvertisementCampaign? Campaign { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Headline { get; set; }

    public string? Description { get; set; }

    public AdvertisementDestinationType DestinationType { get; set; } = AdvertisementDestinationType.Listing;

    public string? DestinationUrl { get; set; }

    public Guid? VehicleListingId { get; set; }

    public VehicleListing? VehicleListing { get; set; }

    public Guid? ServiceCenterId { get; set; }

    public Guid? ChargingStationId { get; set; }

    public Guid? FuelStationId { get; set; }

    public string? MediaReference { get; set; }

    public AdvertisementStatus Status { get; set; } = AdvertisementStatus.Draft;

    public ICollection<AdvertisementCreative> Creatives { get; set; } = new List<AdvertisementCreative>();

    public ICollection<AdvertisementTargeting> Targeting { get; set; } = new List<AdvertisementTargeting>();
}
