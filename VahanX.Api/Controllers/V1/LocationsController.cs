using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Marketplace;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;
using VahanX.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for location operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class LocationsController : ControllerBase
{
    private readonly IRepository<Location> _repository;
    private readonly ILogger<LocationsController> _logger;

    public LocationsController(IRepository<Location> repository, ILogger<LocationsController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Get all locations with pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<LocationResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<LocationResponse>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(l => l.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(l => new LocationResponse
            {
                Id = l.Id,
                Name = l.Name,
                Province = l.Province,
                District = l.District,
                Municipality = l.Municipality,
                Ward = l.Ward,
                StreetAddress = l.StreetAddress,
                Latitude = l.Latitude,
                Longitude = l.Longitude
            })
            .ToListAsync(cancellationToken);

        return Ok(ApiResponse<PagedResult<LocationResponse>>.SuccessResponse(PagedResult<LocationResponse>.Create(items, totalCount, page, pageSize)));
    }

    /// <summary>
    /// Get location by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<LocationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<LocationResponse>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var location = await _repository.GetByIdAsync(id, cancellationToken);
        if (location is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Location not found."));

        return Ok(ApiResponse<LocationResponse>.SuccessResponse(new LocationResponse
        {
            Id = location.Id,
            Name = location.Name,
            Province = location.Province,
            District = location.District,
            Municipality = location.Municipality,
            Ward = location.Ward,
            StreetAddress = location.StreetAddress,
            Latitude = location.Latitude,
            Longitude = location.Longitude
        }));
    }

    /// <summary>
    /// Create a new location.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<LocationResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<LocationResponse>>> Create(
        [FromBody] CreateLocationRequest request,
        CancellationToken cancellationToken)
    {
        var location = new Location
        {
            Name = request.Name,
            Province = request.Province,
            District = request.District,
            Municipality = request.Municipality,
            Ward = request.Ward,
            StreetAddress = request.StreetAddress,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        };

        await _repository.AddAsync(location, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = location.Id }, ApiResponse<LocationResponse>.SuccessResponse(new LocationResponse
        {
            Id = location.Id,
            Name = location.Name,
            Province = location.Province,
            District = location.District,
            Municipality = location.Municipality,
            Ward = location.Ward,
            StreetAddress = location.StreetAddress,
            Latitude = location.Latitude,
            Longitude = location.Longitude
        }, "Location created successfully."));
    }

    /// <summary>
    /// Update an existing location.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<LocationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<LocationResponse>>> Update(
        Guid id,
        [FromBody] UpdateLocationRequest request,
        CancellationToken cancellationToken)
    {
        var location = await _repository.GetByIdAsync(id, cancellationToken);
        if (location is null) throw new NotFoundException("Location", id);

        location.Name = request.Name;
        location.Province = request.Province;
        location.District = request.District;
        location.Municipality = request.Municipality;
        location.Ward = request.Ward;
        location.StreetAddress = request.StreetAddress;
        location.Latitude = request.Latitude;
        location.Longitude = request.Longitude;

        await _repository.UpdateAsync(location, cancellationToken);

        return Ok(ApiResponse<LocationResponse>.SuccessResponse(new LocationResponse
        {
            Id = location.Id,
            Name = location.Name,
            Province = location.Province,
            District = location.District,
            Municipality = location.Municipality,
            Ward = location.Ward,
            StreetAddress = location.StreetAddress,
            Latitude = location.Latitude,
            Longitude = location.Longitude
        }, "Location updated successfully."));
    }

    /// <summary>
    /// Delete a location.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var location = await _repository.GetByIdAsync(id, cancellationToken);
        if (location is null) throw new NotFoundException("Location", id);

        await _repository.DeleteAsync(location, cancellationToken);
        return NoContent();
    }
}
