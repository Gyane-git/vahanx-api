using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Marketplace;
using VahanX.Application.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;
using VahanX.Infrastructure.Persistence;
using VahanX.Infrastructure.Persistence.Repositories;
using Xunit;

namespace VahanX.Tests.Unit.Marketplace;

/// <summary>
/// Unit tests for ListingService.
/// </summary>
public class ListingServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly ListingService _service;

    public ListingServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var listingRepository = new Repository<VehicleListing>(_context);
        var vehicleRepository = new Repository<Vehicle>(_context);
        var sellerRepository = new Repository<Seller>(_context);
        var dealerRepository = new Repository<Dealer>(_context);

        _service = new ListingService(listingRepository, vehicleRepository, sellerRepository, dealerRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_CreatesListingInDraftStatus()
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

        var request = new CreateListingRequest
        {
            VehicleId = vehicle.Id,
            Title = "Test Listing",
            Price = 1000000,
            Mileage = 50000,
            ManufactureYear = 2020,
            Condition = Condition.Used
        };

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(ListingStatus.Draft, result.Status);
        Assert.Equal("Test Listing", result.Title);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidVehicleId_ThrowsNotFoundException()
    {
        // Arrange
        var request = new CreateListingRequest
        {
            VehicleId = Guid.NewGuid(),
            Title = "Test Listing",
            Price = 1000000,
            Mileage = 50000,
            ManufactureYear = 2020
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateAsync(request));
    }

    [Fact]
    public async Task SubmitAsync_WithDraftStatus_ChangesToPendingReview()
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

        var listing = new VehicleListing
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Title = "Test Listing",
            Price = 1000000,
            Mileage = 50000,
            ManufactureYear = 2020,
            Status = ListingStatus.Draft
        };
        await _context.VehicleListings.AddAsync(listing);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.SubmitAsync(listing.Id);

        // Assert
        Assert.Equal(ListingStatus.PendingReview, result.Status);
    }

    [Fact]
    public async Task PublishAsync_WithPendingReviewStatus_ChangesToPublished()
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

        var listing = new VehicleListing
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Title = "Test Listing",
            Price = 1000000,
            Mileage = 50000,
            ManufactureYear = 2020,
            Status = ListingStatus.PendingReview
        };
        await _context.VehicleListings.AddAsync(listing);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.PublishAsync(listing.Id);

        // Assert
        Assert.Equal(ListingStatus.Published, result.Status);
        Assert.NotNull(result.PublishedAt);
    }

    [Fact]
    public async Task UpdateAsync_WithSoldStatus_ThrowsConflictException()
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

        var listing = new VehicleListing
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Title = "Test Listing",
            Price = 1000000,
            Mileage = 50000,
            ManufactureYear = 2020,
            Status = ListingStatus.Sold
        };
        await _context.VehicleListings.AddAsync(listing);
        await _context.SaveChangesAsync();

        var request = new UpdateListingRequest
        {
            Title = "Updated Title",
            Price = 2000000,
            Mileage = 60000,
            ManufactureYear = 2021
        };

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _service.UpdateAsync(listing.Id, request));
    }
}
