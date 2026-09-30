using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Database-driven report reason for moderation.
/// </summary>
public class ReportReason : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TargetType TargetType { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
}
