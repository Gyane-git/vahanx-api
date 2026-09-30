using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// User notification preferences.
/// </summary>
public class NotificationPreference : BaseEntity
{
    public Guid UserId { get; set; }

    public NotificationType NotificationType { get; set; }

    public bool PushEnabled { get; set; } = true;

    public bool EmailEnabled { get; set; } = true;

    public bool SmsEnabled { get; set; }

    public bool InAppEnabled { get; set; } = true;
}
