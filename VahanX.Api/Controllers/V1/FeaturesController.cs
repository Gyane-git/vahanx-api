using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Business;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for feature management.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class FeaturesController : ControllerBase
{
    private readonly IFeatureService _service;
    private readonly ILogger<FeaturesController> _logger;

    public FeaturesController(IFeatureService service, ILogger<FeaturesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all features.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FeatureResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<FeatureResponse>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<FeatureResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get feature by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<FeatureResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FeatureResponse>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Feature not found."));

        return Ok(ApiResponse<FeatureResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new feature.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FeatureResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<FeatureResponse>>> Create(
        [FromBody] CreateFeatureRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<FeatureResponse>.SuccessResponse(result, "Feature created successfully."));
    }

    /// <summary>
    /// Update a feature.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<FeatureResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FeatureResponse>>> Update(
        Guid id,
        [FromBody] UpdateFeatureRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        return Ok(ApiResponse<FeatureResponse>.SuccessResponse(result, "Feature updated successfully."));
    }
}
