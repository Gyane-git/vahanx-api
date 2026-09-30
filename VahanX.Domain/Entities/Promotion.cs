using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// CMS promotion entity for VahanX-managed promotional content.
/// Distinct from paid Advertisement campaigns.
/// </summary>
public class Promotion : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? MediaReference { get; set; }
    public TargetType TargetType { get; set; } = TargetType.None;
    public string? TargetReference { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public BannerStatus Status { get; set; } = BannerStatus.Inactive;
    public int DisplayOrder { get; set; }
}
