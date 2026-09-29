using VahanX.Application.Common;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for master data operations (BodyType, FuelType, TransmissionType, DriveType, EngineType, VehicleFeature, VehicleSpecification).
/// </summary>
public interface IMasterDataService
{
    // BodyType
    Task<PagedResult<BodyTypeDto>> GetBodyTypesAsync(int page, int pageSize, string? search = null, CancellationToken cancellationToken = default);
    Task<BodyTypeDto?> GetBodyTypeByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BodyTypeDto> CreateBodyTypeAsync(CreateBodyTypeRequest request, CancellationToken cancellationToken = default);
    Task<BodyTypeDto> UpdateBodyTypeAsync(Guid id, UpdateBodyTypeRequest request, CancellationToken cancellationToken = default);
    Task DeleteBodyTypeAsync(Guid id, CancellationToken cancellationToken = default);

    // FuelType
    Task<PagedResult<FuelTypeDto>> GetFuelTypesAsync(int page, int pageSize, string? search = null, CancellationToken cancellationToken = default);
    Task<FuelTypeDto?> GetFuelTypeByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<FuelTypeDto> CreateFuelTypeAsync(CreateFuelTypeRequest request, CancellationToken cancellationToken = default);
    Task<FuelTypeDto> UpdateFuelTypeAsync(Guid id, UpdateFuelTypeRequest request, CancellationToken cancellationToken = default);
    Task DeleteFuelTypeAsync(Guid id, CancellationToken cancellationToken = default);

    // TransmissionType
    Task<PagedResult<TransmissionTypeDto>> GetTransmissionTypesAsync(int page, int pageSize, string? search = null, CancellationToken cancellationToken = default);
    Task<TransmissionTypeDto?> GetTransmissionTypeByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TransmissionTypeDto> CreateTransmissionTypeAsync(CreateTransmissionTypeRequest request, CancellationToken cancellationToken = default);
    Task<TransmissionTypeDto> UpdateTransmissionTypeAsync(Guid id, UpdateTransmissionTypeRequest request, CancellationToken cancellationToken = default);
    Task DeleteTransmissionTypeAsync(Guid id, CancellationToken cancellationToken = default);

    // DriveType
    Task<PagedResult<DriveTypeDto>> GetDriveTypesAsync(int page, int pageSize, string? search = null, CancellationToken cancellationToken = default);
    Task<DriveTypeDto?> GetDriveTypeByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DriveTypeDto> CreateDriveTypeAsync(CreateDriveTypeRequest request, CancellationToken cancellationToken = default);
    Task<DriveTypeDto> UpdateDriveTypeAsync(Guid id, UpdateDriveTypeRequest request, CancellationToken cancellationToken = default);
    Task DeleteDriveTypeAsync(Guid id, CancellationToken cancellationToken = default);

    // EngineType
    Task<PagedResult<EngineTypeDto>> GetEngineTypesAsync(int page, int pageSize, string? search = null, CancellationToken cancellationToken = default);
    Task<EngineTypeDto?> GetEngineTypeByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<EngineTypeDto> CreateEngineTypeAsync(CreateEngineTypeRequest request, CancellationToken cancellationToken = default);
    Task<EngineTypeDto> UpdateEngineTypeAsync(Guid id, UpdateEngineTypeRequest request, CancellationToken cancellationToken = default);
    Task DeleteEngineTypeAsync(Guid id, CancellationToken cancellationToken = default);

    // VehicleFeature
    Task<PagedResult<VehicleFeatureDto>> GetVehicleFeaturesAsync(int page, int pageSize, string? search = null, string? featureGroup = null, CancellationToken cancellationToken = default);
    Task<VehicleFeatureDto?> GetVehicleFeatureByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<VehicleFeatureDto> CreateVehicleFeatureAsync(CreateVehicleFeatureRequest request, CancellationToken cancellationToken = default);
    Task<VehicleFeatureDto> UpdateVehicleFeatureAsync(Guid id, UpdateVehicleFeatureRequest request, CancellationToken cancellationToken = default);
    Task DeleteVehicleFeatureAsync(Guid id, CancellationToken cancellationToken = default);

    // VehicleSpecification
    Task<PagedResult<VehicleSpecificationDto>> GetVehicleSpecificationsAsync(int page, int pageSize, string? search = null, string? specGroup = null, CancellationToken cancellationToken = default);
    Task<VehicleSpecificationDto?> GetVehicleSpecificationByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<VehicleSpecificationDto> CreateVehicleSpecificationAsync(CreateVehicleSpecificationRequest request, CancellationToken cancellationToken = default);
    Task<VehicleSpecificationDto> UpdateVehicleSpecificationAsync(Guid id, UpdateVehicleSpecificationRequest request, CancellationToken cancellationToken = default);
    Task DeleteVehicleSpecificationAsync(Guid id, CancellationToken cancellationToken = default);
}
