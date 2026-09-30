using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Engagement;

/// <summary>
/// Notification response DTO.
/// </summary>
public class NotificationResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Data { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

/// <summary>
/// Notification preference response DTO.
/// </summary>
public class NotificationPreferenceResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public NotificationType NotificationType { get; set; }
    public bool PushEnabled { get; set; }
    public bool EmailEnabled { get; set; }
    public bool SmsEnabled { get; set; }
    public bool InAppEnabled { get; set; }
}

/// <summary>
/// Update notification preference request.
/// </summary>
public class UpdateNotificationPreferenceRequest
{
    public NotificationType NotificationType { get; set; }
    public bool? PushEnabled { get; set; }
    public bool? EmailEnabled { get; set; }
    public bool? SmsEnabled { get; set; }
    public bool? InAppEnabled { get; set; }
}

/// <summary>
/// Push token response DTO.
/// </summary>
public class PushTokenResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public PushPlatform Platform { get; set; }
    public string? DeviceId { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastUsedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Register push token request.
/// </summary>
public class RegisterPushTokenRequest
{
    public string Token { get; set; } = string.Empty;
    public PushPlatform Platform { get; set; }
    public string? DeviceId { get; set; }
}
