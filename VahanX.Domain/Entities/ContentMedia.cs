using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Media reference for CMS content.
/// </summary>
public class ContentMedia : BaseEntity
{
    public Guid ContentId { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public string MediaReference { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int DisplayOrder { get; set; }
}
