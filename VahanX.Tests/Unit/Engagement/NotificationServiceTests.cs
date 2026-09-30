using Microsoft.EntityFrameworkCore;
using VahanX.Application.DTOs.Engagement;
using VahanX.Application.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;
using VahanX.Infrastructure.Persistence;
using VahanX.Infrastructure.Persistence.Repositories;
using Xunit;

namespace VahanX.Tests.Unit.Engagement;

/// <summary>
/// Unit tests for NotificationService.
/// </summary>
public class NotificationServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly NotificationService _service;

    public NotificationServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var notificationRepository = new Repository<Notification>(_context);
        var preferenceRepository = new Repository<NotificationPreference>(_context);
        var pushTokenRepository = new Repository<PushToken>(_context);

        _service = new NotificationService(notificationRepository, preferenceRepository, pushTokenRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task MarkAsReadAsync_WithValidNotification_MarksAsRead()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = NotificationType.NewEnquiry,
            Title = "New Enquiry",
            Message = "You have a new enquiry.",
            IsRead = false
        };
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.MarkAsReadAsync(notification.Id, userId);

        // Assert
        Assert.True(result.IsRead);
        Assert.NotNull(result.ReadAt);
    }

    [Fact]
    public async Task MarkAsReadAsync_WithDifferentUser_ThrowsForbiddenException()
    {
        // Arrange
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Type = NotificationType.NewEnquiry,
            Title = "New Enquiry",
            Message = "You have a new enquiry.",
            IsRead = false
        };
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(() => _service.MarkAsReadAsync(notification.Id, Guid.NewGuid()));
    }

    [Fact]
    public async Task GetUnreadCountAsync_WithUnreadNotifications_ReturnsCorrectCount()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notification1 = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = NotificationType.NewEnquiry,
            Title = "New Enquiry",
            Message = "You have a new enquiry.",
            IsRead = false
        };
        var notification2 = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = NotificationType.NewMessage,
            Title = "New Message",
            Message = "You have a new message.",
            IsRead = false
        };
        var notification3 = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = NotificationType.ListingPublished,
            Title = "Listing Published",
            Message = "Your listing is published.",
            IsRead = true,
            ReadAt = DateTime.UtcNow
        };
        await _context.Notifications.AddRangeAsync(notification1, notification2, notification3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetUnreadCountAsync(userId);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task RegisterPushTokenAsync_WithValidRequest_RegistersToken()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new RegisterPushTokenRequest
        {
            Token = "test-push-token-123",
            Platform = PushPlatform.Android,
            DeviceId = "device-123"
        };

        // Act
        var result = await _service.RegisterPushTokenAsync(userId, request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("test-push-token-123", result.Token);
        Assert.Equal(PushPlatform.Android, result.Platform);
    }

    [Fact]
    public async Task RegisterPushTokenAsync_WithValidRequest_ReturnsActiveToken()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new RegisterPushTokenRequest
        {
            Token = "test-push-token-456",
            Platform = PushPlatform.iOS,
            DeviceId = "device-456"
        };

        // Act
        var result = await _service.RegisterPushTokenAsync(userId, request);

        // Assert
        Assert.Equal("test-push-token-456", result.Token);
        Assert.Equal(PushPlatform.iOS, result.Platform);
        Assert.True(result.IsActive);
    }
}
