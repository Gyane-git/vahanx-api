using Microsoft.EntityFrameworkCore;
using VahanX.Application.DTOs.Engagement;
using VahanX.Application.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;
using VahanX.Infrastructure.Persistence;
using VahanX.Infrastructure.Persistence.Repositories;
using Xunit;

namespace VahanX.Tests.Unit.Engagement;

/// <summary>
/// Unit tests for TestDriveService.
/// </summary>
public class TestDriveServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly TestDriveService _service;

    public TestDriveServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var testDriveRepository = new Repository<TestDrive>(_context);
        var listingRepository = new Repository<VehicleListing>(_context);

        _service = new TestDriveService(testDriveRepository, listingRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_CreatesTestDriveInRequestedStatus()
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
            Status = ListingStatus.Published
        };
        await _context.VehicleListings.AddAsync(listing);
        await _context.SaveChangesAsync();

        var request = new CreateTestDriveRequest
        {
            ListingId = listing.Id,
            RequestedDate = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var result = await _service.CreateAsync(request, Guid.NewGuid());

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(TestDriveStatus.Requested, result.Status);
    }

    [Fact]
    public async Task CreateAsync_WithNonPublishedListing_ThrowsConflictException()
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

        var request = new CreateTestDriveRequest
        {
            ListingId = listing.Id,
            RequestedDate = DateTime.UtcNow.AddDays(1)
        };

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _service.CreateAsync(request, Guid.NewGuid()));
    }

    [Fact]
    public async Task ConfirmAsync_WithRequestedStatus_ChangesToConfirmed()
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
            Status = ListingStatus.Published
        };
        await _context.VehicleListings.AddAsync(listing);
        await _context.SaveChangesAsync();

        var testDrive = new TestDrive
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ListingId = listing.Id,
            RequestedDate = DateTime.UtcNow.AddDays(1),
            Status = TestDriveStatus.Requested
        };
        await _context.TestDrives.AddAsync(testDrive);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.ConfirmAsync(testDrive.Id, testDrive.UserId);

        // Assert
        Assert.Equal(TestDriveStatus.Confirmed, result.Status);
    }

    [Fact]
    public async Task CancelAsync_WithConfirmedStatus_ChangesToCancelled()
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
            Status = ListingStatus.Published
        };
        await _context.VehicleListings.AddAsync(listing);
        await _context.SaveChangesAsync();

        var testDrive = new TestDrive
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ListingId = listing.Id,
            RequestedDate = DateTime.UtcNow.AddDays(1),
            Status = TestDriveStatus.Confirmed
        };
        await _context.TestDrives.AddAsync(testDrive);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.CancelAsync(testDrive.Id, testDrive.UserId);

        // Assert
        Assert.Equal(TestDriveStatus.Cancelled, result.Status);
        Assert.NotNull(result.CancelledAt);
    }
}
