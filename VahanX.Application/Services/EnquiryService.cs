using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Engagement;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of enquiry service.
/// </summary>
public class EnquiryService : IEnquiryService
{
    private readonly IRepository<Enquiry> _repository;
    private readonly IRepository<VehicleListing> _listingRepository;

    public EnquiryService(
        IRepository<Enquiry> repository,
        IRepository<VehicleListing> listingRepository)
    {
        _repository = repository;
        _listingRepository = listingRepository;
    }

    public async Task<PagedResult<EnquiryResponse>> GetAllAsync(int page, int pageSize, Guid? userId = null, Guid? listingId = null, Guid? sellerId = null, Guid? dealerId = null, EnquiryStatus? status = null, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        if (userId.HasValue)
            query = query.Where(e => e.UserId == userId.Value);
        if (listingId.HasValue)
            query = query.Where(e => e.ListingId == listingId.Value);
        if (sellerId.HasValue)
            query = query.Where(e => e.SellerId == sellerId.Value);
        if (dealerId.HasValue)
            query = query.Where(e => e.DealerId == dealerId.Value);
        if (status.HasValue)
            query = query.Where(e => e.Status == status.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(e => e.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new EnquiryResponse
            {
                Id = e.Id,
                UserId = e.UserId,
                ListingId = e.ListingId,
                ListingTitle = e.Listing != null ? e.Listing.Title : string.Empty,
                SellerId = e.SellerId,
                SellerName = e.Seller != null ? e.Seller.DisplayName : null,
                DealerId = e.DealerId,
                DealerName = e.Dealer != null ? e.Dealer.BusinessName : null,
                EnquiryType = e.EnquiryType,
                Subject = e.Subject,
                Message = e.Message,
                Status = e.Status,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
                ClosedAt = e.ClosedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<EnquiryResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<EnquiryResponse?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var enquiry = await _repository.GetByIdAsync(id, cancellationToken);
        if (enquiry is null) return null;

        if (enquiry.UserId != userId && enquiry.SellerId == null && enquiry.DealerId == null)
            throw new ForbiddenException("You do not have access to this enquiry.");

        return new EnquiryResponse
        {
            Id = enquiry.Id,
            UserId = enquiry.UserId,
            ListingId = enquiry.ListingId,
            ListingTitle = enquiry.Listing != null ? enquiry.Listing.Title : string.Empty,
            SellerId = enquiry.SellerId,
            SellerName = enquiry.Seller != null ? enquiry.Seller.DisplayName : null,
            DealerId = enquiry.DealerId,
            DealerName = enquiry.Dealer != null ? enquiry.Dealer.BusinessName : null,
            EnquiryType = enquiry.EnquiryType,
            Subject = enquiry.Subject,
            Message = enquiry.Message,
            Status = enquiry.Status,
            CreatedAt = enquiry.CreatedAt,
            UpdatedAt = enquiry.UpdatedAt,
            ClosedAt = enquiry.ClosedAt
        };
    }

    public async Task<EnquiryResponse> CreateAsync(CreateEnquiryRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing is null) throw new NotFoundException("Listing", request.ListingId);

        if (listing.Status != ListingStatus.Published)
            throw new ConflictException("Can only enquire about published listings.");

        var enquiry = new Enquiry
        {
            UserId = userId,
            ListingId = request.ListingId,
            SellerId = listing.SellerId,
            DealerId = listing.DealerId,
            EnquiryType = request.EnquiryType,
            Subject = request.Subject,
            Message = request.Message,
            Status = EnquiryStatus.New
        };

        await _repository.AddAsync(enquiry, cancellationToken);

        return new EnquiryResponse
        {
            Id = enquiry.Id,
            UserId = enquiry.UserId,
            ListingId = enquiry.ListingId,
            ListingTitle = listing.Title,
            SellerId = enquiry.SellerId,
            SellerName = null,
            DealerId = enquiry.DealerId,
            DealerName = null,
            EnquiryType = enquiry.EnquiryType,
            Subject = enquiry.Subject,
            Message = enquiry.Message,
            Status = enquiry.Status,
            CreatedAt = enquiry.CreatedAt,
            UpdatedAt = enquiry.UpdatedAt,
            ClosedAt = enquiry.ClosedAt
        };
    }

    public async Task<EnquiryResponse> UpdateAsync(Guid id, UpdateEnquiryRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var enquiry = await _repository.GetByIdAsync(id, cancellationToken);
        if (enquiry is null) throw new NotFoundException("Enquiry", id);

        if (enquiry.UserId != userId)
            throw new ForbiddenException("You can only update your own enquiries.");

        if (enquiry.Status == EnquiryStatus.Closed || enquiry.Status == EnquiryStatus.Cancelled)
            throw new ConflictException($"Cannot update an enquiry with status {enquiry.Status}.");

        if (request.Subject != null) enquiry.Subject = request.Subject;
        if (request.Message != null) enquiry.Message = request.Message;

        await _repository.UpdateAsync(enquiry, cancellationToken);

        return new EnquiryResponse
        {
            Id = enquiry.Id,
            UserId = enquiry.UserId,
            ListingId = enquiry.ListingId,
            ListingTitle = string.Empty,
            SellerId = enquiry.SellerId,
            SellerName = null,
            DealerId = enquiry.DealerId,
            DealerName = null,
            EnquiryType = enquiry.EnquiryType,
            Subject = enquiry.Subject,
            Message = enquiry.Message,
            Status = enquiry.Status,
            CreatedAt = enquiry.CreatedAt,
            UpdatedAt = enquiry.UpdatedAt,
            ClosedAt = enquiry.ClosedAt
        };
    }

    public async Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var enquiry = await _repository.GetByIdAsync(id, cancellationToken);
        if (enquiry is null) throw new NotFoundException("Enquiry", id);

        if (enquiry.UserId != userId)
            throw new ForbiddenException("You can only delete your own enquiries.");

        await _repository.DeleteAsync(enquiry, cancellationToken);
    }

    public async Task<EnquiryResponse> RespondAsync(Guid id, RespondEnquiryRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var enquiry = await _repository.GetByIdAsync(id, cancellationToken);
        if (enquiry is null) throw new NotFoundException("Enquiry", id);

        if (enquiry.SellerId == null && enquiry.DealerId == null)
            throw new ForbiddenException("Only sellers or dealers can respond to enquiries.");

        if (enquiry.Status == EnquiryStatus.Closed || enquiry.Status == EnquiryStatus.Cancelled)
            throw new ConflictException($"Cannot respond to an enquiry with status {enquiry.Status}.");

        enquiry.Status = EnquiryStatus.Responded;
        await _repository.UpdateAsync(enquiry, cancellationToken);

        return new EnquiryResponse
        {
            Id = enquiry.Id,
            UserId = enquiry.UserId,
            ListingId = enquiry.ListingId,
            ListingTitle = string.Empty,
            SellerId = enquiry.SellerId,
            SellerName = null,
            DealerId = enquiry.DealerId,
            DealerName = null,
            EnquiryType = enquiry.EnquiryType,
            Subject = enquiry.Subject,
            Message = enquiry.Message,
            Status = enquiry.Status,
            CreatedAt = enquiry.CreatedAt,
            UpdatedAt = enquiry.UpdatedAt,
            ClosedAt = enquiry.ClosedAt
        };
    }

    public async Task<EnquiryResponse> CloseAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var enquiry = await _repository.GetByIdAsync(id, cancellationToken);
        if (enquiry is null) throw new NotFoundException("Enquiry", id);

        if (enquiry.UserId != userId)
            throw new ForbiddenException("You can only close your own enquiries.");

        if (enquiry.Status == EnquiryStatus.Closed || enquiry.Status == EnquiryStatus.Cancelled)
            throw new ConflictException($"Cannot close an enquiry with status {enquiry.Status}.");

        enquiry.Status = EnquiryStatus.Closed;
        enquiry.ClosedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(enquiry, cancellationToken);

        return new EnquiryResponse
        {
            Id = enquiry.Id,
            UserId = enquiry.UserId,
            ListingId = enquiry.ListingId,
            ListingTitle = string.Empty,
            SellerId = enquiry.SellerId,
            SellerName = null,
            DealerId = enquiry.DealerId,
            DealerName = null,
            EnquiryType = enquiry.EnquiryType,
            Subject = enquiry.Subject,
            Message = enquiry.Message,
            Status = enquiry.Status,
            CreatedAt = enquiry.CreatedAt,
            UpdatedAt = enquiry.UpdatedAt,
            ClosedAt = enquiry.ClosedAt
        };
    }

    public async Task<EnquiryResponse> CancelAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var enquiry = await _repository.GetByIdAsync(id, cancellationToken);
        if (enquiry is null) throw new NotFoundException("Enquiry", id);

        if (enquiry.UserId != userId)
            throw new ForbiddenException("You can only cancel your own enquiries.");

        if (enquiry.Status == EnquiryStatus.Closed || enquiry.Status == EnquiryStatus.Cancelled)
            throw new ConflictException($"Cannot cancel an enquiry with status {enquiry.Status}.");

        enquiry.Status = EnquiryStatus.Cancelled;
        enquiry.ClosedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(enquiry, cancellationToken);

        return new EnquiryResponse
        {
            Id = enquiry.Id,
            UserId = enquiry.UserId,
            ListingId = enquiry.ListingId,
            ListingTitle = string.Empty,
            SellerId = enquiry.SellerId,
            SellerName = null,
            DealerId = enquiry.DealerId,
            DealerName = null,
            EnquiryType = enquiry.EnquiryType,
            Subject = enquiry.Subject,
            Message = enquiry.Message,
            Status = enquiry.Status,
            CreatedAt = enquiry.CreatedAt,
            UpdatedAt = enquiry.UpdatedAt,
            ClosedAt = enquiry.ClosedAt
        };
    }

    public async Task<PagedResult<EnquiryResponse>> GetByListingAsync(Guid listingId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(e => e.ListingId == listingId).OrderByDescending(e => e.CreatedAt);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new EnquiryResponse
            {
                Id = e.Id,
                UserId = e.UserId,
                ListingId = e.ListingId,
                ListingTitle = string.Empty,
                SellerId = e.SellerId,
                SellerName = null,
                DealerId = e.DealerId,
                DealerName = null,
                EnquiryType = e.EnquiryType,
                Subject = e.Subject,
                Message = e.Message,
                Status = e.Status,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
                ClosedAt = e.ClosedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<EnquiryResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<EnquiryResponse>> GetBySellerAsync(Guid sellerId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(e => e.SellerId == sellerId).OrderByDescending(e => e.CreatedAt);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new EnquiryResponse
            {
                Id = e.Id,
                UserId = e.UserId,
                ListingId = e.ListingId,
                ListingTitle = string.Empty,
                SellerId = e.SellerId,
                SellerName = null,
                DealerId = e.DealerId,
                DealerName = null,
                EnquiryType = e.EnquiryType,
                Subject = e.Subject,
                Message = e.Message,
                Status = e.Status,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
                ClosedAt = e.ClosedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<EnquiryResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<EnquiryResponse>> GetByDealerAsync(Guid dealerId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(e => e.DealerId == dealerId).OrderByDescending(e => e.CreatedAt);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new EnquiryResponse
            {
                Id = e.Id,
                UserId = e.UserId,
                ListingId = e.ListingId,
                ListingTitle = string.Empty,
                SellerId = e.SellerId,
                SellerName = null,
                DealerId = e.DealerId,
                DealerName = null,
                EnquiryType = e.EnquiryType,
                Subject = e.Subject,
                Message = e.Message,
                Status = e.Status,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
                ClosedAt = e.ClosedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<EnquiryResponse>.Create(items, totalCount, page, pageSize);
    }
}
