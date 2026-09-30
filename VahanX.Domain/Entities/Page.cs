using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// CMS page entity for public content.
/// </summary>
public class Page : BaseEntity
{
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string Content { get; set; } = string.Empty;
    public ContentStatus Status { get; set; } = ContentStatus.Draft;
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
    public DateTime? PublishedAt { get; set; }
    public Guid CreatedByUserId { get; set; }
    public Guid? UpdatedByUserId { get; set; }
}
