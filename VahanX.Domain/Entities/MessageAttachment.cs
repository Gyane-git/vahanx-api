using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Attachment for a message.
/// </summary>
public class MessageAttachment : BaseEntity
{
    public Guid MessageId { get; set; }

    public Message? Message { get; set; }

    public string MediaReference { get; set; } = string.Empty;

    public string? FileName { get; set; }

    public string? ContentType { get; set; }

    public long? FileSize { get; set; }
}
