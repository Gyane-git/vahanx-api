using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Analytics;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for analytics operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/admin/analytics")]
[ApiVersion("1.0")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;
    private readonly ILogger<AnalyticsController> _logger;

    public AnalyticsController(IAnalyticsService analyticsService, ILogger<AnalyticsController> logger)
    {
        _analyticsService = analyticsService;
        _logger = logger;
    }

    [HttpGet("overview")]
    [ProducesResponseType(typeof(ApiResponse<AnalyticsOverviewDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<AnalyticsOverviewDto>>> GetOverview(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? period,
        CancellationToken cancellationToken)
    {
        var parameters = new AnalyticsQueryParams { From = from, To = to, Period = period };
        var result = await _analyticsService.GetOverviewAsync(parameters, cancellationToken);
        return Ok(ApiResponse<AnalyticsOverviewDto>.SuccessResponse(result));
    }

    [HttpGet("users")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<UserAnalyticsDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<UserAnalyticsDto>>>> GetUserAnalytics(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? period,
        [FromQuery] string? groupBy,
        CancellationToken cancellationToken)
    {
        var parameters = new AnalyticsQueryParams { From = from, To = to, Period = period, GroupBy = groupBy };
        var result = await _analyticsService.GetUserAnalyticsAsync(parameters, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<UserAnalyticsDto>>.SuccessResponse(result));
    }

    [HttpGet("listings")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ListingAnalyticsDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ListingAnalyticsDto>>>> GetListingAnalytics(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? period,
        [FromQuery] string? groupBy,
        CancellationToken cancellationToken)
    {
        var parameters = new AnalyticsQueryParams { From = from, To = to, Period = period, GroupBy = groupBy };
        var result = await _analyticsService.GetListingAnalyticsAsync(parameters, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<ListingAnalyticsDto>>.SuccessResponse(result));
    }

    [HttpGet("vehicles")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<VehicleAnalyticsDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<VehicleAnalyticsDto>>>> GetVehicleAnalytics(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? period,
        CancellationToken cancellationToken)
    {
        var parameters = new AnalyticsQueryParams { From = from, To = to, Period = period };
        var result = await _analyticsService.GetVehicleAnalyticsAsync(parameters, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<VehicleAnalyticsDto>>.SuccessResponse(result));
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SearchAnalyticsDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SearchAnalyticsDto>>>> GetSearchAnalytics(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? period,
        CancellationToken cancellationToken)
    {
        var parameters = new AnalyticsQueryParams { From = from, To = to, Period = period };
        var result = await _analyticsService.GetSearchAnalyticsAsync(parameters, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<SearchAnalyticsDto>>.SuccessResponse(result));
    }

    [HttpGet("enquiries")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<EnquiryAnalyticsDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<EnquiryAnalyticsDto>>>> GetEnquiryAnalytics(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? period,
        CancellationToken cancellationToken)
    {
        var parameters = new AnalyticsQueryParams { From = from, To = to, Period = period };
        var result = await _analyticsService.GetEnquiryAnalyticsAsync(parameters, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<EnquiryAnalyticsDto>>.SuccessResponse(result));
    }

    [HttpGet("test-drives")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<TestDriveAnalyticsDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<TestDriveAnalyticsDto>>>> GetTestDriveAnalytics(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? period,
        CancellationToken cancellationToken)
    {
        var parameters = new AnalyticsQueryParams { From = from, To = to, Period = period };
        var result = await _analyticsService.GetTestDriveAnalyticsAsync(parameters, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<TestDriveAnalyticsDto>>.SuccessResponse(result));
    }

    [HttpGet("services")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ServiceAnalyticsDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ServiceAnalyticsDto>>>> GetServiceAnalytics(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? period,
        CancellationToken cancellationToken)
    {
        var parameters = new AnalyticsQueryParams { From = from, To = to, Period = period };
        var result = await _analyticsService.GetServiceAnalyticsAsync(parameters, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<ServiceAnalyticsDto>>.SuccessResponse(result));
    }

    [HttpGet("advertisements")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AdvertisementAnalyticsDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AdvertisementAnalyticsDto>>>> GetAdvertisementAnalytics(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? period,
        CancellationToken cancellationToken)
    {
        var parameters = new AnalyticsQueryParams { From = from, To = to, Period = period };
        var result = await _analyticsService.GetAdvertisementAnalyticsAsync(parameters, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<AdvertisementAnalyticsDto>>.SuccessResponse(result));
    }

    [HttpGet("subscriptions")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SubscriptionAnalyticsDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SubscriptionAnalyticsDto>>>> GetSubscriptionAnalytics(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? period,
        CancellationToken cancellationToken)
    {
        var parameters = new AnalyticsQueryParams { From = from, To = to, Period = period };
        var result = await _analyticsService.GetSubscriptionAnalyticsAsync(parameters, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<SubscriptionAnalyticsDto>>.SuccessResponse(result));
    }

    [HttpGet("sell-vehicles")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SellVehicleAnalyticsDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SellVehicleAnalyticsDto>>>> GetSellVehicleAnalytics(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? period,
        CancellationToken cancellationToken)
    {
        var parameters = new AnalyticsQueryParams { From = from, To = to, Period = period };
        var result = await _analyticsService.GetSellVehicleAnalyticsAsync(parameters, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<SellVehicleAnalyticsDto>>.SuccessResponse(result));
    }

    [HttpGet("revenue")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RevenueAnalyticsDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<RevenueAnalyticsDto>>>> GetRevenueAnalytics(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? period,
        CancellationToken cancellationToken)
    {
        var parameters = new AnalyticsQueryParams { From = from, To = to, Period = period };
        var result = await _analyticsService.GetRevenueAnalyticsAsync(parameters, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<RevenueAnalyticsDto>>.SuccessResponse(result));
    }

    [HttpGet("platform")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PlatformAnalyticsDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PlatformAnalyticsDto>>>> GetPlatformAnalytics(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? period,
        CancellationToken cancellationToken)
    {
        var parameters = new AnalyticsQueryParams { From = from, To = to, Period = period };
        var result = await _analyticsService.GetPlatformAnalyticsAsync(parameters, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<PlatformAnalyticsDto>>.SuccessResponse(result));
    }
}
