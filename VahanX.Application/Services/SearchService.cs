using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Marketplace;
using VahanX.Domain.Enums;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of search service.
/// </summary>
public class SearchService : ISearchService
{
    private readonly IRepository<Domain.Entities.VehicleListing> _listingRepository;

    public SearchService(IRepository<Domain.Entities.VehicleListing> listingRepository)
    {
        _listingRepository = listingRepository;
    }

    public async Task<PagedResult<SearchVehicleResponse>> SearchVehiclesAsync(SearchVehicleRequest request, CancellationToken cancellationToken = default)
    {
        var query = await _listingRepository.QueryAsync(true, cancellationToken);

        query = query.Where(l => l.Status == ListingStatus.Published);

        if (!string.IsNullOrEmpty(request.Keyword))
        {
            var keyword = request.Keyword.ToLower();
            query = query.Where(l => l.Title.ToLower().Contains(keyword) ||
                                     (l.Description != null && l.Description.ToLower().Contains(keyword)));
        }

        if (request.BrandId.HasValue)
            query = query.Where(l => l.Vehicle != null && l.Vehicle.Variant != null && l.Vehicle.Variant.Generation != null && l.Vehicle.Variant.Generation.Model != null && l.Vehicle.Variant.Generation.Model.BrandId == request.BrandId.Value);

        if (request.ModelId.HasValue)
            query = query.Where(l => l.Vehicle != null && l.Vehicle.Variant != null && l.Vehicle.Variant.Generation != null && l.Vehicle.Variant.Generation.ModelId == request.ModelId.Value);

        if (request.VariantId.HasValue)
            query = query.Where(l => l.Vehicle != null && l.Vehicle.VariantId == request.VariantId.Value);

        if (request.FuelTypeId.HasValue)
            query = query.Where(l => l.Vehicle != null && l.Vehicle.FuelTypeId == request.FuelTypeId.Value);

        if (request.TransmissionTypeId.HasValue)
            query = query.Where(l => l.Vehicle != null && l.Vehicle.TransmissionTypeId == request.TransmissionTypeId.Value);

        if (request.BodyTypeId.HasValue)
            query = query.Where(l => l.Vehicle != null && l.Vehicle.BodyTypeId == request.BodyTypeId.Value);

        if (request.LocationId.HasValue)
            query = query.Where(l => l.LocationId == request.LocationId.Value);

        if (request.SellerId.HasValue)
            query = query.Where(l => l.SellerId == request.SellerId.Value);

        if (request.DealerId.HasValue)
            query = query.Where(l => l.DealerId == request.DealerId.Value);

        if (request.MinPrice.HasValue)
            query = query.Where(l => l.Price >= request.MinPrice.Value);

        if (request.MaxPrice.HasValue)
            query = query.Where(l => l.Price <= request.MaxPrice.Value);

        if (request.MinMileage.HasValue)
            query = query.Where(l => l.Mileage >= request.MinMileage.Value);

        if (request.MaxMileage.HasValue)
            query = query.Where(l => l.Mileage <= request.MaxMileage.Value);

        if (request.ManufactureYear.HasValue)
            query = query.Where(l => l.ManufactureYear == request.ManufactureYear.Value);

        query = request.SortBy switch
        {
            SortOrder.PriceLowToHigh => query.OrderBy(l => l.Price),
            SortOrder.PriceHighToLow => query.OrderByDescending(l => l.Price),
            SortOrder.MileageLowToHigh => query.OrderBy(l => l.Mileage),
            SortOrder.YearNewest => query.OrderByDescending(l => l.ManufactureYear),
            _ => query.OrderByDescending(l => l.CreatedAt)
        };

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(l => new SearchVehicleResponse
            {
                ListingId = l.Id,
                Title = l.Title,
                VehicleName = l.Vehicle != null && l.Vehicle.Variant != null ? l.Vehicle.Variant.Name : string.Empty,
                BrandName = l.Vehicle != null && l.Vehicle.Variant != null && l.Vehicle.Variant.Generation != null && l.Vehicle.Variant.Generation.Model != null && l.Vehicle.Variant.Generation.Model.Brand != null ? l.Vehicle.Variant.Generation.Model.Brand.Name : string.Empty,
                ModelName = l.Vehicle != null && l.Vehicle.Variant != null && l.Vehicle.Variant.Generation != null && l.Vehicle.Variant.Generation.Model != null ? l.Vehicle.Variant.Generation.Model.Name : string.Empty,
                VariantName = l.Vehicle != null && l.Vehicle.Variant != null ? l.Vehicle.Variant.Name : null,
                Price = l.Price,
                Currency = l.Currency,
                Mileage = l.Mileage,
                MileageUnit = l.MileageUnit,
                ManufactureYear = l.ManufactureYear,
                Condition = l.Condition,
                LocationName = l.Location != null ? l.Location.Name : null,
                PrimaryImageUrl = l.Media != null ? l.Media.Where(m => m.IsPrimary).Select(m => m.MediaUrl).FirstOrDefault() : null,
                PublishedAt = l.PublishedAt ?? l.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<SearchVehicleResponse>.Create(items, totalCount, request.Page, request.PageSize);
    }
}
