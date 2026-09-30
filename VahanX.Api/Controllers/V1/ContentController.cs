using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Cms;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for public CMS content.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/content")]
[ApiVersion("1.0")]
public class ContentController : ControllerBase
{
    private readonly ICmsService _cmsService;
    private readonly ILogger<ContentController> _logger;

    public ContentController(ICmsService cmsService, ILogger<ContentController> logger)
    {
        _cmsService = cmsService;
        _logger = logger;
    }

    [HttpGet("pages/{slug}")]
    [ProducesResponseType(typeof(ApiResponse<PageResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PageResponse>>> GetPageBySlug(string slug, CancellationToken cancellationToken)
    {
        var result = await _cmsService.GetPublicPageBySlugAsync(slug, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Page not found."));
        return Ok(ApiResponse<PageResponse>.SuccessResponse(result));
    }

    [HttpGet("articles")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ArticleResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ArticleResponse>>>> GetArticles(CancellationToken cancellationToken)
    {
        var result = await _cmsService.GetPublicArticlesAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<ArticleResponse>>.SuccessResponse(result));
    }

    [HttpGet("articles/{slug}")]
    [ProducesResponseType(typeof(ApiResponse<ArticleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ArticleResponse>>> GetArticleBySlug(string slug, CancellationToken cancellationToken)
    {
        var result = await _cmsService.GetPublicArticleBySlugAsync(slug, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Article not found."));
        return Ok(ApiResponse<ArticleResponse>.SuccessResponse(result));
    }

    [HttpGet("categories")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ContentCategoryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ContentCategoryResponse>>>> GetCategories(CancellationToken cancellationToken)
    {
        var result = await _cmsService.GetPublicCategoriesAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<ContentCategoryResponse>>.SuccessResponse(result));
    }

    [HttpGet("faqs")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FaqResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<FaqResponse>>>> GetFaqs(CancellationToken cancellationToken)
    {
        var result = await _cmsService.GetPublicFaqsAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<FaqResponse>>.SuccessResponse(result));
    }

    [HttpGet("banners")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BannerResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BannerResponse>>>> GetBanners(CancellationToken cancellationToken)
    {
        var result = await _cmsService.GetPublicBannersAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<BannerResponse>>.SuccessResponse(result));
    }

    [HttpGet("promotions")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PromotionResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PromotionResponse>>>> GetPromotions(CancellationToken cancellationToken)
    {
        var result = await _cmsService.GetPublicPromotionsAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<PromotionResponse>>.SuccessResponse(result));
    }

    [HttpGet("announcements")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AnnouncementResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AnnouncementResponse>>>> GetAnnouncements(CancellationToken cancellationToken)
    {
        var result = await _cmsService.GetPublicAnnouncementsAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<AnnouncementResponse>>.SuccessResponse(result));
    }
}
