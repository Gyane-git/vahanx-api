using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Offer made on a sell request.
/// </summary>
public class SellOffer : BaseEntity
{
    public Guid SellRequestId { get; set; }

    public SellRequest? SellRequest { get; set; }

    public Guid? OfferedByUserId { get; set; }

    public decimal OfferedAmount { get; set; }

    public string Currency { get; set; } = "NPR";

    public DateTime? ValidUntil { get; set; }

    public string? Notes { get; set; }

    public SellOfferStatus Status { get; set; } = SellOfferStatus.Draft;

    public DateTime? AcceptedAt { get; set; }

    public DateTime? RejectedAt { get; set; }
}
