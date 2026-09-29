namespace VahanX.Application.DTOs.VehicleCore;

/// <summary>
/// Vehicle response DTO.
/// </summary>
public class VehicleDto
{
    public Guid Id { get; set; }
    public Guid VariantId { get; set; }
    public string VariantName { get; set; } = string.Empty;
    public Guid BodyTypeId { get; set; }
    public string BodyTypeName { get; set; } = string.Empty;
    public Guid FuelTypeId { get; set; }
    public string FuelTypeName { get; set; } = string.Empty;
    public Guid TransmissionTypeId { get; set; }
    public string TransmissionTypeName { get; set; } = string.Empty;
    public Guid DriveTypeId { get; set; }
    public string DriveTypeName { get; set; } = string.Empty;
    public Guid EngineTypeId { get; set; }
    public string EngineTypeName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// Create vehicle request.
/// </summary>
public class CreateVehicleRequest
{
    public Guid VariantId { get; set; }
    public Guid BodyTypeId { get; set; }
    public Guid FuelTypeId { get; set; }
    public Guid TransmissionTypeId { get; set; }
    public Guid DriveTypeId { get; set; }
    public Guid EngineTypeId { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Update vehicle request.
/// </summary>
public class UpdateVehicleRequest
{
    public Guid BodyTypeId { get; set; }
    public Guid FuelTypeId { get; set; }
    public Guid TransmissionTypeId { get; set; }
    public Guid DriveTypeId { get; set; }
    public Guid EngineTypeId { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
