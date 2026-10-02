using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Cms;
using VahanX.Application.DTOs.Admin;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for CMS admin operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/admin/cms")]
[ApiVersion("1.0")]
public class CmsController : ControllerBase
{
    private readonly ICmsService _cmsService;
    private readonly ILogger<CmsController> _logger;

    public CmsController(ICmsService cmsService, ILogger<CmsController> logger)
    {
        _cmsService = cmsService;
        _logger = logger;
    }

    // Pages

    [HttpPost("pages")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<PageResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<PageResponse>>> CreatePage(
        [FromBody] CreatePageRequest request,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _cmsService.CreatePageAsync(request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetPageById), new { id = result.Id }, ApiResponse<PageResponse>.SuccessResponse(result, "Page created successfully."));
    }

    [HttpGet("pages")]
    [Authorize(Policy = AdminPermissions.CmsView)]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<PageResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<PageResponse>>>> GetPages(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _cmsService.GetPagesAsync(page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<PageResponse>>.SuccessResponse(result));
    }

    [HttpGet("pages/{id:guid}")]
    [Authorize(Policy = AdminPermissions.CmsView)]
    [ProducesResponseType(typeof(ApiResponse<PageResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PageResponse>>> GetPageById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _cmsService.GetPageByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Page not found."));
        return Ok(ApiResponse<PageResponse>.SuccessResponse(result));
    }

    [HttpPut("pages/{id:guid}")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<PageResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PageResponse>>> UpdatePage(
        Guid id,
        [FromBody] UpdatePageRequest request,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _cmsService.UpdatePageAsync(id, request, userId, cancellationToken);
        return Ok(ApiResponse<PageResponse>.SuccessResponse(result, "Page updated successfully."));
    }

    [HttpPost("pages/{id:guid}/submit")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<PageResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PageResponse>>> SubmitPage(Guid id, CancellationToken cancellationToken)
    {
        var result = await _cmsService.SubmitPageAsync(id, cancellationToken);
        return Ok(ApiResponse<PageResponse>.SuccessResponse(result, "Page submitted for review."));
    }

    [HttpPost("pages/{id:guid}/publish")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<PageResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PageResponse>>> PublishPage(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _cmsService.PublishPageAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<PageResponse>.SuccessResponse(result, "Page published successfully."));
    }

    [HttpPost("pages/{id:guid}/unpublish")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<PageResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PageResponse>>> UnpublishPage(Guid id, CancellationToken cancellationToken)
    {
        var result = await _cmsService.UnpublishPageAsync(id, cancellationToken);
        return Ok(ApiResponse<PageResponse>.SuccessResponse(result, "Page unpublished successfully."));
    }

    [HttpPost("pages/{id:guid}/archive")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<PageResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PageResponse>>> ArchivePage(Guid id, CancellationToken cancellationToken)
    {
        var result = await _cmsService.ArchivePageAsync(id, cancellationToken);
        return Ok(ApiResponse<PageResponse>.SuccessResponse(result, "Page archived successfully."));
    }

    // Articles

    [HttpPost("articles")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<ArticleResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<ArticleResponse>>> CreateArticle(
        [FromBody] CreateArticleRequest request,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _cmsService.CreateArticleAsync(request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetArticleById), new { id = result.Id }, ApiResponse<ArticleResponse>.SuccessResponse(result, "Article created successfully."));
    }

    [HttpGet("articles")]
    [Authorize(Policy = AdminPermissions.CmsView)]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ArticleResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ArticleResponse>>>> GetArticles(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _cmsService.GetArticlesAsync(page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ArticleResponse>>.SuccessResponse(result));
    }

    [HttpGet("articles/{id:guid}")]
    [Authorize(Policy = AdminPermissions.CmsView)]
    [ProducesResponseType(typeof(ApiResponse<ArticleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ArticleResponse>>> GetArticleById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _cmsService.GetArticleByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Article not found."));
        return Ok(ApiResponse<ArticleResponse>.SuccessResponse(result));
    }

    [HttpPut("articles/{id:guid}")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<ArticleResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ArticleResponse>>> UpdateArticle(
        Guid id,
        [FromBody] UpdateArticleRequest request,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _cmsService.UpdateArticleAsync(id, request, userId, cancellationToken);
        return Ok(ApiResponse<ArticleResponse>.SuccessResponse(result, "Article updated successfully."));
    }

    [HttpPost("articles/{id:guid}/submit")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<ArticleResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ArticleResponse>>> SubmitArticle(Guid id, CancellationToken cancellationToken)
    {
        var result = await _cmsService.SubmitArticleAsync(id, cancellationToken);
        return Ok(ApiResponse<ArticleResponse>.SuccessResponse(result, "Article submitted for review."));
    }

    [HttpPost("articles/{id:guid}/publish")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<ArticleResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ArticleResponse>>> PublishArticle(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _cmsService.PublishArticleAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<ArticleResponse>.SuccessResponse(result, "Article published successfully."));
    }

    [HttpPost("articles/{id:guid}/unpublish")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<ArticleResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ArticleResponse>>> UnpublishArticle(Guid id, CancellationToken cancellationToken)
    {
        var result = await _cmsService.UnpublishArticleAsync(id, cancellationToken);
        return Ok(ApiResponse<ArticleResponse>.SuccessResponse(result, "Article unpublished successfully."));
    }

    [HttpPost("articles/{id:guid}/archive")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<ArticleResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ArticleResponse>>> ArchiveArticle(Guid id, CancellationToken cancellationToken)
    {
        var result = await _cmsService.ArchiveArticleAsync(id, cancellationToken);
        return Ok(ApiResponse<ArticleResponse>.SuccessResponse(result, "Article archived successfully."));
    }

    // Categories

    [HttpPost("categories")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<ContentCategoryResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<ContentCategoryResponse>>> CreateCategory(
        [FromBody] CreateContentCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _cmsService.CreateCategoryAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetCategories), new { id = result.Id }, ApiResponse<ContentCategoryResponse>.SuccessResponse(result, "Category created successfully."));
    }

    [HttpGet("categories")]
    [Authorize(Policy = AdminPermissions.CmsView)]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ContentCategoryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ContentCategoryResponse>>>> GetCategories(CancellationToken cancellationToken)
    {
        var result = await _cmsService.GetCategoriesAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<ContentCategoryResponse>>.SuccessResponse(result));
    }

    // FAQs

    [HttpPost("faqs")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<FaqResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<FaqResponse>>> CreateFaq(
        [FromBody] CreateFaqRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _cmsService.CreateFaqAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetFaqs), new { id = result.Id }, ApiResponse<FaqResponse>.SuccessResponse(result, "FAQ created successfully."));
    }

    [HttpGet("faqs")]
    [Authorize(Policy = AdminPermissions.CmsView)]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FaqResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<FaqResponse>>>> GetFaqs(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _cmsService.GetFaqsAsync(page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<FaqResponse>>.SuccessResponse(result));
    }

    [HttpPut("faqs/{id:guid}")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<FaqResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<FaqResponse>>> UpdateFaq(
        Guid id,
        [FromBody] UpdateFaqRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _cmsService.UpdateFaqAsync(id, request, cancellationToken);
        return Ok(ApiResponse<FaqResponse>.SuccessResponse(result, "FAQ updated successfully."));
    }

    [HttpDelete("faqs/{id:guid}")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteFaq(Guid id, CancellationToken cancellationToken)
    {
        await _cmsService.DeleteFaqAsync(id, cancellationToken);
        return NoContent();
    }

    // Banners

    [HttpPost("banners")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<BannerResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<BannerResponse>>> CreateBanner(
        [FromBody] CreateBannerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _cmsService.CreateBannerAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetBanners), new { id = result.Id }, ApiResponse<BannerResponse>.SuccessResponse(result, "Banner created successfully."));
    }

    [HttpGet("banners")]
    [Authorize(Policy = AdminPermissions.CmsView)]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<BannerResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<BannerResponse>>>> GetBanners(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _cmsService.GetBannersAsync(page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<BannerResponse>>.SuccessResponse(result));
    }

    [HttpPut("banners/{id:guid}")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<BannerResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<BannerResponse>>> UpdateBanner(
        Guid id,
        [FromBody] UpdateBannerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _cmsService.UpdateBannerAsync(id, request, cancellationToken);
        return Ok(ApiResponse<BannerResponse>.SuccessResponse(result, "Banner updated successfully."));
    }

    [HttpPost("banners/{id:guid}/activate")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<BannerResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<BannerResponse>>> ActivateBanner(Guid id, CancellationToken cancellationToken)
    {
        var result = await _cmsService.ActivateBannerAsync(id, cancellationToken);
        return Ok(ApiResponse<BannerResponse>.SuccessResponse(result, "Banner activated successfully."));
    }

    [HttpPost("banners/{id:guid}/deactivate")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<BannerResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<BannerResponse>>> DeactivateBanner(Guid id, CancellationToken cancellationToken)
    {
        var result = await _cmsService.DeactivateBannerAsync(id, cancellationToken);
        return Ok(ApiResponse<BannerResponse>.SuccessResponse(result, "Banner deactivated successfully."));
    }

    // Promotions

    [HttpPost("promotions")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<PromotionResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<PromotionResponse>>> CreatePromotion(
        [FromBody] CreatePromotionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _cmsService.CreatePromotionAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetPromotions), new { id = result.Id }, ApiResponse<PromotionResponse>.SuccessResponse(result, "Promotion created successfully."));
    }

    [HttpGet("promotions")]
    [Authorize(Policy = AdminPermissions.CmsView)]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<PromotionResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<PromotionResponse>>>> GetPromotions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _cmsService.GetPromotionsAsync(page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<PromotionResponse>>.SuccessResponse(result));
    }

    [HttpPut("promotions/{id:guid}")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<PromotionResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PromotionResponse>>> UpdatePromotion(
        Guid id,
        [FromBody] UpdatePromotionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _cmsService.UpdatePromotionAsync(id, request, cancellationToken);
        return Ok(ApiResponse<PromotionResponse>.SuccessResponse(result, "Promotion updated successfully."));
    }

    // Announcements

    [HttpPost("announcements")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<AnnouncementResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<AnnouncementResponse>>> CreateAnnouncement(
        [FromBody] CreateAnnouncementRequest request,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _cmsService.CreateAnnouncementAsync(request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetAnnouncements), new { id = result.Id }, ApiResponse<AnnouncementResponse>.SuccessResponse(result, "Announcement created successfully."));
    }

    [HttpGet("announcements")]
    [Authorize(Policy = AdminPermissions.CmsView)]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<AnnouncementResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<AnnouncementResponse>>>> GetAnnouncements(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _cmsService.GetAnnouncementsAsync(page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<AnnouncementResponse>>.SuccessResponse(result));
    }

    [HttpPut("announcements/{id:guid}")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<AnnouncementResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<AnnouncementResponse>>> UpdateAnnouncement(
        Guid id,
        [FromBody] UpdateAnnouncementRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _cmsService.UpdateAnnouncementAsync(id, request, cancellationToken);
        return Ok(ApiResponse<AnnouncementResponse>.SuccessResponse(result, "Announcement updated successfully."));
    }

    [HttpPost("announcements/{id:guid}/publish")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<AnnouncementResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<AnnouncementResponse>>> PublishAnnouncement(Guid id, CancellationToken cancellationToken)
    {
        var result = await _cmsService.PublishAnnouncementAsync(id, cancellationToken);
        return Ok(ApiResponse<AnnouncementResponse>.SuccessResponse(result, "Announcement published successfully."));
    }

    [HttpPost("announcements/{id:guid}/unpublish")]
    [Authorize(Policy = AdminPermissions.CmsManage)]
    [ProducesResponseType(typeof(ApiResponse<AnnouncementResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<AnnouncementResponse>>> UnpublishAnnouncement(Guid id, CancellationToken cancellationToken)
    {
        var result = await _cmsService.UnpublishAnnouncementAsync(id, cancellationToken);
        return Ok(ApiResponse<AnnouncementResponse>.SuccessResponse(result, "Announcement unpublished successfully."));
    }
}
