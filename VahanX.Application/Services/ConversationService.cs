using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Engagement;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of conversation service.
/// </summary>
public class ConversationService : IConversationService
{
    private readonly IRepository<Conversation> _conversationRepository;
    private readonly IRepository<ConversationParticipant> _participantRepository;
    private readonly IRepository<Message> _messageRepository;

    public ConversationService(
        IRepository<Conversation> conversationRepository,
        IRepository<ConversationParticipant> participantRepository,
        IRepository<Message> messageRepository)
    {
        _conversationRepository = conversationRepository;
        _participantRepository = participantRepository;
        _messageRepository = messageRepository;
    }

    public async Task<PagedResult<ConversationResponse>> GetConversationsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var conversations = await _conversationRepository.QueryAsync(true, cancellationToken);
        var userConversations = await conversations
            .Where(c => c.Participants.Any(p => p.UserId == userId && p.IsActive))
            .OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt)
            .ToListAsync(cancellationToken);

        var totalCount = userConversations.Count;
        var items = userConversations
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ConversationResponse
            {
                Id = c.Id,
                ConversationType = c.ConversationType,
                EnquiryId = c.EnquiryId,
                ListingId = c.ListingId,
                ListingTitle = c.Listing != null ? c.Listing.Title : null,
                LastMessageAt = c.LastMessageAt,
                Status = c.Status,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .ToList();

        return PagedResult<ConversationResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<ConversationResponse?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var conversations = await _conversationRepository.QueryAsync(true, cancellationToken);
        var conversation = await conversations
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (conversation is null) return null;

        if (!conversation.Participants.Any(p => p.UserId == userId && p.IsActive))
            throw new ForbiddenException("You do not have access to this conversation.");

        return new ConversationResponse
        {
            Id = conversation.Id,
            ConversationType = conversation.ConversationType,
            EnquiryId = conversation.EnquiryId,
            ListingId = conversation.ListingId,
            ListingTitle = conversation.Listing != null ? conversation.Listing.Title : null,
            LastMessageAt = conversation.LastMessageAt,
            Status = conversation.Status,
            CreatedAt = conversation.CreatedAt,
            UpdatedAt = conversation.UpdatedAt
        };
    }

    public async Task<ConversationResponse> CreateAsync(CreateConversationRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var conversation = new Conversation
        {
            ConversationType = request.ConversationType,
            EnquiryId = request.EnquiryId,
            ListingId = request.ListingId,
            Status = Domain.Enums.ConversationStatus.Active
        };

        await _conversationRepository.AddAsync(conversation, cancellationToken);

        var participant = new ConversationParticipant
        {
            ConversationId = conversation.Id,
            UserId = userId,
            JoinedAt = DateTime.UtcNow,
            IsActive = true
        };
        await _participantRepository.AddAsync(participant, cancellationToken);

        foreach (var participantUserId in request.ParticipantUserIds.Where(id => id != userId))
        {
            var existingParticipant = await _participantRepository.AnyAsync(
                p => p.ConversationId == conversation.Id && p.UserId == participantUserId, cancellationToken);
            if (!existingParticipant)
            {
                await _participantRepository.AddAsync(new ConversationParticipant
                {
                    ConversationId = conversation.Id,
                    UserId = participantUserId,
                    JoinedAt = DateTime.UtcNow,
                    IsActive = true
                }, cancellationToken);
            }
        }

        return new ConversationResponse
        {
            Id = conversation.Id,
            ConversationType = conversation.ConversationType,
            EnquiryId = conversation.EnquiryId,
            ListingId = conversation.ListingId,
            LastMessageAt = conversation.LastMessageAt,
            Status = conversation.Status,
            CreatedAt = conversation.CreatedAt,
            UpdatedAt = conversation.UpdatedAt
        };
    }

    public async Task<PagedResult<MessageResponse>> GetMessagesAsync(Guid conversationId, Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var conversations = await _conversationRepository.QueryAsync(true, cancellationToken);
        var conversation = await conversations
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == conversationId, cancellationToken);

        if (conversation is null) throw new NotFoundException("Conversation", conversationId);

        if (!conversation.Participants.Any(p => p.UserId == userId && p.IsActive))
            throw new ForbiddenException("You do not have access to this conversation.");

        var messages = await _messageRepository.QueryAsync(true, cancellationToken);
        var filteredMessages = messages
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.SentAt);

        var totalCount = await filteredMessages.CountAsync(cancellationToken);
        var items = await filteredMessages
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new MessageResponse
            {
                Id = m.Id,
                ConversationId = m.ConversationId,
                SenderUserId = m.SenderUserId,
                MessageType = m.MessageType,
                Content = m.DeletedAt.HasValue ? "[deleted]" : m.Content,
                SentAt = m.SentAt,
                EditedAt = m.EditedAt,
                IsDeleted = m.DeletedAt.HasValue
            })
            .ToListAsync(cancellationToken);

        return PagedResult<MessageResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<MessageResponse> SendMessageAsync(Guid conversationId, Guid userId, SendMessageRequest request, CancellationToken cancellationToken = default)
    {
        var conversations = await _conversationRepository.QueryAsync(true, cancellationToken);
        var conversation = await conversations
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == conversationId, cancellationToken);

        if (conversation is null) throw new NotFoundException("Conversation", conversationId);

        if (!conversation.Participants.Any(p => p.UserId == userId && p.IsActive))
            throw new ForbiddenException("You do not have access to this conversation.");

        var message = new Message
        {
            ConversationId = conversationId,
            SenderUserId = userId,
            MessageType = request.MessageType,
            Content = request.Content,
            SentAt = DateTime.UtcNow
        };

        await _messageRepository.AddAsync(message, cancellationToken);

        conversation.LastMessageAt = message.SentAt;
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);

        return new MessageResponse
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            SenderUserId = message.SenderUserId,
            MessageType = message.MessageType,
            Content = message.Content,
            SentAt = message.SentAt,
            EditedAt = message.EditedAt,
            IsDeleted = false
        };
    }

    public async Task<MessageResponse> UpdateMessageAsync(Guid messageId, Guid userId, UpdateMessageRequest request, CancellationToken cancellationToken = default)
    {
        var message = await _messageRepository.GetByIdAsync(messageId, cancellationToken);
        if (message is null) throw new NotFoundException("Message", messageId);

        if (message.SenderUserId != userId)
            throw new ForbiddenException("You can only edit your own messages.");

        if (message.DeletedAt.HasValue)
            throw new ConflictException("Cannot edit a deleted message.");

        message.Content = request.Content;
        message.EditedAt = DateTime.UtcNow;
        await _messageRepository.UpdateAsync(message, cancellationToken);

        return new MessageResponse
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            SenderUserId = message.SenderUserId,
            MessageType = message.MessageType,
            Content = message.Content,
            SentAt = message.SentAt,
            EditedAt = message.EditedAt,
            IsDeleted = false
        };
    }

    public async Task DeleteMessageAsync(Guid messageId, Guid userId, CancellationToken cancellationToken = default)
    {
        var message = await _messageRepository.GetByIdAsync(messageId, cancellationToken);
        if (message is null) throw new NotFoundException("Message", messageId);

        if (message.SenderUserId != userId)
            throw new ForbiddenException("You can only delete your own messages.");

        message.DeletedAt = DateTime.UtcNow;
        await _messageRepository.UpdateAsync(message, cancellationToken);
    }
}
