using VahanX.Application.Common;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for vehicle category operations.
/// </summary>
public interface IVehicleCategoryService
{
    Task<PagedResult<VehicleCategoryDto>> GetAllAsync(int page, int pageSize, Guid? vehicleTypeId = null, string? search = null, CancellationToken cancellationToken = default);
    Task<VehicleCategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<VehicleCategoryDto> CreateAsync(CreateVehicleCategoryRequest request, CancellationToken cancellationToken = default);
    Task<VehicleCategoryDto> UpdateAsync(Guid id, UpdateVehicleCategoryRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
