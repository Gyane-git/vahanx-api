using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Notification template for multi-channel delivery.
/// </summary>
public class NotificationTemplate : BaseEntity
{
    public string Code { get; set; } = string.Empty;

    public string TitleTemplate { get; set; } = string.Empty;

    public string MessageTemplate { get; set; } = string.Empty;

    public NotificationChannel Channel { get; set; }

    public string Language { get; set; } = "en";

    public bool IsActive { get; set; } = true;
}
