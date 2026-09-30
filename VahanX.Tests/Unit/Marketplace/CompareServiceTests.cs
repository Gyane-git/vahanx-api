using Microsoft.EntityFrameworkCore;
using VahanX.Application.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;
using VahanX.Infrastructure.Persistence;
using VahanX.Infrastructure.Persistence.Repositories;
using Xunit;

namespace VahanX.Tests.Unit.Marketplace;

/// <summary>
/// Unit tests for CompareService.
/// </summary>
public class CompareServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly CompareService _service;

    public CompareServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var compareListRepository = new Repository<CompareList>(_context);
        var itemRepository = new Repository<CompareItem>(_context);
        var listingRepository = new Repository<VehicleListing>(_context);

        _service = new CompareService(compareListRepository, itemRepository, listingRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task AddItemAsync_WithValidListing_AddsToCompareList()
    {
        // Arrange
        var userId = Guid.NewGuid();
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

        var listing = new VehicleListing
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Title = "Test Listing",
            Price = 1000000,
            Mileage = 50000,
            ManufactureYear = 2020,
            Status = ListingStatus.Published
        };
        await _context.VehicleListings.AddAsync(listing);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.AddItemAsync(userId, listing.Id);

        // Assert
        Assert.Single(result.Items);
        Assert.Equal(listing.Id, result.Items[0].ListingId);
    }

    [Fact]
    public async Task AddItemAsync_WithDuplicateListing_ThrowsConflictException()
    {
        // Arrange
        var userId = Guid.NewGuid();
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

        var listing = new VehicleListing
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Title = "Test Listing",
            Price = 1000000,
            Mileage = 50000,
            ManufactureYear = 2020,
            Status = ListingStatus.Published
        };
        await _context.VehicleListings.AddAsync(listing);
        await _context.SaveChangesAsync();

        await _service.AddItemAsync(userId, listing.Id);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _service.AddItemAsync(userId, listing.Id));
    }

    [Fact]
    public async Task AddItemAsync_WithMaximumItems_ThrowsConflictException()
    {
        // Arrange
        var userId = Guid.NewGuid();
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

        for (int i = 0; i < 4; i++)
        {
            var listing = new VehicleListing
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicle.Id,
                Title = $"Test Listing {i}",
                Price = 1000000 + i,
                Mileage = 50000,
                ManufactureYear = 2020,
                Status = ListingStatus.Published
            };
            await _context.VehicleListings.AddAsync(listing);
            await _context.SaveChangesAsync();
            await _service.AddItemAsync(userId, listing.Id);
        }

        var extraListing = new VehicleListing
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Title = "Extra Listing",
            Price = 5000000,
            Mileage = 30000,
            ManufactureYear = 2022,
            Status = ListingStatus.Published
        };
        await _context.VehicleListings.AddAsync(extraListing);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _service.AddItemAsync(userId, extraListing.Id));
    }
}
