using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Append-only history of moderation case status changes.
/// </summary>
public class ModerationHistory : BaseEntity
{
    public Guid ModerationCaseId { get; set; }
    public ModerationCaseStatus OldStatus { get; set; }
    public ModerationCaseStatus NewStatus { get; set; }
    public ModerationActionType Action { get; set; }
    public Guid ChangedByUserId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }
}
