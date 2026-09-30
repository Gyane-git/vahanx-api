using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Engagement;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for conversation and messaging operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ConversationsController : ControllerBase
{
    private readonly IConversationService _service;
    private readonly ILogger<ConversationsController> _logger;

    public ConversationsController(IConversationService service, ILogger<ConversationsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get conversations for the current user.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ConversationResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ConversationResponse>>>> GetConversations(
        [FromQuery] Guid userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetConversationsAsync(userId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ConversationResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get conversation by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ConversationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<ConversationResponse>>> GetById(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, userId, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Conversation not found."));

        return Ok(ApiResponse<ConversationResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new conversation.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ConversationResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ConversationResponse>>> Create(
        [FromBody] CreateConversationRequest request,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id, userId }, ApiResponse<ConversationResponse>.SuccessResponse(result, "Conversation created successfully."));
    }

    /// <summary>
    /// Get messages in a conversation.
    /// </summary>
    [HttpGet("{id:guid}/messages")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<MessageResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PagedResult<MessageResponse>>>> GetMessages(
        Guid id,
        [FromQuery] Guid userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetMessagesAsync(id, userId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<MessageResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Send a message in a conversation.
    /// </summary>
    [HttpPost("{id:guid}/messages")]
    [ProducesResponseType(typeof(ApiResponse<MessageResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<MessageResponse>>> SendMessage(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] SendMessageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.SendMessageAsync(id, userId, request, cancellationToken);
        return CreatedAtAction(nameof(GetMessages), new { id, userId }, ApiResponse<MessageResponse>.SuccessResponse(result, "Message sent successfully."));
    }

    /// <summary>
    /// Update a message.
    /// </summary>
    [HttpPut("messages/{messageId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<MessageResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<MessageResponse>>> UpdateMessage(
        Guid messageId,
        [FromQuery] Guid userId,
        [FromBody] UpdateMessageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateMessageAsync(messageId, userId, request, cancellationToken);
        return Ok(ApiResponse<MessageResponse>.SuccessResponse(result, "Message updated successfully."));
    }

    /// <summary>
    /// Delete a message.
    /// </summary>
    [HttpDelete("messages/{messageId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteMessage(
        Guid messageId,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        await _service.DeleteMessageAsync(messageId, userId, cancellationToken);
        return NoContent();
    }
}
