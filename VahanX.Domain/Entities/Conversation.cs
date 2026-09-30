using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Conversation between users.
/// </summary>
public class Conversation : BaseEntity
{
    public ConversationType ConversationType { get; set; } = ConversationType.Enquiry;

    public Guid? EnquiryId { get; set; }

    public Enquiry? Enquiry { get; set; }

    public Guid? ListingId { get; set; }

    public VehicleListing? Listing { get; set; }

    public DateTime? LastMessageAt { get; set; }

    public ConversationStatus Status { get; set; } = ConversationStatus.Active;

    public ICollection<ConversationParticipant> Participants { get; set; } = new List<ConversationParticipant>();

    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
