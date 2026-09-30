using VahanX.Application.Common;
using VahanX.Application.DTOs.Business;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for feature management.
/// </summary>
public interface IFeatureService
{
    Task<PagedResult<FeatureResponse>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<FeatureResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<FeatureResponse> CreateAsync(CreateFeatureRequest request, CancellationToken cancellationToken = default);
    Task<FeatureResponse> UpdateAsync(Guid id, UpdateFeatureRequest request, CancellationToken cancellationToken = default);
}
