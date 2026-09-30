using VahanX.Application.Common;
using VahanX.Application.DTOs.Marketplace;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for marketplace search operations.
/// </summary>
public interface ISearchService
{
    Task<PagedResult<SearchVehicleResponse>> SearchVehiclesAsync(SearchVehicleRequest request, CancellationToken cancellationToken = default);
}
