using VahanX.Application.Common;
using VahanX.Application.DTOs.Marketplace;
using VahanX.Domain.Enums;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for vehicle listing operations.
/// </summary>
public interface IListingService
{
    Task<PagedResult<ListingResponse>> GetAllAsync(int page, int pageSize, Guid? vehicleId = null, Guid? sellerId = null, Guid? dealerId = null, ListingStatus? status = null, Guid? locationId = null, decimal? minPrice = null, decimal? maxPrice = null, int? minMileage = null, int? maxMileage = null, CancellationToken cancellationToken = default);
    Task<ListingResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListingResponse> CreateAsync(CreateListingRequest request, CancellationToken cancellationToken = default);
    Task<ListingResponse> UpdateAsync(Guid id, UpdateListingRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListingResponse> SubmitAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListingResponse> PublishAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListingResponse> PauseAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListingResponse> MarkSoldAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListingResponse> ArchiveAsync(Guid id, CancellationToken cancellationToken = default);
}
