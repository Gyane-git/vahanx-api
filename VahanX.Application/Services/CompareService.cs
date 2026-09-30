using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Marketplace;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of compare service.
/// </summary>
public class CompareService : ICompareService
{
    private const int MaxCompareItems = 4;

    private readonly IRepository<CompareList> _compareListRepository;
    private readonly IRepository<CompareItem> _itemRepository;
    private readonly IRepository<VehicleListing> _listingRepository;

    public CompareService(
        IRepository<CompareList> compareListRepository,
        IRepository<CompareItem> itemRepository,
        IRepository<VehicleListing> listingRepository)
    {
        _compareListRepository = compareListRepository;
        _itemRepository = itemRepository;
        _listingRepository = listingRepository;
    }

    public async Task<CompareResponse> GetCompareListAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var compareLists = await _compareListRepository.QueryAsync(true, cancellationToken);
        var compareList = await compareLists
            .Include(c => c.Items)
            .ThenInclude(i => i.Listing)
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

        if (compareList is null)
        {
            return new CompareResponse
            {
                Id = Guid.Empty,
                UserId = userId,
                Items = []
            };
        }

        var items = compareList.Items
            .Where(i => i.Listing != null)
            .Select(i => new CompareItemResponse
            {
                Id = i.Id,
                ListingId = i.ListingId,
                ListingTitle = i.Listing!.Title,
                VehicleName = i.Listing.Vehicle != null && i.Listing.Vehicle.Variant != null ? i.Listing.Vehicle.Variant.Name : string.Empty,
                Price = i.Listing.Price,
                Currency = i.Listing.Currency,
                Mileage = i.Listing.Mileage,
                MileageUnit = i.Listing.MileageUnit,
                ManufactureYear = i.Listing.ManufactureYear,
                PrimaryImageUrl = i.Listing.Media != null ? i.Listing.Media.Where(m => m.IsPrimary).Select(m => m.MediaUrl).FirstOrDefault() : null,
                AddedAt = i.CreatedAt
            })
            .ToList();

        return new CompareResponse
        {
            Id = compareList.Id,
            UserId = compareList.UserId,
            Items = items
        };
    }

    public async Task<CompareResponse> AddItemAsync(Guid userId, Guid listingId, CancellationToken cancellationToken = default)
    {
        var listingExists = await _listingRepository.AnyAsync(l => l.Id == listingId, cancellationToken);
        if (!listingExists) throw new NotFoundException("Listing", listingId);

        var compareLists = await _compareListRepository.QueryAsync(true, cancellationToken);
        var compareList = await compareLists
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

        if (compareList is null)
        {
            compareList = new CompareList { UserId = userId };
            await _compareListRepository.AddAsync(compareList, cancellationToken);
        }

        if (compareList.Items.Count >= MaxCompareItems)
            throw new ConflictException($"Maximum of {MaxCompareItems} items allowed in compare list.");

        var existingItem = await _itemRepository.AnyAsync(i => i.CompareListId == compareList.Id && i.ListingId == listingId, cancellationToken);
        if (existingItem) throw new ConflictException("Listing already exists in compare list.");

        var item = new CompareItem
        {
            CompareListId = compareList.Id,
            ListingId = listingId
        };

        await _itemRepository.AddAsync(item, cancellationToken);

        return await GetCompareListAsync(userId, cancellationToken);
    }

    public async Task<CompareResponse> RemoveItemAsync(Guid userId, Guid listingId, CancellationToken cancellationToken = default)
    {
        var compareLists = await _compareListRepository.QueryAsync(true, cancellationToken);
        var compareList = await compareLists
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

        if (compareList is null)
            throw new NotFoundException("CompareList", userId);

        var items = await _itemRepository.QueryAsync(true, cancellationToken);
        var item = await items
            .FirstOrDefaultAsync(i => i.CompareListId == compareList.Id && i.ListingId == listingId, cancellationToken);

        if (item is null)
            throw new NotFoundException("CompareItem", listingId);

        await _itemRepository.DeleteAsync(item, cancellationToken);

        return await GetCompareListAsync(userId, cancellationToken);
    }
}
