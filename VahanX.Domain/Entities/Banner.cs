using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// CMS banner entity for promotional display.
/// </summary>
public class Banner : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string? MediaReference { get; set; }
    public string? MobileMediaReference { get; set; }
    public string? DesktopMediaReference { get; set; }
    public string Placement { get; set; } = string.Empty;
    public TargetType TargetType { get; set; } = TargetType.None;
    public string? TargetReference { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public int DisplayOrder { get; set; }
    public BannerStatus Status { get; set; } = BannerStatus.Inactive;
}
