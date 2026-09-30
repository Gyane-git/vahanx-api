using VahanX.Application.Common;
using VahanX.Application.DTOs.Marketplace;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for listing media operations.
/// </summary>
public interface IListingMediaService
{
    Task<ListingMediaResponse> CreateAsync(Guid listingId, CreateListingMediaRequest request, CancellationToken cancellationToken = default);
    Task<ListingMediaResponse> UpdateAsync(Guid listingId, Guid mediaId, UpdateListingMediaRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid listingId, Guid mediaId, CancellationToken cancellationToken = default);
    Task<ListingMediaResponse> SetPrimaryAsync(Guid listingId, Guid mediaId, CancellationToken cancellationToken = default);
    Task<PagedResult<ListingMediaResponse>> GetByListingAsync(Guid listingId, int page, int pageSize, CancellationToken cancellationToken = default);
}
