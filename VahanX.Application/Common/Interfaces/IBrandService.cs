using VahanX.Application.Common;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for brand operations.
/// </summary>
public interface IBrandService
{
    Task<PagedResult<BrandDto>> GetAllAsync(int page, int pageSize, string? search = null, CancellationToken cancellationToken = default);
    Task<BrandDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BrandDto> CreateAsync(CreateBrandRequest request, CancellationToken cancellationToken = default);
    Task<BrandDto> UpdateAsync(Guid id, UpdateBrandRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
