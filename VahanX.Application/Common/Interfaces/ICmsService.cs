using VahanX.Application.Common;
using VahanX.Application.DTOs.Cms;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for CMS operations.
/// </summary>
public interface ICmsService
{
    // Pages
    Task<PageResponse> CreatePageAsync(CreatePageRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<PagedResult<PageResponse>> GetPagesAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PageResponse?> GetPageByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PageResponse> UpdatePageAsync(Guid id, UpdatePageRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<PageResponse> SubmitPageAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PageResponse> PublishPageAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<PageResponse> UnpublishPageAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PageResponse> ArchivePageAsync(Guid id, CancellationToken cancellationToken = default);

    // Articles
    Task<ArticleResponse> CreateArticleAsync(CreateArticleRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<PagedResult<ArticleResponse>> GetArticlesAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<ArticleResponse?> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ArticleResponse> UpdateArticleAsync(Guid id, UpdateArticleRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<ArticleResponse> SubmitArticleAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ArticleResponse> PublishArticleAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<ArticleResponse> UnpublishArticleAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ArticleResponse> ArchiveArticleAsync(Guid id, CancellationToken cancellationToken = default);

    // Categories
    Task<ContentCategoryResponse> CreateCategoryAsync(CreateContentCategoryRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ContentCategoryResponse>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    // FAQs
    Task<FaqResponse> CreateFaqAsync(CreateFaqRequest request, CancellationToken cancellationToken = default);
    Task<PagedResult<FaqResponse>> GetFaqsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<FaqResponse> UpdateFaqAsync(Guid id, UpdateFaqRequest request, CancellationToken cancellationToken = default);
    Task DeleteFaqAsync(Guid id, CancellationToken cancellationToken = default);

    // Banners
    Task<BannerResponse> CreateBannerAsync(CreateBannerRequest request, CancellationToken cancellationToken = default);
    Task<PagedResult<BannerResponse>> GetBannersAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<BannerResponse> UpdateBannerAsync(Guid id, UpdateBannerRequest request, CancellationToken cancellationToken = default);
    Task<BannerResponse> ActivateBannerAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BannerResponse> DeactivateBannerAsync(Guid id, CancellationToken cancellationToken = default);

    // Promotions
    Task<PromotionResponse> CreatePromotionAsync(CreatePromotionRequest request, CancellationToken cancellationToken = default);
    Task<PagedResult<PromotionResponse>> GetPromotionsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PromotionResponse> UpdatePromotionAsync(Guid id, UpdatePromotionRequest request, CancellationToken cancellationToken = default);

    // Announcements
    Task<AnnouncementResponse> CreateAnnouncementAsync(CreateAnnouncementRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<PagedResult<AnnouncementResponse>> GetAnnouncementsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<AnnouncementResponse> UpdateAnnouncementAsync(Guid id, UpdateAnnouncementRequest request, CancellationToken cancellationToken = default);
    Task<AnnouncementResponse> PublishAnnouncementAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AnnouncementResponse> UnpublishAnnouncementAsync(Guid id, CancellationToken cancellationToken = default);

    // Public APIs
    Task<PageResponse?> GetPublicPageBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ArticleResponse>> GetPublicArticlesAsync(CancellationToken cancellationToken = default);
    Task<ArticleResponse?> GetPublicArticleBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ContentCategoryResponse>> GetPublicCategoriesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FaqResponse>> GetPublicFaqsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BannerResponse>> GetPublicBannersAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PromotionResponse>> GetPublicPromotionsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AnnouncementResponse>> GetPublicAnnouncementsAsync(CancellationToken cancellationToken = default);
}
