using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for master data operations (BodyType, FuelType, TransmissionType, DriveType, EngineType, VehicleFeature, VehicleSpecification).
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class MasterDataController : ControllerBase
{
    private readonly IMasterDataService _service;
    private readonly ILogger<MasterDataController> _logger;

    public MasterDataController(IMasterDataService service, ILogger<MasterDataController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // BodyType
    [HttpGet("body-types")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<BodyTypeDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<BodyTypeDto>>>> GetBodyTypes(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetBodyTypesAsync(page, pageSize, search, cancellationToken);
        return Ok(ApiResponse<PagedResult<BodyTypeDto>>.SuccessResponse(result));
    }

    [HttpGet("body-types/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<BodyTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<BodyTypeDto>>> GetBodyTypeById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetBodyTypeByIdAsync(id, cancellationToken);
        if (result is null) return NotFound(ApiResponse<object>.ErrorResponse("Body type not found."));
        return Ok(ApiResponse<BodyTypeDto>.SuccessResponse(result));
    }

    [HttpPost("body-types")]
    [ProducesResponseType(typeof(ApiResponse<BodyTypeDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<BodyTypeDto>>> CreateBodyType([FromBody] CreateBodyTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateBodyTypeAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetBodyTypeById), new { id = result.Id }, ApiResponse<BodyTypeDto>.SuccessResponse(result, "Body type created successfully."));
    }

    [HttpPut("body-types/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<BodyTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<BodyTypeDto>>> UpdateBodyType(Guid id, [FromBody] UpdateBodyTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateBodyTypeAsync(id, request, cancellationToken);
        return Ok(ApiResponse<BodyTypeDto>.SuccessResponse(result, "Body type updated successfully."));
    }

    [HttpDelete("body-types/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteBodyType(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteBodyTypeAsync(id, cancellationToken);
        return NoContent();
    }

    // FuelType
    [HttpGet("fuel-types")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FuelTypeDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<FuelTypeDto>>>> GetFuelTypes(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetFuelTypesAsync(page, pageSize, search, cancellationToken);
        return Ok(ApiResponse<PagedResult<FuelTypeDto>>.SuccessResponse(result));
    }

    [HttpGet("fuel-types/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<FuelTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<FuelTypeDto>>> GetFuelTypeById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetFuelTypeByIdAsync(id, cancellationToken);
        if (result is null) return NotFound(ApiResponse<object>.ErrorResponse("Fuel type not found."));
        return Ok(ApiResponse<FuelTypeDto>.SuccessResponse(result));
    }

    [HttpPost("fuel-types")]
    [ProducesResponseType(typeof(ApiResponse<FuelTypeDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<FuelTypeDto>>> CreateFuelType([FromBody] CreateFuelTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateFuelTypeAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetFuelTypeById), new { id = result.Id }, ApiResponse<FuelTypeDto>.SuccessResponse(result, "Fuel type created successfully."));
    }

    [HttpPut("fuel-types/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<FuelTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<FuelTypeDto>>> UpdateFuelType(Guid id, [FromBody] UpdateFuelTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateFuelTypeAsync(id, request, cancellationToken);
        return Ok(ApiResponse<FuelTypeDto>.SuccessResponse(result, "Fuel type updated successfully."));
    }

    [HttpDelete("fuel-types/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteFuelType(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteFuelTypeAsync(id, cancellationToken);
        return NoContent();
    }

    // TransmissionType
    [HttpGet("transmissions")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<TransmissionTypeDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<TransmissionTypeDto>>>> GetTransmissionTypes(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetTransmissionTypesAsync(page, pageSize, search, cancellationToken);
        return Ok(ApiResponse<PagedResult<TransmissionTypeDto>>.SuccessResponse(result));
    }

    [HttpGet("transmissions/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<TransmissionTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<TransmissionTypeDto>>> GetTransmissionTypeById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetTransmissionTypeByIdAsync(id, cancellationToken);
        if (result is null) return NotFound(ApiResponse<object>.ErrorResponse("Transmission type not found."));
        return Ok(ApiResponse<TransmissionTypeDto>.SuccessResponse(result));
    }

    [HttpPost("transmissions")]
    [ProducesResponseType(typeof(ApiResponse<TransmissionTypeDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<TransmissionTypeDto>>> CreateTransmissionType([FromBody] CreateTransmissionTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateTransmissionTypeAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetTransmissionTypeById), new { id = result.Id }, ApiResponse<TransmissionTypeDto>.SuccessResponse(result, "Transmission type created successfully."));
    }

    [HttpPut("transmissions/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<TransmissionTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<TransmissionTypeDto>>> UpdateTransmissionType(Guid id, [FromBody] UpdateTransmissionTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateTransmissionTypeAsync(id, request, cancellationToken);
        return Ok(ApiResponse<TransmissionTypeDto>.SuccessResponse(result, "Transmission type updated successfully."));
    }

    [HttpDelete("transmissions/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTransmissionType(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteTransmissionTypeAsync(id, cancellationToken);
        return NoContent();
    }

    // DriveType
    [HttpGet("drive-types")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<DriveTypeDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<DriveTypeDto>>>> GetDriveTypes(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetDriveTypesAsync(page, pageSize, search, cancellationToken);
        return Ok(ApiResponse<PagedResult<DriveTypeDto>>.SuccessResponse(result));
    }

    [HttpGet("drive-types/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<DriveTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<DriveTypeDto>>> GetDriveTypeById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetDriveTypeByIdAsync(id, cancellationToken);
        if (result is null) return NotFound(ApiResponse<object>.ErrorResponse("Drive type not found."));
        return Ok(ApiResponse<DriveTypeDto>.SuccessResponse(result));
    }

    [HttpPost("drive-types")]
    [ProducesResponseType(typeof(ApiResponse<DriveTypeDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<DriveTypeDto>>> CreateDriveType([FromBody] CreateDriveTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateDriveTypeAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetDriveTypeById), new { id = result.Id }, ApiResponse<DriveTypeDto>.SuccessResponse(result, "Drive type created successfully."));
    }

    [HttpPut("drive-types/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<DriveTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<DriveTypeDto>>> UpdateDriveType(Guid id, [FromBody] UpdateDriveTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateDriveTypeAsync(id, request, cancellationToken);
        return Ok(ApiResponse<DriveTypeDto>.SuccessResponse(result, "Drive type updated successfully."));
    }

    [HttpDelete("drive-types/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteDriveType(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteDriveTypeAsync(id, cancellationToken);
        return NoContent();
    }

    // EngineType
    [HttpGet("engine-types")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<EngineTypeDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<EngineTypeDto>>>> GetEngineTypes(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetEngineTypesAsync(page, pageSize, search, cancellationToken);
        return Ok(ApiResponse<PagedResult<EngineTypeDto>>.SuccessResponse(result));
    }

    [HttpGet("engine-types/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<EngineTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<EngineTypeDto>>> GetEngineTypeById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetEngineTypeByIdAsync(id, cancellationToken);
        if (result is null) return NotFound(ApiResponse<object>.ErrorResponse("Engine type not found."));
        return Ok(ApiResponse<EngineTypeDto>.SuccessResponse(result));
    }

    [HttpPost("engine-types")]
    [ProducesResponseType(typeof(ApiResponse<EngineTypeDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<EngineTypeDto>>> CreateEngineType([FromBody] CreateEngineTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateEngineTypeAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetEngineTypeById), new { id = result.Id }, ApiResponse<EngineTypeDto>.SuccessResponse(result, "Engine type created successfully."));
    }

    [HttpPut("engine-types/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<EngineTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<EngineTypeDto>>> UpdateEngineType(Guid id, [FromBody] UpdateEngineTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateEngineTypeAsync(id, request, cancellationToken);
        return Ok(ApiResponse<EngineTypeDto>.SuccessResponse(result, "Engine type updated successfully."));
    }

    [HttpDelete("engine-types/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteEngineType(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteEngineTypeAsync(id, cancellationToken);
        return NoContent();
    }

    // VehicleFeature
    [HttpGet("features")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<VehicleFeatureDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<VehicleFeatureDto>>>> GetVehicleFeatures(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, [FromQuery] string? featureGroup = null, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetVehicleFeaturesAsync(page, pageSize, search, featureGroup, cancellationToken);
        return Ok(ApiResponse<PagedResult<VehicleFeatureDto>>.SuccessResponse(result));
    }

    [HttpGet("features/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VehicleFeatureDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<VehicleFeatureDto>>> GetVehicleFeatureById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetVehicleFeatureByIdAsync(id, cancellationToken);
        if (result is null) return NotFound(ApiResponse<object>.ErrorResponse("Vehicle feature not found."));
        return Ok(ApiResponse<VehicleFeatureDto>.SuccessResponse(result));
    }

    [HttpPost("features")]
    [ProducesResponseType(typeof(ApiResponse<VehicleFeatureDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<VehicleFeatureDto>>> CreateVehicleFeature([FromBody] CreateVehicleFeatureRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateVehicleFeatureAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetVehicleFeatureById), new { id = result.Id }, ApiResponse<VehicleFeatureDto>.SuccessResponse(result, "Vehicle feature created successfully."));
    }

    [HttpPut("features/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VehicleFeatureDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<VehicleFeatureDto>>> UpdateVehicleFeature(Guid id, [FromBody] UpdateVehicleFeatureRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateVehicleFeatureAsync(id, request, cancellationToken);
        return Ok(ApiResponse<VehicleFeatureDto>.SuccessResponse(result, "Vehicle feature updated successfully."));
    }

    [HttpDelete("features/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteVehicleFeature(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteVehicleFeatureAsync(id, cancellationToken);
        return NoContent();
    }

    // VehicleSpecification
    [HttpGet("specifications")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<VehicleSpecificationDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<VehicleSpecificationDto>>>> GetVehicleSpecifications(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, [FromQuery] string? specGroup = null, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetVehicleSpecificationsAsync(page, pageSize, search, specGroup, cancellationToken);
        return Ok(ApiResponse<PagedResult<VehicleSpecificationDto>>.SuccessResponse(result));
    }

    [HttpGet("specifications/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VehicleSpecificationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<VehicleSpecificationDto>>> GetVehicleSpecificationById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetVehicleSpecificationByIdAsync(id, cancellationToken);
        if (result is null) return NotFound(ApiResponse<object>.ErrorResponse("Vehicle specification not found."));
        return Ok(ApiResponse<VehicleSpecificationDto>.SuccessResponse(result));
    }

    [HttpPost("specifications")]
    [ProducesResponseType(typeof(ApiResponse<VehicleSpecificationDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<VehicleSpecificationDto>>> CreateVehicleSpecification([FromBody] CreateVehicleSpecificationRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateVehicleSpecificationAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetVehicleSpecificationById), new { id = result.Id }, ApiResponse<VehicleSpecificationDto>.SuccessResponse(result, "Vehicle specification created successfully."));
    }

    [HttpPut("specifications/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VehicleSpecificationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<VehicleSpecificationDto>>> UpdateVehicleSpecification(Guid id, [FromBody] UpdateVehicleSpecificationRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateVehicleSpecificationAsync(id, request, cancellationToken);
        return Ok(ApiResponse<VehicleSpecificationDto>.SuccessResponse(result, "Vehicle specification updated successfully."));
    }

    [HttpDelete("specifications/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteVehicleSpecification(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteVehicleSpecificationAsync(id, cancellationToken);
        return NoContent();
    }
}
