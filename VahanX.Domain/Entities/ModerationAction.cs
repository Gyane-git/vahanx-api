using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Record of a moderation action performed within a case.
/// </summary>
public class ModerationAction : BaseEntity
{
    public Guid ModerationCaseId { get; set; }
    public ModerationActionType ActionType { get; set; }
    public TargetType TargetType { get; set; }
    public Guid TargetId { get; set; }
    public Guid PerformedByUserId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
}
