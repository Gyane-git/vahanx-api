using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Marketplace;
using VahanX.Domain.Enums;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for marketplace search operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/search")]
[ApiVersion("1.0")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _service;
    private readonly ILogger<SearchController> _logger;

    public SearchController(ISearchService service, ILogger<SearchController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Search for published vehicle listings.
    /// </summary>
    [HttpGet("vehicles")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<SearchVehicleResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<SearchVehicleResponse>>>> SearchVehicles(
        [FromQuery] string? keyword = null,
        [FromQuery] Guid? brandId = null,
        [FromQuery] Guid? modelId = null,
        [FromQuery] Guid? variantId = null,
        [FromQuery] Guid? vehicleTypeId = null,
        [FromQuery] Guid? categoryId = null,
        [FromQuery] Guid? fuelTypeId = null,
        [FromQuery] Guid? transmissionTypeId = null,
        [FromQuery] Guid? bodyTypeId = null,
        [FromQuery] Guid? locationId = null,
        [FromQuery] Guid? sellerId = null,
        [FromQuery] Guid? dealerId = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] int? minMileage = null,
        [FromQuery] int? maxMileage = null,
        [FromQuery] int? manufactureYear = null,
        [FromQuery] SortOrder sortBy = SortOrder.Newest,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new SearchVehicleRequest
        {
            Keyword = keyword,
            BrandId = brandId,
            ModelId = modelId,
            VariantId = variantId,
            VehicleTypeId = vehicleTypeId,
            CategoryId = categoryId,
            FuelTypeId = fuelTypeId,
            TransmissionTypeId = transmissionTypeId,
            BodyTypeId = bodyTypeId,
            LocationId = locationId,
            SellerId = sellerId,
            DealerId = dealerId,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            MinMileage = minMileage,
            MaxMileage = maxMileage,
            ManufactureYear = manufactureYear,
            SortBy = sortBy,
            Page = page,
            PageSize = pageSize
        };

        var result = await _service.SearchVehiclesAsync(request, cancellationToken);
        return Ok(ApiResponse<PagedResult<SearchVehicleResponse>>.SuccessResponse(result));
    }
}
