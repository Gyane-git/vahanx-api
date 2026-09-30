using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Participant in a conversation.
/// </summary>
public class ConversationParticipant : BaseEntity
{
    public Guid ConversationId { get; set; }

    public Conversation? Conversation { get; set; }

    public Guid UserId { get; set; }

    public DateTime JoinedAt { get; set; }

    public DateTime? LeftAt { get; set; }

    public bool IsActive { get; set; } = true;
}
