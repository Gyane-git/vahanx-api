using VahanX.Application.Common;
using VahanX.Application.DTOs.Engagement;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for notification operations.
/// </summary>
public interface INotificationService
{
    Task<PagedResult<NotificationResponse>> GetNotificationsAsync(Guid userId, int page, int pageSize, bool? unreadOnly = null, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<NotificationResponse> MarkAsReadAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<PagedResult<NotificationPreferenceResponse>> GetPreferencesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<NotificationPreferenceResponse> UpdatePreferenceAsync(Guid userId, UpdateNotificationPreferenceRequest request, CancellationToken cancellationToken = default);
    Task<PushTokenResponse> RegisterPushTokenAsync(Guid userId, RegisterPushTokenRequest request, CancellationToken cancellationToken = default);
    Task DeletePushTokenAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
}
