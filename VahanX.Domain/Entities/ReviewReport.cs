using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Report against a review for moderation.
/// </summary>
public class ReviewReport : BaseEntity
{
    public Guid ReviewId { get; set; }

    public Guid ReportedBy { get; set; }

    public string Reason { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
}
