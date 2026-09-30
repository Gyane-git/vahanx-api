using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Marketplace;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of listing media service.
/// </summary>
public class ListingMediaService : IListingMediaService
{
    private readonly IRepository<ListingMedia> _repository;
    private readonly IRepository<VehicleListing> _listingRepository;

    public ListingMediaService(
        IRepository<ListingMedia> repository,
        IRepository<VehicleListing> listingRepository)
    {
        _repository = repository;
        _listingRepository = listingRepository;
    }

    public async Task<ListingMediaResponse> CreateAsync(Guid listingId, CreateListingMediaRequest request, CancellationToken cancellationToken = default)
    {
        var listingExists = await _listingRepository.AnyAsync(l => l.Id == listingId, cancellationToken);
        if (!listingExists) throw new NotFoundException("Listing", listingId);

        if (request.IsPrimary)
        {
            var mediaList = await _repository.QueryAsync(true, cancellationToken);
            var existingPrimary = await mediaList
                .Where(m => m.ListingId == listingId && m.IsPrimary)
                .ToListAsync(cancellationToken);

            foreach (var media in existingPrimary)
            {
                media.IsPrimary = false;
                await _repository.UpdateAsync(media, cancellationToken);
            }
        }

        var mediaItem = new ListingMedia
        {
            ListingId = listingId,
            MediaUrl = request.MediaUrl,
            MediaType = request.MediaType,
            DisplayOrder = request.DisplayOrder,
            IsPrimary = request.IsPrimary,
            Caption = request.Caption
        };

        await _repository.AddAsync(mediaItem, cancellationToken);

        return new ListingMediaResponse
        {
            Id = mediaItem.Id,
            ListingId = mediaItem.ListingId,
            MediaUrl = mediaItem.MediaUrl,
            MediaType = mediaItem.MediaType,
            DisplayOrder = mediaItem.DisplayOrder,
            IsPrimary = mediaItem.IsPrimary,
            Caption = mediaItem.Caption,
            CreatedAt = mediaItem.CreatedAt
        };
    }

    public async Task<ListingMediaResponse> UpdateAsync(Guid listingId, Guid mediaId, UpdateListingMediaRequest request, CancellationToken cancellationToken = default)
    {
        var media = await _repository.GetByIdAsync(mediaId, cancellationToken);
        if (media is null) throw new NotFoundException("ListingMedia", mediaId);

        if (media.ListingId != listingId)
            throw new NotFoundException("ListingMedia", mediaId);

        if (request.MediaUrl != null) media.MediaUrl = request.MediaUrl;
        if (request.MediaType.HasValue) media.MediaType = request.MediaType.Value;
        if (request.DisplayOrder.HasValue) media.DisplayOrder = request.DisplayOrder.Value;
        if (request.Caption != null) media.Caption = request.Caption;

        await _repository.UpdateAsync(media, cancellationToken);

        return new ListingMediaResponse
        {
            Id = media.Id,
            ListingId = media.ListingId,
            MediaUrl = media.MediaUrl,
            MediaType = media.MediaType,
            DisplayOrder = media.DisplayOrder,
            IsPrimary = media.IsPrimary,
            Caption = media.Caption,
            CreatedAt = media.CreatedAt
        };
    }

    public async Task DeleteAsync(Guid listingId, Guid mediaId, CancellationToken cancellationToken = default)
    {
        var media = await _repository.GetByIdAsync(mediaId, cancellationToken);
        if (media is null) throw new NotFoundException("ListingMedia", mediaId);

        if (media.ListingId != listingId)
            throw new NotFoundException("ListingMedia", mediaId);

        await _repository.DeleteAsync(media, cancellationToken);
    }

    public async Task<ListingMediaResponse> SetPrimaryAsync(Guid listingId, Guid mediaId, CancellationToken cancellationToken = default)
    {
        var media = await _repository.GetByIdAsync(mediaId, cancellationToken);
        if (media is null) throw new NotFoundException("ListingMedia", mediaId);

        if (media.ListingId != listingId)
            throw new NotFoundException("ListingMedia", mediaId);

        var mediaList = await _repository.QueryAsync(true, cancellationToken);
        var existingPrimary = await mediaList
            .Where(m => m.ListingId == listingId && m.IsPrimary && m.Id != mediaId)
            .ToListAsync(cancellationToken);

        foreach (var item in existingPrimary)
        {
            item.IsPrimary = false;
            await _repository.UpdateAsync(item, cancellationToken);
        }

        media.IsPrimary = true;
        await _repository.UpdateAsync(media, cancellationToken);

        return new ListingMediaResponse
        {
            Id = media.Id,
            ListingId = media.ListingId,
            MediaUrl = media.MediaUrl,
            MediaType = media.MediaType,
            DisplayOrder = media.DisplayOrder,
            IsPrimary = media.IsPrimary,
            Caption = media.Caption,
            CreatedAt = media.CreatedAt
        };
    }

    public async Task<PagedResult<ListingMediaResponse>> GetByListingAsync(Guid listingId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        var mediaQuery = query
            .Where(m => m.ListingId == listingId)
            .OrderBy(m => m.DisplayOrder);

        var totalCount = await mediaQuery.CountAsync(cancellationToken);
        var items = await mediaQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new ListingMediaResponse
            {
                Id = m.Id,
                ListingId = m.ListingId,
                MediaUrl = m.MediaUrl,
                MediaType = m.MediaType,
                DisplayOrder = m.DisplayOrder,
                IsPrimary = m.IsPrimary,
                Caption = m.Caption,
                CreatedAt = m.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<ListingMediaResponse>.Create(items, totalCount, page, pageSize);
    }
}
