using VahanX.Application.Common;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for variant operations.
/// </summary>
public interface IVariantService
{
    Task<PagedResult<VariantDto>> GetAllAsync(int page, int pageSize, Guid? generationId = null, string? search = null, CancellationToken cancellationToken = default);
    Task<VariantDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<VariantDto> CreateAsync(CreateVariantRequest request, CancellationToken cancellationToken = default);
    Task<VariantDto> UpdateAsync(Guid id, UpdateVariantRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
