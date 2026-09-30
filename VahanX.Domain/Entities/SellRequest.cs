using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Sell vehicle request from an owner.
/// </summary>
public class SellRequest : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid SellVehicleId { get; set; }

    public SellVehicle? SellVehicle { get; set; }

    public ContactPreference PreferredContactMethod { get; set; } = ContactPreference.Phone;

    public string? PreferredContactTime { get; set; }

    public Guid? LocationId { get; set; }

    public Location? Location { get; set; }

    public SellRequestStatus Status { get; set; } = SellRequestStatus.Draft;

    public string? Notes { get; set; }

    public DateTime? ClosedAt { get; set; }

    public ICollection<SellOffer> Offers { get; set; } = new List<SellOffer>();

    public ICollection<SellRequestStatusHistory> StatusHistory { get; set; } = new List<SellRequestStatusHistory>();
}
