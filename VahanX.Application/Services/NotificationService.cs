using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Engagement;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of notification service.
/// </summary>
public class NotificationService : INotificationService
{
    private readonly IRepository<Notification> _notificationRepository;
    private readonly IRepository<NotificationPreference> _preferenceRepository;
    private readonly IRepository<PushToken> _pushTokenRepository;

    public NotificationService(
        IRepository<Notification> notificationRepository,
        IRepository<NotificationPreference> preferenceRepository,
        IRepository<PushToken> pushTokenRepository)
    {
        _notificationRepository = notificationRepository;
        _preferenceRepository = preferenceRepository;
        _pushTokenRepository = pushTokenRepository;
    }

    public async Task<PagedResult<NotificationResponse>> GetNotificationsAsync(Guid userId, int page, int pageSize, bool? unreadOnly = null, CancellationToken cancellationToken = default)
    {
        var query = await _notificationRepository.QueryAsync(true, cancellationToken);

        query = query.Where(n => n.UserId == userId);

        if (unreadOnly == true)
            query = query.Where(n => !n.IsRead);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new NotificationResponse
            {
                Id = n.Id,
                UserId = n.UserId,
                Type = n.Type,
                Title = n.Title,
                Message = n.Message,
                Data = n.Data,
                IsRead = n.IsRead,
                ReadAt = n.ReadAt,
                CreatedAt = n.CreatedAt,
                ExpiresAt = n.ExpiresAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<NotificationResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<int> GetUnreadCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var notifications = await _notificationRepository.QueryAsync(true, cancellationToken);
        return await notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .CountAsync(cancellationToken);
    }

    public async Task<NotificationResponse> MarkAsReadAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var notification = await _notificationRepository.GetByIdAsync(id, cancellationToken);
        if (notification is null) throw new NotFoundException("Notification", id);

        if (notification.UserId != userId)
            throw new ForbiddenException("You can only mark your own notifications as read.");

        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;
        await _notificationRepository.UpdateAsync(notification, cancellationToken);

        return new NotificationResponse
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Type = notification.Type,
            Title = notification.Title,
            Message = notification.Message,
            Data = notification.Data,
            IsRead = notification.IsRead,
            ReadAt = notification.ReadAt,
            CreatedAt = notification.CreatedAt,
            ExpiresAt = notification.ExpiresAt
        };
    }

    public async Task MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var notifications = await _notificationRepository.QueryAsync(true, cancellationToken);
        var unreadNotifications = await notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync(cancellationToken);

        foreach (var notification in unreadNotifications)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await _notificationRepository.UpdateAsync(notification, cancellationToken);
        }
    }

    public async Task<PagedResult<NotificationPreferenceResponse>> GetPreferencesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var preferences = await _preferenceRepository.QueryAsync(true, cancellationToken);
        var userPreferences = await preferences
            .Where(p => p.UserId == userId)
            .ToListAsync(cancellationToken);

        var items = userPreferences.Select(p => new NotificationPreferenceResponse
        {
            Id = p.Id,
            UserId = p.UserId,
            NotificationType = p.NotificationType,
            PushEnabled = p.PushEnabled,
            EmailEnabled = p.EmailEnabled,
            SmsEnabled = p.SmsEnabled,
            InAppEnabled = p.InAppEnabled
        }).ToList();

        return PagedResult<NotificationPreferenceResponse>.Create(items, items.Count, 1, items.Count);
    }

    public async Task<NotificationPreferenceResponse> UpdatePreferenceAsync(Guid userId, UpdateNotificationPreferenceRequest request, CancellationToken cancellationToken = default)
    {
        var preferences = await _preferenceRepository.QueryAsync(true, cancellationToken);
        var preference = await preferences
            .FirstOrDefaultAsync(p => p.UserId == userId && p.NotificationType == request.NotificationType, cancellationToken);

        if (preference is null)
        {
            preference = new NotificationPreference
            {
                UserId = userId,
                NotificationType = request.NotificationType
            };
            await _preferenceRepository.AddAsync(preference, cancellationToken);
        }

        if (request.PushEnabled.HasValue) preference.PushEnabled = request.PushEnabled.Value;
        if (request.EmailEnabled.HasValue) preference.EmailEnabled = request.EmailEnabled.Value;
        if (request.SmsEnabled.HasValue) preference.SmsEnabled = request.SmsEnabled.Value;
        if (request.InAppEnabled.HasValue) preference.InAppEnabled = request.InAppEnabled.Value;

        await _preferenceRepository.UpdateAsync(preference, cancellationToken);

        return new NotificationPreferenceResponse
        {
            Id = preference.Id,
            UserId = preference.UserId,
            NotificationType = preference.NotificationType,
            PushEnabled = preference.PushEnabled,
            EmailEnabled = preference.EmailEnabled,
            SmsEnabled = preference.SmsEnabled,
            InAppEnabled = preference.InAppEnabled
        };
    }

    public async Task<PushTokenResponse> RegisterPushTokenAsync(Guid userId, RegisterPushTokenRequest request, CancellationToken cancellationToken = default)
    {
        var pushTokens = await _pushTokenRepository.QueryAsync(true, cancellationToken);
        var existingToken = await pushTokens
            .FirstOrDefaultAsync(t => t.UserId == userId && t.Token == request.Token, cancellationToken);

        if (existingToken != null)
        {
            existingToken.IsActive = true;
            existingToken.LastUsedAt = DateTime.UtcNow;
            await _pushTokenRepository.UpdateAsync(existingToken, cancellationToken);

            return new PushTokenResponse
            {
                Id = existingToken.Id,
                UserId = existingToken.UserId,
                Token = existingToken.Token,
                Platform = existingToken.Platform,
                DeviceId = existingToken.DeviceId,
                IsActive = existingToken.IsActive,
                LastUsedAt = existingToken.LastUsedAt,
                CreatedAt = existingToken.CreatedAt
            };
        }

        var pushToken = new PushToken
        {
            UserId = userId,
            Token = request.Token,
            Platform = request.Platform,
            DeviceId = request.DeviceId,
            IsActive = true,
            LastUsedAt = DateTime.UtcNow
        };

        await _pushTokenRepository.AddAsync(pushToken, cancellationToken);

        return new PushTokenResponse
        {
            Id = pushToken.Id,
            UserId = pushToken.UserId,
            Token = pushToken.Token,
            Platform = pushToken.Platform,
            DeviceId = pushToken.DeviceId,
            IsActive = pushToken.IsActive,
            LastUsedAt = pushToken.LastUsedAt,
            CreatedAt = pushToken.CreatedAt
        };
    }

    public async Task DeletePushTokenAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var pushToken = await _pushTokenRepository.GetByIdAsync(id, cancellationToken);
        if (pushToken is null) throw new NotFoundException("PushToken", id);

        if (pushToken.UserId != userId)
            throw new ForbiddenException("You can only delete your own push tokens.");

        await _pushTokenRepository.DeleteAsync(pushToken, cancellationToken);
    }
}
