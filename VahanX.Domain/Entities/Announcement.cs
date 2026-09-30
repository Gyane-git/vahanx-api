using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// CMS announcement entity for platform-wide messages.
/// </summary>
public class Announcement : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public AnnouncementPriority Priority { get; set; } = AnnouncementPriority.Normal;
    public ContentStatus Status { get; set; } = ContentStatus.Draft;
    public DateTime StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public Guid CreatedByUserId { get; set; }
}
