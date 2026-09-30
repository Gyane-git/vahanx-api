using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Message within a conversation.
/// </summary>
public class Message : BaseEntity
{
    public Guid ConversationId { get; set; }

    public Conversation? Conversation { get; set; }

    public Guid SenderUserId { get; set; }

    public MessageType MessageType { get; set; } = MessageType.Text;

    public string Content { get; set; } = string.Empty;

    public DateTime SentAt { get; set; }

    public DateTime? EditedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public ICollection<MessageAttachment> Attachments { get; set; } = new List<MessageAttachment>();
}
