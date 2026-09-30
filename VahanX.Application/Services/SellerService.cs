using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Marketplace;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of seller service.
/// </summary>
public class SellerService : ISellerService
{
    private readonly IRepository<Seller> _repository;
    private readonly IRepository<VehicleListing> _listingRepository;

    public SellerService(
        IRepository<Seller> repository,
        IRepository<VehicleListing> listingRepository)
    {
        _repository = repository;
        _listingRepository = listingRepository;
    }

    public async Task<PagedResult<SellerResponse>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new SellerResponse
            {
                Id = s.Id,
                UserId = s.UserId,
                DisplayName = s.DisplayName,
                Phone = s.Phone,
                Email = s.Email,
                ProfileImageUrl = s.ProfileImageUrl,
                Description = s.Description,
                LocationId = s.LocationId,
                LocationName = s.Location != null ? s.Location.Name : null,
                PreferredContactMethod = s.PreferredContactMethod,
                SellerType = s.SellerType,
                VerificationStatus = s.VerificationStatus,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<SellerResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<SellerResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var seller = await _repository.GetByIdAsync(id, cancellationToken);

        if (seller is null)
            return null;

        return new SellerResponse
        {
            Id = seller.Id,
            UserId = seller.UserId,
            DisplayName = seller.DisplayName,
            Phone = seller.Phone,
            Email = seller.Email,
            ProfileImageUrl = seller.ProfileImageUrl,
            Description = seller.Description,
            LocationId = seller.LocationId,
            LocationName = seller.Location != null ? seller.Location.Name : null,
            PreferredContactMethod = seller.PreferredContactMethod,
            SellerType = seller.SellerType,
            VerificationStatus = seller.VerificationStatus,
            IsActive = seller.IsActive,
            CreatedAt = seller.CreatedAt,
            UpdatedAt = seller.UpdatedAt
        };
    }

    public async Task<SellerResponse> CreateAsync(CreateSellerRequest request, CancellationToken cancellationToken = default)
    {
        var existingSeller = await _repository.AnyAsync(s => s.UserId == request.UserId, cancellationToken);
        if (existingSeller) throw new ConflictException("Seller already exists for this user.");

        var seller = new Seller
        {
            UserId = request.UserId,
            DisplayName = request.DisplayName,
            Phone = request.Phone,
            Email = request.Email,
            ProfileImageUrl = request.ProfileImageUrl,
            Description = request.Description,
            LocationId = request.LocationId,
            PreferredContactMethod = request.PreferredContactMethod,
            SellerType = request.SellerType
        };

        await _repository.AddAsync(seller, cancellationToken);

        return new SellerResponse
        {
            Id = seller.Id,
            UserId = seller.UserId,
            DisplayName = seller.DisplayName,
            Phone = seller.Phone,
            Email = seller.Email,
            ProfileImageUrl = seller.ProfileImageUrl,
            Description = seller.Description,
            LocationId = seller.LocationId,
            LocationName = null,
            PreferredContactMethod = seller.PreferredContactMethod,
            SellerType = seller.SellerType,
            VerificationStatus = seller.VerificationStatus,
            IsActive = seller.IsActive,
            CreatedAt = seller.CreatedAt,
            UpdatedAt = seller.UpdatedAt
        };
    }

    public async Task<SellerResponse> UpdateAsync(Guid id, UpdateSellerRequest request, CancellationToken cancellationToken = default)
    {
        var seller = await _repository.GetByIdAsync(id, cancellationToken);
        if (seller is null) throw new NotFoundException("Seller", id);

        seller.DisplayName = request.DisplayName;
        seller.Phone = request.Phone;
        seller.Email = request.Email;
        seller.ProfileImageUrl = request.ProfileImageUrl;
        seller.Description = request.Description;
        seller.LocationId = request.LocationId;
        seller.PreferredContactMethod = request.PreferredContactMethod;
        seller.SellerType = request.SellerType;
        seller.IsActive = request.IsActive;

        await _repository.UpdateAsync(seller, cancellationToken);

        return new SellerResponse
        {
            Id = seller.Id,
            UserId = seller.UserId,
            DisplayName = seller.DisplayName,
            Phone = seller.Phone,
            Email = seller.Email,
            ProfileImageUrl = seller.ProfileImageUrl,
            Description = seller.Description,
            LocationId = seller.LocationId,
            LocationName = null,
            PreferredContactMethod = seller.PreferredContactMethod,
            SellerType = seller.SellerType,
            VerificationStatus = seller.VerificationStatus,
            IsActive = seller.IsActive,
            CreatedAt = seller.CreatedAt,
            UpdatedAt = seller.UpdatedAt
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var seller = await _repository.GetByIdAsync(id, cancellationToken);
        if (seller is null) throw new NotFoundException("Seller", id);

        await _repository.DeleteAsync(seller, cancellationToken);
    }

    public async Task<PagedResult<ListingResponse>> GetSellerListingsAsync(Guid sellerId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _listingRepository.QueryAsync(true, cancellationToken);

        var listingsQuery = query
            .Where(l => l.SellerId == sellerId)
            .OrderByDescending(l => l.CreatedAt);

        var totalCount = await listingsQuery.CountAsync(cancellationToken);
        var items = await listingsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(l => new ListingResponse
            {
                Id = l.Id,
                VehicleId = l.VehicleId,
                VehicleName = string.Empty,
                SellerId = l.SellerId,
                SellerName = null,
                DealerId = l.DealerId,
                DealerName = null,
                Title = l.Title,
                Description = l.Description,
                Price = l.Price,
                Currency = l.Currency,
                Mileage = l.Mileage,
                MileageUnit = l.MileageUnit,
                ManufactureYear = l.ManufactureYear,
                RegistrationYear = l.RegistrationYear,
                Condition = l.Condition,
                Status = l.Status,
                LocationId = l.LocationId,
                LocationName = null,
                Latitude = l.Latitude,
                Longitude = l.Longitude,
                ContactPreference = l.ContactPreference,
                IsNegotiable = l.IsNegotiable,
                PublishedAt = l.PublishedAt,
                ExpiresAt = l.ExpiresAt,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<ListingResponse>.Create(items, totalCount, page, pageSize);
    }
}
