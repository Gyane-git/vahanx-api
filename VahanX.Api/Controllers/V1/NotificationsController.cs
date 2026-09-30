using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Engagement;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for notification operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _service;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(INotificationService service, ILogger<NotificationsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get notifications for the current user.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<NotificationResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<NotificationResponse>>>> GetNotifications(
        [FromQuery] Guid userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] bool? unreadOnly = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetNotificationsAsync(userId, page, pageSize, unreadOnly, cancellationToken);
        return Ok(ApiResponse<PagedResult<NotificationResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get unread notification count.
    /// </summary>
    [HttpGet("unread-count")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<int>>> GetUnreadCount(
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetUnreadCountAsync(userId, cancellationToken);
        return Ok(ApiResponse<int>.SuccessResponse(result));
    }

    /// <summary>
    /// Mark a notification as read.
    /// </summary>
    [HttpPost("{id:guid}/read")]
    [ProducesResponseType(typeof(ApiResponse<NotificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<NotificationResponse>>> MarkAsRead(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.MarkAsReadAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<NotificationResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Mark all notifications as read.
    /// </summary>
    [HttpPost("read-all")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> MarkAllAsRead(
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        await _service.MarkAllAsReadAsync(userId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Get notification preferences.
    /// </summary>
    [HttpGet("preferences")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<NotificationPreferenceResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<NotificationPreferenceResponse>>>> GetPreferences(
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetPreferencesAsync(userId, cancellationToken);
        return Ok(ApiResponse<PagedResult<NotificationPreferenceResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Update notification preferences.
    /// </summary>
    [HttpPut("preferences")]
    [ProducesResponseType(typeof(ApiResponse<NotificationPreferenceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<NotificationPreferenceResponse>>> UpdatePreferences(
        [FromQuery] Guid userId,
        [FromBody] UpdateNotificationPreferenceRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdatePreferenceAsync(userId, request, cancellationToken);
        return Ok(ApiResponse<NotificationPreferenceResponse>.SuccessResponse(result, "Notification preference updated successfully."));
    }

    /// <summary>
    /// Register a push token.
    /// </summary>
    [HttpPost("push-tokens")]
    [ProducesResponseType(typeof(ApiResponse<PushTokenResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<PushTokenResponse>>> RegisterPushToken(
        [FromQuery] Guid userId,
        [FromBody] RegisterPushTokenRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.RegisterPushTokenAsync(userId, request, cancellationToken);
        return CreatedAtAction(nameof(RegisterPushToken), new { userId }, ApiResponse<PushTokenResponse>.SuccessResponse(result, "Push token registered successfully."));
    }

    /// <summary>
    /// Delete a push token.
    /// </summary>
    [HttpDelete("push-tokens/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeletePushToken(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        await _service.DeletePushTokenAsync(id, userId, cancellationToken);
        return NoContent();
    }
}
