using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Marketplace;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of listing service.
/// </summary>
public class ListingService : IListingService
{
    private readonly IRepository<VehicleListing> _repository;
    private readonly IRepository<Vehicle> _vehicleRepository;
    private readonly IRepository<Seller> _sellerRepository;
    private readonly IRepository<Dealer> _dealerRepository;

    public ListingService(
        IRepository<VehicleListing> repository,
        IRepository<Vehicle> vehicleRepository,
        IRepository<Seller> sellerRepository,
        IRepository<Dealer> dealerRepository)
    {
        _repository = repository;
        _vehicleRepository = vehicleRepository;
        _sellerRepository = sellerRepository;
        _dealerRepository = dealerRepository;
    }

    public async Task<PagedResult<ListingResponse>> GetAllAsync(int page, int pageSize, Guid? vehicleId = null, Guid? sellerId = null, Guid? dealerId = null, ListingStatus? status = null, Guid? locationId = null, decimal? minPrice = null, decimal? maxPrice = null, int? minMileage = null, int? maxMileage = null, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        if (vehicleId.HasValue)
            query = query.Where(l => l.VehicleId == vehicleId.Value);

        if (sellerId.HasValue)
            query = query.Where(l => l.SellerId == sellerId.Value);

        if (dealerId.HasValue)
            query = query.Where(l => l.DealerId == dealerId.Value);

        if (status.HasValue)
            query = query.Where(l => l.Status == status.Value);

        if (locationId.HasValue)
            query = query.Where(l => l.LocationId == locationId.Value);

        if (minPrice.HasValue)
            query = query.Where(l => l.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(l => l.Price <= maxPrice.Value);

        if (minMileage.HasValue)
            query = query.Where(l => l.Mileage >= minMileage.Value);

        if (maxMileage.HasValue)
            query = query.Where(l => l.Mileage <= maxMileage.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(l => new ListingResponse
            {
                Id = l.Id,
                VehicleId = l.VehicleId,
                VehicleName = l.Vehicle != null && l.Vehicle.Variant != null ? l.Vehicle.Variant.Name : string.Empty,
                SellerId = l.SellerId,
                SellerName = l.Seller != null ? l.Seller.DisplayName : null,
                DealerId = l.DealerId,
                DealerName = l.Dealer != null ? l.Dealer.BusinessName : null,
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
                LocationName = l.Location != null ? l.Location.Name : null,
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

    public async Task<ListingResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var listing = await _repository.GetByIdAsync(id, cancellationToken);

        if (listing is null)
            return null;

        return new ListingResponse
        {
            Id = listing.Id,
            VehicleId = listing.VehicleId,
            VehicleName = listing.Vehicle != null && listing.Vehicle.Variant != null ? listing.Vehicle.Variant.Name : string.Empty,
            SellerId = listing.SellerId,
            SellerName = listing.Seller != null ? listing.Seller.DisplayName : null,
            DealerId = listing.DealerId,
            DealerName = listing.Dealer != null ? listing.Dealer.BusinessName : null,
            Title = listing.Title,
            Description = listing.Description,
            Price = listing.Price,
            Currency = listing.Currency,
            Mileage = listing.Mileage,
            MileageUnit = listing.MileageUnit,
            ManufactureYear = listing.ManufactureYear,
            RegistrationYear = listing.RegistrationYear,
            Condition = listing.Condition,
            Status = listing.Status,
            LocationId = listing.LocationId,
            LocationName = listing.Location != null ? listing.Location.Name : null,
            Latitude = listing.Latitude,
            Longitude = listing.Longitude,
            ContactPreference = listing.ContactPreference,
            IsNegotiable = listing.IsNegotiable,
            PublishedAt = listing.PublishedAt,
            ExpiresAt = listing.ExpiresAt,
            CreatedAt = listing.CreatedAt,
            UpdatedAt = listing.UpdatedAt
        };
    }

    public async Task<ListingResponse> CreateAsync(CreateListingRequest request, CancellationToken cancellationToken = default)
    {
        var vehicleExists = await _vehicleRepository.AnyAsync(v => v.Id == request.VehicleId, cancellationToken);
        if (!vehicleExists) throw new NotFoundException("Vehicle", request.VehicleId);

        if (request.SellerId.HasValue)
        {
            var sellerExists = await _sellerRepository.AnyAsync(s => s.Id == request.SellerId.Value, cancellationToken);
            if (!sellerExists) throw new NotFoundException("Seller", request.SellerId.Value);
        }

        if (request.DealerId.HasValue)
        {
            var dealerExists = await _dealerRepository.AnyAsync(d => d.Id == request.DealerId.Value, cancellationToken);
            if (!dealerExists) throw new NotFoundException("Dealer", request.DealerId.Value);
        }

        var listing = new VehicleListing
        {
            VehicleId = request.VehicleId,
            SellerId = request.SellerId,
            DealerId = request.DealerId,
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            Currency = request.Currency,
            Mileage = request.Mileage,
            MileageUnit = request.MileageUnit,
            ManufactureYear = request.ManufactureYear,
            RegistrationYear = request.RegistrationYear,
            Condition = request.Condition,
            Status = ListingStatus.Draft,
            LocationId = request.LocationId,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            ContactPreference = request.ContactPreference,
            IsNegotiable = request.IsNegotiable
        };

        await _repository.AddAsync(listing, cancellationToken);

        return new ListingResponse
        {
            Id = listing.Id,
            VehicleId = listing.VehicleId,
            VehicleName = string.Empty,
            SellerId = listing.SellerId,
            SellerName = null,
            DealerId = listing.DealerId,
            DealerName = null,
            Title = listing.Title,
            Description = listing.Description,
            Price = listing.Price,
            Currency = listing.Currency,
            Mileage = listing.Mileage,
            MileageUnit = listing.MileageUnit,
            ManufactureYear = listing.ManufactureYear,
            RegistrationYear = listing.RegistrationYear,
            Condition = listing.Condition,
            Status = listing.Status,
            LocationId = listing.LocationId,
            LocationName = null,
            Latitude = listing.Latitude,
            Longitude = listing.Longitude,
            ContactPreference = listing.ContactPreference,
            IsNegotiable = listing.IsNegotiable,
            PublishedAt = listing.PublishedAt,
            ExpiresAt = listing.ExpiresAt,
            CreatedAt = listing.CreatedAt,
            UpdatedAt = listing.UpdatedAt
        };
    }

    public async Task<ListingResponse> UpdateAsync(Guid id, UpdateListingRequest request, CancellationToken cancellationToken = default)
    {
        var listing = await _repository.GetByIdAsync(id, cancellationToken);
        if (listing is null) throw new NotFoundException("Listing", id);

        if (listing.Status == ListingStatus.Sold || listing.Status == ListingStatus.Archived)
            throw new ConflictException($"Cannot update a listing with status {listing.Status}.");

        listing.Title = request.Title;
        listing.Description = request.Description;
        listing.Price = request.Price;
        listing.Currency = request.Currency;
        listing.Mileage = request.Mileage;
        listing.MileageUnit = request.MileageUnit;
        listing.ManufactureYear = request.ManufactureYear;
        listing.RegistrationYear = request.RegistrationYear;
        listing.Condition = request.Condition;
        listing.LocationId = request.LocationId;
        listing.Latitude = request.Latitude;
        listing.Longitude = request.Longitude;
        listing.ContactPreference = request.ContactPreference;
        listing.IsNegotiable = request.IsNegotiable;

        await _repository.UpdateAsync(listing, cancellationToken);

        return new ListingResponse
        {
            Id = listing.Id,
            VehicleId = listing.VehicleId,
            VehicleName = string.Empty,
            SellerId = listing.SellerId,
            SellerName = null,
            DealerId = listing.DealerId,
            DealerName = null,
            Title = listing.Title,
            Description = listing.Description,
            Price = listing.Price,
            Currency = listing.Currency,
            Mileage = listing.Mileage,
            MileageUnit = listing.MileageUnit,
            ManufactureYear = listing.ManufactureYear,
            RegistrationYear = listing.RegistrationYear,
            Condition = listing.Condition,
            Status = listing.Status,
            LocationId = listing.LocationId,
            LocationName = null,
            Latitude = listing.Latitude,
            Longitude = listing.Longitude,
            ContactPreference = listing.ContactPreference,
            IsNegotiable = listing.IsNegotiable,
            PublishedAt = listing.PublishedAt,
            ExpiresAt = listing.ExpiresAt,
            CreatedAt = listing.CreatedAt,
            UpdatedAt = listing.UpdatedAt
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var listing = await _repository.GetByIdAsync(id, cancellationToken);
        if (listing is null) throw new NotFoundException("Listing", id);

        await _repository.DeleteAsync(listing, cancellationToken);
    }

    public async Task<ListingResponse> SubmitAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var listing = await _repository.GetByIdAsync(id, cancellationToken);
        if (listing is null) throw new NotFoundException("Listing", id);

        if (listing.Status != ListingStatus.Draft && listing.Status != ListingStatus.Rejected)
            throw new ConflictException($"Cannot submit a listing with status {listing.Status}.");

        listing.Status = ListingStatus.PendingReview;
        await _repository.UpdateAsync(listing, cancellationToken);

        return new ListingResponse
        {
            Id = listing.Id,
            VehicleId = listing.VehicleId,
            VehicleName = string.Empty,
            SellerId = listing.SellerId,
            SellerName = null,
            DealerId = listing.DealerId,
            DealerName = null,
            Title = listing.Title,
            Description = listing.Description,
            Price = listing.Price,
            Currency = listing.Currency,
            Mileage = listing.Mileage,
            MileageUnit = listing.MileageUnit,
            ManufactureYear = listing.ManufactureYear,
            RegistrationYear = listing.RegistrationYear,
            Condition = listing.Condition,
            Status = listing.Status,
            LocationId = listing.LocationId,
            LocationName = null,
            Latitude = listing.Latitude,
            Longitude = listing.Longitude,
            ContactPreference = listing.ContactPreference,
            IsNegotiable = listing.IsNegotiable,
            PublishedAt = listing.PublishedAt,
            ExpiresAt = listing.ExpiresAt,
            CreatedAt = listing.CreatedAt,
            UpdatedAt = listing.UpdatedAt
        };
    }

    public async Task<ListingResponse> PublishAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var listing = await _repository.GetByIdAsync(id, cancellationToken);
        if (listing is null) throw new NotFoundException("Listing", id);

        if (listing.Status != ListingStatus.PendingReview && listing.Status != ListingStatus.Draft)
            throw new ConflictException($"Cannot publish a listing with status {listing.Status}.");

        listing.Status = ListingStatus.Published;
        listing.PublishedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(listing, cancellationToken);

        return new ListingResponse
        {
            Id = listing.Id,
            VehicleId = listing.VehicleId,
            VehicleName = string.Empty,
            SellerId = listing.SellerId,
            SellerName = null,
            DealerId = listing.DealerId,
            DealerName = null,
            Title = listing.Title,
            Description = listing.Description,
            Price = listing.Price,
            Currency = listing.Currency,
            Mileage = listing.Mileage,
            MileageUnit = listing.MileageUnit,
            ManufactureYear = listing.ManufactureYear,
            RegistrationYear = listing.RegistrationYear,
            Condition = listing.Condition,
            Status = listing.Status,
            LocationId = listing.LocationId,
            LocationName = null,
            Latitude = listing.Latitude,
            Longitude = listing.Longitude,
            ContactPreference = listing.ContactPreference,
            IsNegotiable = listing.IsNegotiable,
            PublishedAt = listing.PublishedAt,
            ExpiresAt = listing.ExpiresAt,
            CreatedAt = listing.CreatedAt,
            UpdatedAt = listing.UpdatedAt
        };
    }

    public async Task<ListingResponse> PauseAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var listing = await _repository.GetByIdAsync(id, cancellationToken);
        if (listing is null) throw new NotFoundException("Listing", id);

        if (listing.Status != ListingStatus.Published)
            throw new ConflictException($"Cannot pause a listing with status {listing.Status}.");

        listing.Status = ListingStatus.Paused;
        await _repository.UpdateAsync(listing, cancellationToken);

        return new ListingResponse
        {
            Id = listing.Id,
            VehicleId = listing.VehicleId,
            VehicleName = string.Empty,
            SellerId = listing.SellerId,
            SellerName = null,
            DealerId = listing.DealerId,
            DealerName = null,
            Title = listing.Title,
            Description = listing.Description,
            Price = listing.Price,
            Currency = listing.Currency,
            Mileage = listing.Mileage,
            MileageUnit = listing.MileageUnit,
            ManufactureYear = listing.ManufactureYear,
            RegistrationYear = listing.RegistrationYear,
            Condition = listing.Condition,
            Status = listing.Status,
            LocationId = listing.LocationId,
            LocationName = null,
            Latitude = listing.Latitude,
            Longitude = listing.Longitude,
            ContactPreference = listing.ContactPreference,
            IsNegotiable = listing.IsNegotiable,
            PublishedAt = listing.PublishedAt,
            ExpiresAt = listing.ExpiresAt,
            CreatedAt = listing.CreatedAt,
            UpdatedAt = listing.UpdatedAt
        };
    }

    public async Task<ListingResponse> MarkSoldAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var listing = await _repository.GetByIdAsync(id, cancellationToken);
        if (listing is null) throw new NotFoundException("Listing", id);

        if (listing.Status != ListingStatus.Published && listing.Status != ListingStatus.Paused)
            throw new ConflictException($"Cannot mark as sold a listing with status {listing.Status}.");

        listing.Status = ListingStatus.Sold;
        await _repository.UpdateAsync(listing, cancellationToken);

        return new ListingResponse
        {
            Id = listing.Id,
            VehicleId = listing.VehicleId,
            VehicleName = string.Empty,
            SellerId = listing.SellerId,
            SellerName = null,
            DealerId = listing.DealerId,
            DealerName = null,
            Title = listing.Title,
            Description = listing.Description,
            Price = listing.Price,
            Currency = listing.Currency,
            Mileage = listing.Mileage,
            MileageUnit = listing.MileageUnit,
            ManufactureYear = listing.ManufactureYear,
            RegistrationYear = listing.RegistrationYear,
            Condition = listing.Condition,
            Status = listing.Status,
            LocationId = listing.LocationId,
            LocationName = null,
            Latitude = listing.Latitude,
            Longitude = listing.Longitude,
            ContactPreference = listing.ContactPreference,
            IsNegotiable = listing.IsNegotiable,
            PublishedAt = listing.PublishedAt,
            ExpiresAt = listing.ExpiresAt,
            CreatedAt = listing.CreatedAt,
            UpdatedAt = listing.UpdatedAt
        };
    }

    public async Task<ListingResponse> ArchiveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var listing = await _repository.GetByIdAsync(id, cancellationToken);
        if (listing is null) throw new NotFoundException("Listing", id);

        if (listing.Status == ListingStatus.Sold || listing.Status == ListingStatus.Archived)
            throw new ConflictException($"Cannot archive a listing with status {listing.Status}.");

        listing.Status = ListingStatus.Archived;
        await _repository.UpdateAsync(listing, cancellationToken);

        return new ListingResponse
        {
            Id = listing.Id,
            VehicleId = listing.VehicleId,
            VehicleName = string.Empty,
            SellerId = listing.SellerId,
            SellerName = null,
            DealerId = listing.DealerId,
            DealerName = null,
            Title = listing.Title,
            Description = listing.Description,
            Price = listing.Price,
            Currency = listing.Currency,
            Mileage = listing.Mileage,
            MileageUnit = listing.MileageUnit,
            ManufactureYear = listing.ManufactureYear,
            RegistrationYear = listing.RegistrationYear,
            Condition = listing.Condition,
            Status = listing.Status,
            LocationId = listing.LocationId,
            LocationName = null,
            Latitude = listing.Latitude,
            Longitude = listing.Longitude,
            ContactPreference = listing.ContactPreference,
            IsNegotiable = listing.IsNegotiable,
            PublishedAt = listing.PublishedAt,
            ExpiresAt = listing.ExpiresAt,
            CreatedAt = listing.CreatedAt,
            UpdatedAt = listing.UpdatedAt
        };
    }
}
