using VahanX.Application.Common;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for model operations.
/// </summary>
public interface IModelService
{
    Task<PagedResult<ModelDto>> GetAllAsync(int page, int pageSize, Guid? brandId = null, string? search = null, CancellationToken cancellationToken = default);
    Task<ModelDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ModelDto> CreateAsync(CreateModelRequest request, CancellationToken cancellationToken = default);
    Task<ModelDto> UpdateAsync(Guid id, UpdateModelRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
