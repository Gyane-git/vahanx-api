using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Audit trail for sell request status changes. Append-only.
/// </summary>
public class SellRequestStatusHistory : BaseEntity
{
    public Guid SellRequestId { get; set; }

    public SellRequest? SellRequest { get; set; }

    public SellRequestStatus? OldStatus { get; set; }

    public SellRequestStatus NewStatus { get; set; }

    public string? Reason { get; set; }

    public Guid? ChangedByUserId { get; set; }

    public DateTime ChangedAt { get; set; }
}
