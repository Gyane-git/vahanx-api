using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Marketplace;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for dealer operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class DealersController : ControllerBase
{
    private readonly IDealerService _service;
    private readonly ILogger<DealersController> _logger;

    public DealersController(IDealerService service, ILogger<DealersController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all dealers with pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<DealerResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<DealerResponse>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<DealerResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get dealer by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<DealerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<DealerResponse>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Dealer not found."));

        return Ok(ApiResponse<DealerResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new dealer.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<DealerResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<DealerResponse>>> Create(
        [FromBody] CreateDealerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<DealerResponse>.SuccessResponse(result, "Dealer created successfully."));
    }

    /// <summary>
    /// Update an existing dealer.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<DealerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<DealerResponse>>> Update(
        Guid id,
        [FromBody] UpdateDealerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        return Ok(ApiResponse<DealerResponse>.SuccessResponse(result, "Dealer updated successfully."));
    }

    /// <summary>
    /// Delete a dealer.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Get listings for a specific dealer.
    /// </summary>
    [HttpGet("{id:guid}/listings")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ListingResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PagedResult<ListingResponse>>>> GetDealerListings(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetDealerListingsAsync(id, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ListingResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get branches for a specific dealer.
    /// </summary>
    [HttpGet("{id:guid}/branches")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<DealerBranchResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<DealerBranchResponse>>>> GetBranches(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetBranchesAsync(id, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<DealerBranchResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new branch for a dealer.
    /// </summary>
    [HttpPost("{id:guid}/branches")]
    [ProducesResponseType(typeof(ApiResponse<DealerBranchResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<DealerBranchResponse>>> CreateBranch(
        Guid id,
        [FromBody] CreateDealerBranchRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateBranchAsync(id, request, cancellationToken);
        return CreatedAtAction(nameof(GetBranches), new { id }, ApiResponse<DealerBranchResponse>.SuccessResponse(result, "Dealer branch created successfully."));
    }

    /// <summary>
    /// Update a dealer branch.
    /// </summary>
    [HttpPut("{id:guid}/branches/{branchId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<DealerBranchResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<DealerBranchResponse>>> UpdateBranch(
        Guid id,
        Guid branchId,
        [FromBody] UpdateDealerBranchRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateBranchAsync(id, branchId, request, cancellationToken);
        return Ok(ApiResponse<DealerBranchResponse>.SuccessResponse(result, "Dealer branch updated successfully."));
    }

    /// <summary>
    /// Delete a dealer branch.
    /// </summary>
    [HttpDelete("{id:guid}/branches/{branchId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBranch(Guid id, Guid branchId, CancellationToken cancellationToken)
    {
        await _service.DeleteBranchAsync(id, branchId, cancellationToken);
        return NoContent();
    }
}
