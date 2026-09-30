using Microsoft.EntityFrameworkCore;
using VahanX.Application.DTOs.Marketplace;
using VahanX.Application.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Infrastructure.Persistence;
using VahanX.Infrastructure.Persistence.Repositories;
using Xunit;

namespace VahanX.Tests.Unit.Marketplace;

/// <summary>
/// Unit tests for SearchService.
/// </summary>
public class SearchServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly SearchService _service;

    public SearchServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var listingRepository = new Repository<VehicleListing>(_context);

        _service = new SearchService(listingRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task SearchVehiclesAsync_WithPublishedListings_ReturnsOnlyPublished()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            VariantId = Guid.NewGuid(),
            BodyTypeId = Guid.NewGuid(),
            FuelTypeId = Guid.NewGuid(),
            TransmissionTypeId = Guid.NewGuid(),
            DriveTypeId = Guid.NewGuid(),
            EngineTypeId = Guid.NewGuid()
        };
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();

        var publishedListing = new VehicleListing
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Title = "Published Listing",
            Price = 1000000,
            Mileage = 50000,
            ManufactureYear = 2020,
            Status = ListingStatus.Published
        };

        var draftListing = new VehicleListing
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Title = "Draft Listing",
            Price = 2000000,
            Mileage = 60000,
            ManufactureYear = 2021,
            Status = ListingStatus.Draft
        };

        await _context.VehicleListings.AddRangeAsync(publishedListing, draftListing);
        await _context.SaveChangesAsync();

        var request = new SearchVehicleRequest { Page = 1, PageSize = 20 };

        // Act
        var result = await _service.SearchVehiclesAsync(request);

        // Assert
        Assert.Single(result.Items);
        Assert.Equal("Published Listing", result.Items[0].Title);
    }

    [Fact]
    public async Task SearchVehiclesAsync_WithPriceRange_FiltersCorrectly()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            VariantId = Guid.NewGuid(),
            BodyTypeId = Guid.NewGuid(),
            FuelTypeId = Guid.NewGuid(),
            TransmissionTypeId = Guid.NewGuid(),
            DriveTypeId = Guid.NewGuid(),
            EngineTypeId = Guid.NewGuid()
        };
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();

        var cheapListing = new VehicleListing
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Title = "Cheap Car",
            Price = 500000,
            Mileage = 50000,
            ManufactureYear = 2020,
            Status = ListingStatus.Published
        };

        var expensiveListing = new VehicleListing
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Title = "Expensive Car",
            Price = 5000000,
            Mileage = 30000,
            ManufactureYear = 2022,
            Status = ListingStatus.Published
        };

        await _context.VehicleListings.AddRangeAsync(cheapListing, expensiveListing);
        await _context.SaveChangesAsync();

        var request = new SearchVehicleRequest
        {
            MinPrice = 1000000,
            MaxPrice = 3000000,
            Page = 1,
            PageSize = 20
        };

        // Act
        var result = await _service.SearchVehiclesAsync(request);

        // Assert
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task SearchVehiclesAsync_WithSorting_SortsCorrectly()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            VariantId = Guid.NewGuid(),
            BodyTypeId = Guid.NewGuid(),
            FuelTypeId = Guid.NewGuid(),
            TransmissionTypeId = Guid.NewGuid(),
            DriveTypeId = Guid.NewGuid(),
            EngineTypeId = Guid.NewGuid()
        };
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();

        var listing1 = new VehicleListing
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Title = "Cheap Car",
            Price = 500000,
            Mileage = 50000,
            ManufactureYear = 2020,
            Status = ListingStatus.Published
        };

        var listing2 = new VehicleListing
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Title = "Expensive Car",
            Price = 5000000,
            Mileage = 30000,
            ManufactureYear = 2022,
            Status = ListingStatus.Published
        };

        await _context.VehicleListings.AddRangeAsync(listing1, listing2);
        await _context.SaveChangesAsync();

        var request = new SearchVehicleRequest
        {
            SortBy = SortOrder.PriceLowToHigh,
            Page = 1,
            PageSize = 20
        };

        // Act
        var result = await _service.SearchVehiclesAsync(request);

        // Assert
        Assert.Equal(2, result.Items.Count);
        Assert.Equal("Cheap Car", result.Items[0].Title);
        Assert.Equal("Expensive Car", result.Items[1].Title);
    }
}
