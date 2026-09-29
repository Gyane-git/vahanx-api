using VahanX.Application.Common;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for generation operations.
/// </summary>
public interface IGenerationService
{
    Task<PagedResult<GenerationDto>> GetAllAsync(int page, int pageSize, Guid? modelId = null, string? search = null, CancellationToken cancellationToken = default);
    Task<GenerationDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<GenerationDto> CreateAsync(CreateGenerationRequest request, CancellationToken cancellationToken = default);
    Task<GenerationDto> UpdateAsync(Guid id, UpdateGenerationRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
