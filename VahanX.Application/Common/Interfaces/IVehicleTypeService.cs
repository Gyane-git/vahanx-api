using VahanX.Application.Common;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for vehicle type operations.
/// </summary>
public interface IVehicleTypeService
{
    Task<PagedResult<VehicleTypeDto>> GetAllAsync(int page, int pageSize, string? search = null, CancellationToken cancellationToken = default);
    Task<VehicleTypeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<VehicleTypeDto> CreateAsync(CreateVehicleTypeRequest request, CancellationToken cancellationToken = default);
    Task<VehicleTypeDto> UpdateAsync(Guid id, UpdateVehicleTypeRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
