using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Cms;
using VahanX.Application.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;
using VahanX.Infrastructure.Persistence;
using VahanX.Infrastructure.Persistence.Repositories;
using Xunit;

namespace VahanX.Tests.Unit.Cms;

/// <summary>
/// Unit tests for CmsService.
/// </summary>
public class CmsServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly CmsService _service;

    public CmsServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var pages = new Repository<Page>(_context);
        var articles = new Repository<Article>(_context);
        var categories = new Repository<ContentCategory>(_context);
        var faqs = new Repository<FAQ>(_context);
        var banners = new Repository<Banner>(_context);
        var promotions = new Repository<Promotion>(_context);
        var announcements = new Repository<Announcement>(_context);

        _service = new CmsService(pages, articles, categories, faqs, banners, promotions, announcements);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreatePageAsync_WithValidRequest_CreatesPageInDraftStatus()
    {
        var request = new CreatePageRequest
        {
            Slug = "test-page",
            Title = "Test Page",
            Content = "<p>Test content</p>"
        };

        var result = await _service.CreatePageAsync(request, Guid.NewGuid());

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(ContentStatus.Draft, result.Status);
        Assert.Equal("test-page", result.Slug);
    }

    [Fact]
    public async Task CreatePageAsync_WithDuplicateSlug_ThrowsDuplicateSlugException()
    {
        var page = new Page
        {
            Id = Guid.NewGuid(),
            Slug = "test-page",
            Title = "Test Page",
            Content = "<p>Test content</p>",
            Status = ContentStatus.Draft
        };
        await _context.Pages.AddAsync(page);
        await _context.SaveChangesAsync();

        var request = new CreatePageRequest
        {
            Slug = "test-page",
            Title = "Test Page",
            Content = "<p>Test content</p>"
        };

        await Assert.ThrowsAsync<DuplicateSlugException>(() => _service.CreatePageAsync(request, Guid.NewGuid()));
    }

    [Fact]
    public async Task PublishPageAsync_WithPendingReviewStatus_ChangesToPublished()
    {
        var page = new Page
        {
            Id = Guid.NewGuid(),
            Slug = "test-page",
            Title = "Test Page",
            Content = "<p>Test content</p>",
            Status = ContentStatus.PendingReview
        };
        await _context.Pages.AddAsync(page);
        await _context.SaveChangesAsync();

        var result = await _service.PublishPageAsync(page.Id, Guid.NewGuid());

        Assert.Equal(ContentStatus.Published, result.Status);
        Assert.NotNull(result.PublishedAt);
    }

    [Fact]
    public async Task CreateArticleAsync_WithValidRequest_CreatesArticleInDraftStatus()
    {
        var request = new CreateArticleRequest
        {
            Slug = "test-article",
            Title = "Test Article",
            Content = "<p>Test content</p>"
        };

        var result = await _service.CreateArticleAsync(request, Guid.NewGuid());

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(ContentStatus.Draft, result.Status);
    }

    [Fact]
    public async Task CreateCategoryAsync_WithValidRequest_CreatesActiveCategory()
    {
        var request = new CreateContentCategoryRequest
        {
            Name = "Test Category",
            Slug = "test-category"
        };

        var result = await _service.CreateCategoryAsync(request);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task CreateFaqAsync_WithValidRequest_CreatesUnpublishedFaq()
    {
        var request = new CreateFaqRequest
        {
            Question = "Test question?",
            Answer = "Test answer."
        };

        var result = await _service.CreateFaqAsync(request);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.False(result.IsPublished);
    }

    [Fact]
    public async Task CreateBannerAsync_WithValidRequest_CreatesInactiveBanner()
    {
        var request = new CreateBannerRequest
        {
            Title = "Test Banner",
            Placement = "Homepage"
        };

        var result = await _service.CreateBannerAsync(request);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(BannerStatus.Inactive, result.Status);
    }

    [Fact]
    public async Task ActivateBannerAsync_WithInactiveStatus_ChangesToActive()
    {
        var banner = new Banner
        {
            Id = Guid.NewGuid(),
            Title = "Test Banner",
            Placement = "Homepage",
            Status = BannerStatus.Inactive
        };
        await _context.Banners.AddAsync(banner);
        await _context.SaveChangesAsync();

        var result = await _service.ActivateBannerAsync(banner.Id);

        Assert.Equal(BannerStatus.Active, result.Status);
    }

    [Fact]
    public async Task CreateAnnouncementAsync_WithValidRequest_CreatesDraftAnnouncement()
    {
        var request = new CreateAnnouncementRequest
        {
            Title = "Test Announcement",
            Content = "Test content",
            StartAt = DateTime.UtcNow
        };

        var result = await _service.CreateAnnouncementAsync(request, Guid.NewGuid());

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(ContentStatus.Draft, result.Status);
    }

    [Fact]
    public async Task GetPublicPageBySlugAsync_WithPublishedPage_ReturnsPage()
    {
        var page = new Page
        {
            Id = Guid.NewGuid(),
            Slug = "test-page",
            Title = "Test Page",
            Content = "<p>Test content</p>",
            Status = ContentStatus.Published,
            PublishedAt = DateTime.UtcNow
        };
        await _context.Pages.AddAsync(page);
        await _context.SaveChangesAsync();

        var result = await _service.GetPublicPageBySlugAsync("test-page");

        Assert.NotNull(result);
        Assert.Equal("test-page", result.Slug);
    }

    [Fact]
    public async Task GetPublicPageBySlugAsync_WithDraftPage_ReturnsNull()
    {
        var page = new Page
        {
            Id = Guid.NewGuid(),
            Slug = "test-page",
            Title = "Test Page",
            Content = "<p>Test content</p>",
            Status = ContentStatus.Draft
        };
        await _context.Pages.AddAsync(page);
        await _context.SaveChangesAsync();

        var result = await _service.GetPublicPageBySlugAsync("test-page");

        Assert.Null(result);
    }
}
