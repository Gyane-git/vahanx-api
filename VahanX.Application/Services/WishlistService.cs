using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Marketplace;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of wishlist service.
/// </summary>
public class WishlistService : IWishlistService
{
    private readonly IRepository<Wishlist> _wishlistRepository;
    private readonly IRepository<WishlistItem> _itemRepository;
    private readonly IRepository<VehicleListing> _listingRepository;

    public WishlistService(
        IRepository<Wishlist> wishlistRepository,
        IRepository<WishlistItem> itemRepository,
        IRepository<VehicleListing> listingRepository)
    {
        _wishlistRepository = wishlistRepository;
        _itemRepository = itemRepository;
        _listingRepository = listingRepository;
    }

    public async Task<WishlistResponse> GetWishlistAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var wishlists = await _wishlistRepository.QueryAsync(true, cancellationToken);
        var wishlist = await wishlists
            .Include(w => w.Items)
            .ThenInclude(i => i.Listing)
            .FirstOrDefaultAsync(w => w.UserId == userId, cancellationToken);

        if (wishlist is null)
        {
            return new WishlistResponse
            {
                Id = Guid.Empty,
                UserId = userId,
                Items = []
            };
        }

        var items = wishlist.Items
            .Where(i => i.Listing != null)
            .Select(i => new WishlistItemResponse
            {
                Id = i.Id,
                ListingId = i.ListingId,
                ListingTitle = i.Listing!.Title,
                VehicleName = i.Listing.Vehicle != null && i.Listing.Vehicle.Variant != null ? i.Listing.Vehicle.Variant.Name : string.Empty,
                Price = i.Listing.Price,
                Currency = i.Listing.Currency,
                PrimaryImageUrl = i.Listing.Media != null ? i.Listing.Media.Where(m => m.IsPrimary).Select(m => m.MediaUrl).FirstOrDefault() : null,
                AddedAt = i.CreatedAt
            })
            .ToList();

        return new WishlistResponse
        {
            Id = wishlist.Id,
            UserId = wishlist.UserId,
            Items = items
        };
    }

    public async Task<WishlistResponse> AddItemAsync(Guid userId, Guid listingId, CancellationToken cancellationToken = default)
    {
        var listingExists = await _listingRepository.AnyAsync(l => l.Id == listingId, cancellationToken);
        if (!listingExists) throw new NotFoundException("Listing", listingId);

        var wishlists = await _wishlistRepository.QueryAsync(true, cancellationToken);
        var wishlist = await wishlists
            .Include(w => w.Items)
            .FirstOrDefaultAsync(w => w.UserId == userId, cancellationToken);

        if (wishlist is null)
        {
            wishlist = new Wishlist { UserId = userId };
            await _wishlistRepository.AddAsync(wishlist, cancellationToken);
        }

        var existingItem = await _itemRepository.AnyAsync(i => i.WishlistId == wishlist.Id && i.ListingId == listingId, cancellationToken);
        if (existingItem) throw new ConflictException("Listing already exists in wishlist.");

        var item = new WishlistItem
        {
            WishlistId = wishlist.Id,
            ListingId = listingId
        };

        await _itemRepository.AddAsync(item, cancellationToken);

        return await GetWishlistAsync(userId, cancellationToken);
    }

    public async Task<WishlistResponse> RemoveItemAsync(Guid userId, Guid listingId, CancellationToken cancellationToken = default)
    {
        var wishlists = await _wishlistRepository.QueryAsync(true, cancellationToken);
        var wishlist = await wishlists
            .Include(w => w.Items)
            .FirstOrDefaultAsync(w => w.UserId == userId, cancellationToken);

        if (wishlist is null)
            throw new NotFoundException("Wishlist", userId);

        var item = wishlist.Items
            .FirstOrDefault(i => i.ListingId == listingId);

        if (item is null)
            throw new NotFoundException("WishlistItem", listingId);

        await _itemRepository.DeleteAsync(item, cancellationToken);

        return await GetWishlistAsync(userId, cancellationToken);
    }
}
