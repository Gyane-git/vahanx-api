using VahanX.Application.DTOs.Marketplace;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for wishlist operations.
/// </summary>
public interface IWishlistService
{
    Task<WishlistResponse> GetWishlistAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<WishlistResponse> AddItemAsync(Guid userId, Guid listingId, CancellationToken cancellationToken = default);
    Task<WishlistResponse> RemoveItemAsync(Guid userId, Guid listingId, CancellationToken cancellationToken = default);
}
