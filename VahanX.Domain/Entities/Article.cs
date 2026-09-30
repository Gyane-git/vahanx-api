using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// CMS article entity for public content.
/// </summary>
public class Article : BaseEntity
{
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid AuthorUserId { get; set; }
    public Guid? CategoryId { get; set; }
    public ContentStatus Status { get; set; } = ContentStatus.Draft;
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
    public DateTime? PublishedAt { get; set; }
}
