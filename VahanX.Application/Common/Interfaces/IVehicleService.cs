using VahanX.Application.Common;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for vehicle operations.
/// </summary>
public interface IVehicleService
{
    Task<PagedResult<VehicleDto>> GetAllAsync(int page, int pageSize, Guid? variantId = null, Guid? fuelTypeId = null, Guid? bodyTypeId = null, CancellationToken cancellationToken = default);
    Task<VehicleDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<VehicleDto> CreateAsync(CreateVehicleRequest request, CancellationToken cancellationToken = default);
    Task<VehicleDto> UpdateAsync(Guid id, UpdateVehicleRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
