using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Cms;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of CMS service.
/// </summary>
public class CmsService : ICmsService
{
    private readonly IRepository<Page> _pages;
    private readonly IRepository<Article> _articles;
    private readonly IRepository<ContentCategory> _categories;
    private readonly IRepository<FAQ> _faqs;
    private readonly IRepository<Banner> _banners;
    private readonly IRepository<Promotion> _promotions;
    private readonly IRepository<Announcement> _announcements;

    public CmsService(
        IRepository<Page> pages,
        IRepository<Article> articles,
        IRepository<ContentCategory> categories,
        IRepository<FAQ> faqs,
        IRepository<Banner> banners,
        IRepository<Promotion> promotions,
        IRepository<Announcement> announcements)
    {
        _pages = pages;
        _articles = articles;
        _categories = categories;
        _faqs = faqs;
        _banners = banners;
        _promotions = promotions;
        _announcements = announcements;
    }

    public async Task<PageResponse> CreatePageAsync(CreatePageRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var slugExists = await _pages.AnyAsync(p => p.Slug == request.Slug, cancellationToken);
        if (slugExists) throw new DuplicateSlugException(request.Slug, "Page");

        var page = new Page
        {
            Slug = request.Slug,
            Title = request.Title,
            Summary = request.Summary,
            Content = SanitizeHtml(request.Content),
            Status = ContentStatus.Draft,
            SeoTitle = request.SeoTitle,
            SeoDescription = request.SeoDescription,
            CreatedByUserId = userId
        };

        await _pages.AddAsync(page, cancellationToken);
        return MapToResponse(page);
    }

    public async Task<PagedResult<PageResponse>> GetPagesAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _pages.QueryAsync(true, cancellationToken);
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PageResponse
            {
                Id = p.Id,
                Slug = p.Slug,
                Title = p.Title,
                Summary = p.Summary,
                Content = p.Content,
                Status = p.Status,
                SeoTitle = p.SeoTitle,
                SeoDescription = p.SeoDescription,
                PublishedAt = p.PublishedAt,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<PageResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PageResponse?> GetPageByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var page = await _pages.GetByIdAsync(id, cancellationToken);
        return page == null ? null : MapToResponse(page);
    }

    public async Task<PageResponse> UpdatePageAsync(Guid id, UpdatePageRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var page = await _pages.GetByIdAsync(id, cancellationToken);
        if (page is null) throw new CmsPageNotFoundException(id.ToString());

        if (page.Status == ContentStatus.Archived)
            throw new ConflictException("Cannot update an archived page.");

        if (request.Title != null) page.Title = request.Title;
        if (request.Summary != null) page.Summary = request.Summary;
        if (request.Content != null) page.Content = SanitizeHtml(request.Content);
        if (request.SeoTitle != null) page.SeoTitle = request.SeoTitle;
        if (request.SeoDescription != null) page.SeoDescription = request.SeoDescription;
        page.UpdatedByUserId = userId;

        await _pages.UpdateAsync(page, cancellationToken);
        return MapToResponse(page);
    }

    public async Task<PageResponse> SubmitPageAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var page = await _pages.GetByIdAsync(id, cancellationToken);
        if (page is null) throw new CmsPageNotFoundException(id.ToString());

        if (page.Status != ContentStatus.Draft)
            throw new ConflictException($"Cannot submit a page with status {page.Status}.");

        page.Status = ContentStatus.PendingReview;
        await _pages.UpdateAsync(page, cancellationToken);
        return MapToResponse(page);
    }

    public async Task<PageResponse> PublishPageAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var page = await _pages.GetByIdAsync(id, cancellationToken);
        if (page is null) throw new CmsPageNotFoundException(id.ToString());

        if (page.Status != ContentStatus.PendingReview && page.Status != ContentStatus.Draft)
            throw new ConflictException($"Cannot publish a page with status {page.Status}.");

        page.Status = ContentStatus.Published;
        page.PublishedAt = DateTime.UtcNow;
        page.UpdatedByUserId = userId;
        await _pages.UpdateAsync(page, cancellationToken);
        return MapToResponse(page);
    }

    public async Task<PageResponse> UnpublishPageAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var page = await _pages.GetByIdAsync(id, cancellationToken);
        if (page is null) throw new CmsPageNotFoundException(id.ToString());

        if (page.Status != ContentStatus.Published)
            throw new ConflictException($"Cannot unpublish a page with status {page.Status}.");

        page.Status = ContentStatus.Unpublished;
        await _pages.UpdateAsync(page, cancellationToken);
        return MapToResponse(page);
    }

    public async Task<PageResponse> ArchivePageAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var page = await _pages.GetByIdAsync(id, cancellationToken);
        if (page is null) throw new CmsPageNotFoundException(id.ToString());

        page.Status = ContentStatus.Archived;
        await _pages.UpdateAsync(page, cancellationToken);
        return MapToResponse(page);
    }

    public async Task<ArticleResponse> CreateArticleAsync(CreateArticleRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var slugExists = await _articles.AnyAsync(a => a.Slug == request.Slug, cancellationToken);
        if (slugExists) throw new DuplicateSlugException(request.Slug, "Article");

        var article = new Article
        {
            Slug = request.Slug,
            Title = request.Title,
            Summary = request.Summary,
            Content = SanitizeHtml(request.Content),
            AuthorUserId = userId,
            CategoryId = request.CategoryId,
            Status = ContentStatus.Draft,
            SeoTitle = request.SeoTitle,
            SeoDescription = request.SeoDescription
        };

        await _articles.AddAsync(article, cancellationToken);
        return MapToResponse(article);
    }

    public async Task<PagedResult<ArticleResponse>> GetArticlesAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _articles.QueryAsync(true, cancellationToken);
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new ArticleResponse
            {
                Id = a.Id,
                Slug = a.Slug,
                Title = a.Title,
                Summary = a.Summary,
                Content = a.Content,
                AuthorUserId = a.AuthorUserId,
                CategoryId = a.CategoryId,
                Status = a.Status,
                SeoTitle = a.SeoTitle,
                SeoDescription = a.SeoDescription,
                PublishedAt = a.PublishedAt,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<ArticleResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<ArticleResponse?> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var article = await _articles.GetByIdAsync(id, cancellationToken);
        return article == null ? null : MapToResponse(article);
    }

    public async Task<ArticleResponse> UpdateArticleAsync(Guid id, UpdateArticleRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var article = await _articles.GetByIdAsync(id, cancellationToken);
        if (article is null) throw new NotFoundException("Article", id);

        if (article.Status == ContentStatus.Archived)
            throw new ConflictException("Cannot update an archived article.");

        if (request.Title != null) article.Title = request.Title;
        if (request.Summary != null) article.Summary = request.Summary;
        if (request.Content != null) article.Content = SanitizeHtml(request.Content);
        if (request.CategoryId != null) article.CategoryId = request.CategoryId;
        if (request.SeoTitle != null) article.SeoTitle = request.SeoTitle;
        if (request.SeoDescription != null) article.SeoDescription = request.SeoDescription;

        await _articles.UpdateAsync(article, cancellationToken);
        return MapToResponse(article);
    }

    public async Task<ArticleResponse> SubmitArticleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var article = await _articles.GetByIdAsync(id, cancellationToken);
        if (article is null) throw new NotFoundException("Article", id);

        if (article.Status != ContentStatus.Draft)
            throw new ConflictException($"Cannot submit an article with status {article.Status}.");

        article.Status = ContentStatus.PendingReview;
        await _articles.UpdateAsync(article, cancellationToken);
        return MapToResponse(article);
    }

    public async Task<ArticleResponse> PublishArticleAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var article = await _articles.GetByIdAsync(id, cancellationToken);
        if (article is null) throw new NotFoundException("Article", id);

        if (article.Status != ContentStatus.PendingReview && article.Status != ContentStatus.Draft)
            throw new ConflictException($"Cannot publish an article with status {article.Status}.");

        article.Status = ContentStatus.Published;
        article.PublishedAt = DateTime.UtcNow;
        await _articles.UpdateAsync(article, cancellationToken);
        return MapToResponse(article);
    }

    public async Task<ArticleResponse> UnpublishArticleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var article = await _articles.GetByIdAsync(id, cancellationToken);
        if (article is null) throw new NotFoundException("Article", id);

        if (article.Status != ContentStatus.Published)
            throw new ConflictException($"Cannot unpublish an article with status {article.Status}.");

        article.Status = ContentStatus.Unpublished;
        await _articles.UpdateAsync(article, cancellationToken);
        return MapToResponse(article);
    }

    public async Task<ArticleResponse> ArchiveArticleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var article = await _articles.GetByIdAsync(id, cancellationToken);
        if (article is null) throw new NotFoundException("Article", id);

        article.Status = ContentStatus.Archived;
        await _articles.UpdateAsync(article, cancellationToken);
        return MapToResponse(article);
    }

    public async Task<ContentCategoryResponse> CreateCategoryAsync(CreateContentCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var slugExists = await _categories.AnyAsync(cc => cc.Slug == request.Slug, cancellationToken);
        if (slugExists) throw new DuplicateSlugException(request.Slug, "ContentCategory");

        var category = new ContentCategory
        {
            Name = request.Name,
            Slug = request.Slug,
            Description = request.Description,
            DisplayOrder = request.DisplayOrder
        };

        await _categories.AddAsync(category, cancellationToken);

        return new ContentCategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description,
            IsActive = category.IsActive,
            DisplayOrder = category.DisplayOrder
        };
    }

    public async Task<IReadOnlyList<ContentCategoryResponse>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var query = await _categories.QueryAsync(true, cancellationToken);
        return await query
            .Where(cc => cc.IsActive)
            .OrderBy(cc => cc.DisplayOrder)
            .Select(cc => new ContentCategoryResponse
            {
                Id = cc.Id,
                Name = cc.Name,
                Slug = cc.Slug,
                Description = cc.Description,
                IsActive = cc.IsActive,
                DisplayOrder = cc.DisplayOrder
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<FaqResponse> CreateFaqAsync(CreateFaqRequest request, CancellationToken cancellationToken = default)
    {
        var faq = new FAQ
        {
            Question = request.Question,
            Answer = SanitizeHtml(request.Answer),
            CategoryId = request.CategoryId,
            DisplayOrder = request.DisplayOrder,
            IsPublished = false
        };

        await _faqs.AddAsync(faq, cancellationToken);

        return new FaqResponse
        {
            Id = faq.Id,
            Question = faq.Question,
            Answer = faq.Answer,
            CategoryId = faq.CategoryId,
            DisplayOrder = faq.DisplayOrder,
            IsPublished = faq.IsPublished
        };
    }

    public async Task<PagedResult<FaqResponse>> GetFaqsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _faqs.QueryAsync(true, cancellationToken);
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(f => f.DisplayOrder)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(f => new FaqResponse
            {
                Id = f.Id,
                Question = f.Question,
                Answer = f.Answer,
                CategoryId = f.CategoryId,
                DisplayOrder = f.DisplayOrder,
                IsPublished = f.IsPublished
            })
            .ToListAsync(cancellationToken);

        return PagedResult<FaqResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<FaqResponse> UpdateFaqAsync(Guid id, UpdateFaqRequest request, CancellationToken cancellationToken = default)
    {
        var faq = await _faqs.GetByIdAsync(id, cancellationToken);
        if (faq is null) throw new NotFoundException("FAQ", id);

        if (request.Question != null) faq.Question = request.Question;
        if (request.Answer != null) faq.Answer = SanitizeHtml(request.Answer);
        if (request.CategoryId != null) faq.CategoryId = request.CategoryId;
        if (request.DisplayOrder != null) faq.DisplayOrder = request.DisplayOrder.Value;
        if (request.IsPublished != null) faq.IsPublished = request.IsPublished.Value;

        await _faqs.UpdateAsync(faq, cancellationToken);

        return new FaqResponse
        {
            Id = faq.Id,
            Question = faq.Question,
            Answer = faq.Answer,
            CategoryId = faq.CategoryId,
            DisplayOrder = faq.DisplayOrder,
            IsPublished = faq.IsPublished
        };
    }

    public async Task DeleteFaqAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var faq = await _faqs.GetByIdAsync(id, cancellationToken);
        if (faq is null) throw new NotFoundException("FAQ", id);

        await _faqs.DeleteAsync(faq, cancellationToken);
    }

    public async Task<BannerResponse> CreateBannerAsync(CreateBannerRequest request, CancellationToken cancellationToken = default)
    {
        var banner = new Banner
        {
            Title = request.Title,
            Subtitle = request.Subtitle,
            MediaReference = request.MediaReference,
            MobileMediaReference = request.MobileMediaReference,
            DesktopMediaReference = request.DesktopMediaReference,
            Placement = request.Placement,
            TargetType = request.TargetType,
            TargetReference = request.TargetReference,
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            DisplayOrder = request.DisplayOrder,
            Status = BannerStatus.Inactive
        };

        await _banners.AddAsync(banner, cancellationToken);
        return MapToResponse(banner);
    }

    public async Task<PagedResult<BannerResponse>> GetBannersAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _banners.QueryAsync(true, cancellationToken);
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(b => b.DisplayOrder)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BannerResponse
            {
                Id = b.Id,
                Title = b.Title,
                Subtitle = b.Subtitle,
                MediaReference = b.MediaReference,
                MobileMediaReference = b.MobileMediaReference,
                DesktopMediaReference = b.DesktopMediaReference,
                Placement = b.Placement,
                TargetType = b.TargetType,
                TargetReference = b.TargetReference,
                StartAt = b.StartAt,
                EndAt = b.EndAt,
                DisplayOrder = b.DisplayOrder,
                Status = b.Status
            })
            .ToListAsync(cancellationToken);

        return PagedResult<BannerResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<BannerResponse> UpdateBannerAsync(Guid id, UpdateBannerRequest request, CancellationToken cancellationToken = default)
    {
        var banner = await _banners.GetByIdAsync(id, cancellationToken);
        if (banner is null) throw new NotFoundException("Banner", id);

        if (request.Title != null) banner.Title = request.Title;
        if (request.Subtitle != null) banner.Subtitle = request.Subtitle;
        if (request.MediaReference != null) banner.MediaReference = request.MediaReference;
        if (request.MobileMediaReference != null) banner.MobileMediaReference = request.MobileMediaReference;
        if (request.DesktopMediaReference != null) banner.DesktopMediaReference = request.DesktopMediaReference;
        if (request.Placement != null) banner.Placement = request.Placement;
        if (request.TargetType != null) banner.TargetType = request.TargetType.Value;
        if (request.TargetReference != null) banner.TargetReference = request.TargetReference;
        if (request.StartAt != null) banner.StartAt = request.StartAt;
        if (request.EndAt != null) banner.EndAt = request.EndAt;
        if (request.DisplayOrder != null) banner.DisplayOrder = request.DisplayOrder.Value;

        await _banners.UpdateAsync(banner, cancellationToken);
        return MapToResponse(banner);
    }

    public async Task<BannerResponse> ActivateBannerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var banner = await _banners.GetByIdAsync(id, cancellationToken);
        if (banner is null) throw new NotFoundException("Banner", id);

        banner.Status = BannerStatus.Active;
        await _banners.UpdateAsync(banner, cancellationToken);
        return MapToResponse(banner);
    }

    public async Task<BannerResponse> DeactivateBannerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var banner = await _banners.GetByIdAsync(id, cancellationToken);
        if (banner is null) throw new NotFoundException("Banner", id);

        banner.Status = BannerStatus.Inactive;
        await _banners.UpdateAsync(banner, cancellationToken);
        return MapToResponse(banner);
    }

    public async Task<PromotionResponse> CreatePromotionAsync(CreatePromotionRequest request, CancellationToken cancellationToken = default)
    {
        var promotion = new Promotion
        {
            Title = request.Title,
            Description = request.Description,
            MediaReference = request.MediaReference,
            TargetType = request.TargetType,
            TargetReference = request.TargetReference,
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            DisplayOrder = request.DisplayOrder,
            Status = BannerStatus.Inactive
        };

        await _promotions.AddAsync(promotion, cancellationToken);
        return MapToResponse(promotion);
    }

    public async Task<PagedResult<PromotionResponse>> GetPromotionsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _promotions.QueryAsync(true, cancellationToken);
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(p => p.DisplayOrder)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PromotionResponse
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                MediaReference = p.MediaReference,
                TargetType = p.TargetType,
                TargetReference = p.TargetReference,
                StartAt = p.StartAt,
                EndAt = p.EndAt,
                Status = p.Status,
                DisplayOrder = p.DisplayOrder
            })
            .ToListAsync(cancellationToken);

        return PagedResult<PromotionResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PromotionResponse> UpdatePromotionAsync(Guid id, UpdatePromotionRequest request, CancellationToken cancellationToken = default)
    {
        var promotion = await _promotions.GetByIdAsync(id, cancellationToken);
        if (promotion is null) throw new NotFoundException("Promotion", id);

        if (request.Title != null) promotion.Title = request.Title;
        if (request.Description != null) promotion.Description = request.Description;
        if (request.MediaReference != null) promotion.MediaReference = request.MediaReference;
        if (request.TargetType != null) promotion.TargetType = request.TargetType.Value;
        if (request.TargetReference != null) promotion.TargetReference = request.TargetReference;
        if (request.StartAt != null) promotion.StartAt = request.StartAt;
        if (request.EndAt != null) promotion.EndAt = request.EndAt;
        if (request.DisplayOrder != null) promotion.DisplayOrder = request.DisplayOrder.Value;

        await _promotions.UpdateAsync(promotion, cancellationToken);
        return MapToResponse(promotion);
    }

    public async Task<AnnouncementResponse> CreateAnnouncementAsync(CreateAnnouncementRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var announcement = new Announcement
        {
            Title = request.Title,
            Content = SanitizeHtml(request.Content),
            Priority = request.Priority,
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            Status = ContentStatus.Draft,
            CreatedByUserId = userId
        };

        await _announcements.AddAsync(announcement, cancellationToken);
        return MapToResponse(announcement);
    }

    public async Task<PagedResult<AnnouncementResponse>> GetAnnouncementsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _announcements.QueryAsync(true, cancellationToken);
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new AnnouncementResponse
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                Priority = a.Priority,
                Status = a.Status,
                StartAt = a.StartAt,
                EndAt = a.EndAt,
                PublishedAt = a.PublishedAt,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<AnnouncementResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<AnnouncementResponse> UpdateAnnouncementAsync(Guid id, UpdateAnnouncementRequest request, CancellationToken cancellationToken = default)
    {
        var announcement = await _announcements.GetByIdAsync(id, cancellationToken);
        if (announcement is null) throw new NotFoundException("Announcement", id);

        if (request.Title != null) announcement.Title = request.Title;
        if (request.Content != null) announcement.Content = SanitizeHtml(request.Content);
        if (request.Priority != null) announcement.Priority = request.Priority.Value;
        if (request.StartAt != null) announcement.StartAt = request.StartAt.Value;
        if (request.EndAt != null) announcement.EndAt = request.EndAt;

        await _announcements.UpdateAsync(announcement, cancellationToken);
        return MapToResponse(announcement);
    }

    public async Task<AnnouncementResponse> PublishAnnouncementAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var announcement = await _announcements.GetByIdAsync(id, cancellationToken);
        if (announcement is null) throw new NotFoundException("Announcement", id);

        if (announcement.Status != ContentStatus.Draft && announcement.Status != ContentStatus.PendingReview)
            throw new ConflictException($"Cannot publish an announcement with status {announcement.Status}.");

        announcement.Status = ContentStatus.Published;
        announcement.PublishedAt = DateTime.UtcNow;
        await _announcements.UpdateAsync(announcement, cancellationToken);
        return MapToResponse(announcement);
    }

    public async Task<AnnouncementResponse> UnpublishAnnouncementAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var announcement = await _announcements.GetByIdAsync(id, cancellationToken);
        if (announcement is null) throw new NotFoundException("Announcement", id);

        if (announcement.Status != ContentStatus.Published)
            throw new ConflictException($"Cannot unpublish an announcement with status {announcement.Status}.");

        announcement.Status = ContentStatus.Unpublished;
        await _announcements.UpdateAsync(announcement, cancellationToken);
        return MapToResponse(announcement);
    }

    public async Task<PageResponse?> GetPublicPageBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var query = await _pages.QueryAsync(true, cancellationToken);
        var page = await query.FirstOrDefaultAsync(p => p.Slug == slug && p.Status == ContentStatus.Published, cancellationToken);
        return page == null ? null : MapToResponse(page);
    }

    public async Task<IReadOnlyList<ArticleResponse>> GetPublicArticlesAsync(CancellationToken cancellationToken = default)
    {
        var query = await _articles.QueryAsync(true, cancellationToken);
        return await query
            .Where(a => a.Status == ContentStatus.Published)
            .OrderByDescending(a => a.PublishedAt)
            .Select(a => new ArticleResponse
            {
                Id = a.Id,
                Slug = a.Slug,
                Title = a.Title,
                Summary = a.Summary,
                Content = a.Content,
                AuthorUserId = a.AuthorUserId,
                CategoryId = a.CategoryId,
                Status = a.Status,
                SeoTitle = a.SeoTitle,
                SeoDescription = a.SeoDescription,
                PublishedAt = a.PublishedAt,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<ArticleResponse?> GetPublicArticleBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var query = await _articles.QueryAsync(true, cancellationToken);
        var article = await query.FirstOrDefaultAsync(a => a.Slug == slug && a.Status == ContentStatus.Published, cancellationToken);
        return article == null ? null : MapToResponse(article);
    }

    public async Task<IReadOnlyList<ContentCategoryResponse>> GetPublicCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var query = await _categories.QueryAsync(true, cancellationToken);
        return await query
            .Where(cc => cc.IsActive)
            .OrderBy(cc => cc.DisplayOrder)
            .Select(cc => new ContentCategoryResponse
            {
                Id = cc.Id,
                Name = cc.Name,
                Slug = cc.Slug,
                Description = cc.Description,
                IsActive = cc.IsActive,
                DisplayOrder = cc.DisplayOrder
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<FaqResponse>> GetPublicFaqsAsync(CancellationToken cancellationToken = default)
    {
        var query = await _faqs.QueryAsync(true, cancellationToken);
        return await query
            .Where(f => f.IsPublished)
            .OrderBy(f => f.DisplayOrder)
            .Select(f => new FaqResponse
            {
                Id = f.Id,
                Question = f.Question,
                Answer = f.Answer,
                CategoryId = f.CategoryId,
                DisplayOrder = f.DisplayOrder,
                IsPublished = f.IsPublished
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<BannerResponse>> GetPublicBannersAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var query = await _banners.QueryAsync(true, cancellationToken);
        return await query
            .Where(b => b.Status == BannerStatus.Active &&
                        (b.StartAt == null || b.StartAt <= now) &&
                        (b.EndAt == null || b.EndAt >= now))
            .OrderBy(b => b.DisplayOrder)
            .Select(b => new BannerResponse
            {
                Id = b.Id,
                Title = b.Title,
                Subtitle = b.Subtitle,
                MediaReference = b.MediaReference,
                MobileMediaReference = b.MobileMediaReference,
                DesktopMediaReference = b.DesktopMediaReference,
                Placement = b.Placement,
                TargetType = b.TargetType,
                TargetReference = b.TargetReference,
                StartAt = b.StartAt,
                EndAt = b.EndAt,
                DisplayOrder = b.DisplayOrder,
                Status = b.Status
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PromotionResponse>> GetPublicPromotionsAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var query = await _promotions.QueryAsync(true, cancellationToken);
        return await query
            .Where(p => p.Status == BannerStatus.Active &&
                        (p.StartAt == null || p.StartAt <= now) &&
                        (p.EndAt == null || p.EndAt >= now))
            .OrderBy(p => p.DisplayOrder)
            .Select(p => new PromotionResponse
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                MediaReference = p.MediaReference,
                TargetType = p.TargetType,
                TargetReference = p.TargetReference,
                StartAt = p.StartAt,
                EndAt = p.EndAt,
                Status = p.Status,
                DisplayOrder = p.DisplayOrder
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AnnouncementResponse>> GetPublicAnnouncementsAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var query = await _announcements.QueryAsync(true, cancellationToken);
        return await query
            .Where(a => a.Status == ContentStatus.Published &&
                        a.StartAt <= now &&
                        (a.EndAt == null || a.EndAt >= now))
            .OrderByDescending(a => a.Priority)
            .ThenByDescending(a => a.PublishedAt)
            .Select(a => new AnnouncementResponse
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                Priority = a.Priority,
                Status = a.Status,
                StartAt = a.StartAt,
                EndAt = a.EndAt,
                PublishedAt = a.PublishedAt,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }

    private static string SanitizeHtml(string html)
    {
        if (string.IsNullOrWhiteSpace(html)) return string.Empty;

        var sanitized = html
            .Replace("<script", "<removed-script", StringComparison.OrdinalIgnoreCase)
            .Replace("</script>", "</removed-script>", StringComparison.OrdinalIgnoreCase)
            .Replace("javascript:", "removed:", StringComparison.OrdinalIgnoreCase)
            .Replace("onerror=", "removed-onerror=", StringComparison.OrdinalIgnoreCase)
            .Replace("onclick=", "removed-onclick=", StringComparison.OrdinalIgnoreCase)
            .Replace("onload=", "removed-onload=", StringComparison.OrdinalIgnoreCase);

        return sanitized;
    }

    private static PageResponse MapToResponse(Page page)
    {
        return new PageResponse
        {
            Id = page.Id,
            Slug = page.Slug,
            Title = page.Title,
            Summary = page.Summary,
            Content = page.Content,
            Status = page.Status,
            SeoTitle = page.SeoTitle,
            SeoDescription = page.SeoDescription,
            PublishedAt = page.PublishedAt,
            CreatedAt = page.CreatedAt,
            UpdatedAt = page.UpdatedAt
        };
    }

    private static ArticleResponse MapToResponse(Article article)
    {
        return new ArticleResponse
        {
            Id = article.Id,
            Slug = article.Slug,
            Title = article.Title,
            Summary = article.Summary,
            Content = article.Content,
            AuthorUserId = article.AuthorUserId,
            CategoryId = article.CategoryId,
            Status = article.Status,
            SeoTitle = article.SeoTitle,
            SeoDescription = article.SeoDescription,
            PublishedAt = article.PublishedAt,
            CreatedAt = article.CreatedAt,
            UpdatedAt = article.UpdatedAt
        };
    }

    private static BannerResponse MapToResponse(Banner banner)
    {
        return new BannerResponse
        {
            Id = banner.Id,
            Title = banner.Title,
            Subtitle = banner.Subtitle,
            MediaReference = banner.MediaReference,
            MobileMediaReference = banner.MobileMediaReference,
            DesktopMediaReference = banner.DesktopMediaReference,
            Placement = banner.Placement,
            TargetType = banner.TargetType,
            TargetReference = banner.TargetReference,
            StartAt = banner.StartAt,
            EndAt = banner.EndAt,
            DisplayOrder = banner.DisplayOrder,
            Status = banner.Status
        };
    }

    private static PromotionResponse MapToResponse(Promotion promotion)
    {
        return new PromotionResponse
        {
            Id = promotion.Id,
            Title = promotion.Title,
            Description = promotion.Description,
            MediaReference = promotion.MediaReference,
            TargetType = promotion.TargetType,
            TargetReference = promotion.TargetReference,
            StartAt = promotion.StartAt,
            EndAt = promotion.EndAt,
            Status = promotion.Status,
            DisplayOrder = promotion.DisplayOrder
        };
    }

    private static AnnouncementResponse MapToResponse(Announcement announcement)
    {
        return new AnnouncementResponse
        {
            Id = announcement.Id,
            Title = announcement.Title,
            Content = announcement.Content,
            Priority = announcement.Priority,
            Status = announcement.Status,
            StartAt = announcement.StartAt,
            EndAt = announcement.EndAt,
            PublishedAt = announcement.PublishedAt,
            CreatedAt = announcement.CreatedAt
        };
    }
}
