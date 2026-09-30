using VahanX.Application.Common;
using VahanX.Application.DTOs.Marketplace;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for seller operations.
/// </summary>
public interface ISellerService
{
    Task<PagedResult<SellerResponse>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<SellerResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SellerResponse> CreateAsync(CreateSellerRequest request, CancellationToken cancellationToken = default);
    Task<SellerResponse> UpdateAsync(Guid id, UpdateSellerRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<ListingResponse>> GetSellerListingsAsync(Guid sellerId, int page, int pageSize, CancellationToken cancellationToken = default);
}
