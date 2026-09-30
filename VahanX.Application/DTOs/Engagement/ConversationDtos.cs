using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Engagement;

/// <summary>
/// Conversation response DTO.
/// </summary>
public class ConversationResponse
{
    public Guid Id { get; set; }
    public ConversationType ConversationType { get; set; }
    public Guid? EnquiryId { get; set; }
    public Guid? ListingId { get; set; }
    public string? ListingTitle { get; set; }
    public DateTime? LastMessageAt { get; set; }
    public ConversationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Create conversation request.
/// </summary>
public class CreateConversationRequest
{
    public ConversationType ConversationType { get; set; } = ConversationType.Enquiry;
    public Guid? EnquiryId { get; set; }
    public Guid? ListingId { get; set; }
    public List<Guid> ParticipantUserIds { get; set; } = [];
}

/// <summary>
/// Message response DTO.
/// </summary>
public class MessageResponse
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public Guid SenderUserId { get; set; }
    public MessageType MessageType { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public bool IsDeleted { get; set; }
}

/// <summary>
/// Send message request.
/// </summary>
public class SendMessageRequest
{
    public string Content { get; set; } = string.Empty;
    public MessageType MessageType { get; set; } = MessageType.Text;
}

/// <summary>
/// Update message request.
/// </summary>
public class UpdateMessageRequest
{
    public string Content { get; set; } = string.Empty;
}
