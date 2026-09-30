using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Cms;

/// <summary>
/// Response DTO for a CMS page.
/// </summary>
public class PageResponse
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string Content { get; set; } = string.Empty;
    public ContentStatus Status { get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Request DTO for creating a CMS page.
/// </summary>
public class CreatePageRequest
{
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
}

/// <summary>
/// Request DTO for updating a CMS page.
/// </summary>
public class UpdatePageRequest
{
    public string? Title { get; set; }
    public string? Summary { get; set; }
    public string? Content { get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
}

/// <summary>
/// Response DTO for a CMS article.
/// </summary>
public class ArticleResponse
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid AuthorUserId { get; set; }
    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public ContentStatus Status { get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Request DTO for creating a CMS article.
/// </summary>
public class CreateArticleRequest
{
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid? CategoryId { get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
}

/// <summary>
/// Request DTO for updating a CMS article.
/// </summary>
public class UpdateArticleRequest
{
    public string? Title { get; set; }
    public string? Summary { get; set; }
    public string? Content { get; set; }
    public Guid? CategoryId { get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
}

/// <summary>
/// Response DTO for a CMS category.
/// </summary>
public class ContentCategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
}

/// <summary>
/// Request DTO for creating a CMS category.
/// </summary>
public class CreateContentCategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
}

/// <summary>
/// Response DTO for a CMS tag.
/// </summary>
public class ContentTagResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

/// <summary>
/// Response DTO for a FAQ.
/// </summary>
public class FaqResponse
{
    public Guid Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public Guid? CategoryId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsPublished { get; set; }
}

/// <summary>
/// Request DTO for creating a FAQ.
/// </summary>
public class CreateFaqRequest
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public Guid? CategoryId { get; set; }
    public int DisplayOrder { get; set; }
}

/// <summary>
/// Request DTO for updating a FAQ.
/// </summary>
public class UpdateFaqRequest
{
    public string? Question { get; set; }
    public string? Answer { get; set; }
    public Guid? CategoryId { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsPublished { get; set; }
}

/// <summary>
/// Response DTO for a banner.
/// </summary>
public class BannerResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string? MediaReference { get; set; }
    public string? MobileMediaReference { get; set; }
    public string? DesktopMediaReference { get; set; }
    public string Placement { get; set; } = string.Empty;
    public TargetType TargetType { get; set; }
    public string? TargetReference { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public int DisplayOrder { get; set; }
    public BannerStatus Status { get; set; }
}

/// <summary>
/// Request DTO for creating a banner.
/// </summary>
public class CreateBannerRequest
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
}

/// <summary>
/// Request DTO for updating a banner.
/// </summary>
public class UpdateBannerRequest
{
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public string? MediaReference { get; set; }
    public string? MobileMediaReference { get; set; }
    public string? DesktopMediaReference { get; set; }
    public string? Placement { get; set; }
    public TargetType? TargetType { get; set; }
    public string? TargetReference { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public int? DisplayOrder { get; set; }
}

/// <summary>
/// Response DTO for a promotion.
/// </summary>
public class PromotionResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? MediaReference { get; set; }
    public TargetType TargetType { get; set; }
    public string? TargetReference { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public BannerStatus Status { get; set; }
    public int DisplayOrder { get; set; }
}

/// <summary>
/// Request DTO for creating a promotion.
/// </summary>
public class CreatePromotionRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? MediaReference { get; set; }
    public TargetType TargetType { get; set; } = TargetType.None;
    public string? TargetReference { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public int DisplayOrder { get; set; }
}

/// <summary>
/// Request DTO for updating a promotion.
/// </summary>
public class UpdatePromotionRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? MediaReference { get; set; }
    public TargetType? TargetType { get; set; }
    public string? TargetReference { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public int? DisplayOrder { get; set; }
}

/// <summary>
/// Response DTO for an announcement.
/// </summary>
public class AnnouncementResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public AnnouncementPriority Priority { get; set; }
    public ContentStatus Status { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Request DTO for creating an announcement.
/// </summary>
public class CreateAnnouncementRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public AnnouncementPriority Priority { get; set; } = AnnouncementPriority.Normal;
    public DateTime StartAt { get; set; }
    public DateTime? EndAt { get; set; }
}

/// <summary>
/// Request DTO for updating an announcement.
/// </summary>
public class UpdateAnnouncementRequest
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public AnnouncementPriority? Priority { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
}
