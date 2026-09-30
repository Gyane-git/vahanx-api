using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Tag for CMS content labeling.
/// </summary>
public class ContentTag : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
