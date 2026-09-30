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
/// Unit tests for WishlistService.
/// </summary>
public class WishlistServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly WishlistService _service;

    public WishlistServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var wishlistRepository = new Repository<Wishlist>(_context);
        var itemRepository = new Repository<WishlistItem>(_context);
        var listingRepository = new Repository<VehicleListing>(_context);

        _service = new WishlistService(wishlistRepository, itemRepository, listingRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task AddItemAsync_WithValidListing_AddsToWishlist()
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
    public async Task RemoveItemAsync_WithExistingItem_RemovesFromWishlist()
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

        // Act
        var result = await _service.RemoveItemAsync(userId, listing.Id);

        // Assert
        Assert.Empty(result.Items);
    }
}
