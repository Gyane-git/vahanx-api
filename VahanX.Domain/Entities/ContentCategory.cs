using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Category for CMS content organization.
/// </summary>
public class ContentCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
}
