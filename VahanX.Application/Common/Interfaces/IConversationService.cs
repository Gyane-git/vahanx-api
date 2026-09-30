using VahanX.Application.Common;
using VahanX.Application.DTOs.Engagement;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for conversation and messaging operations.
/// </summary>
public interface IConversationService
{
    Task<PagedResult<ConversationResponse>> GetConversationsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<ConversationResponse?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<ConversationResponse> CreateAsync(CreateConversationRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<PagedResult<MessageResponse>> GetMessagesAsync(Guid conversationId, Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<MessageResponse> SendMessageAsync(Guid conversationId, Guid userId, SendMessageRequest request, CancellationToken cancellationToken = default);
    Task<MessageResponse> UpdateMessageAsync(Guid messageId, Guid userId, UpdateMessageRequest request, CancellationToken cancellationToken = default);
    Task DeleteMessageAsync(Guid messageId, Guid userId, CancellationToken cancellationToken = default);
}
