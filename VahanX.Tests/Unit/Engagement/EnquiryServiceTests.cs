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
/// Unit tests for EnquiryService.
/// </summary>
public class EnquiryServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly EnquiryService _service;

    public EnquiryServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var enquiryRepository = new Repository<Enquiry>(_context);
        var listingRepository = new Repository<VehicleListing>(_context);

        _service = new EnquiryService(enquiryRepository, listingRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_CreatesEnquiryInNewStatus()
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

        var request = new CreateEnquiryRequest
        {
            ListingId = listing.Id,
            EnquiryType = EnquiryType.General,
            Subject = "Test Enquiry",
            Message = "I am interested in this vehicle."
        };

        // Act
        var result = await _service.CreateAsync(request, Guid.NewGuid());

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(EnquiryStatus.New, result.Status);
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
            Status = ListingStatus.Draft
        };
        await _context.VehicleListings.AddAsync(listing);
        await _context.SaveChangesAsync();

        var request = new CreateEnquiryRequest
        {
            ListingId = listing.Id,
            EnquiryType = EnquiryType.General,
            Subject = "Test Enquiry",
            Message = "I am interested in this vehicle."
        };

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _service.CreateAsync(request, Guid.NewGuid()));
    }

    [Fact]
    public async Task CloseAsync_WithValidEnquiry_ChangesToClosed()
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

        var userId = Guid.NewGuid();
        var enquiry = new Enquiry
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ListingId = listing.Id,
            Subject = "Test Enquiry",
            Message = "I am interested.",
            Status = EnquiryStatus.New
        };
        await _context.Enquiries.AddAsync(enquiry);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.CloseAsync(enquiry.Id, userId);

        // Assert
        Assert.Equal(EnquiryStatus.Closed, result.Status);
        Assert.NotNull(result.ClosedAt);
    }

    [Fact]
    public async Task CloseAsync_WithDifferentUser_ThrowsForbiddenException()
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

        var enquiry = new Enquiry
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ListingId = listing.Id,
            Subject = "Test Enquiry",
            Message = "I am interested.",
            Status = EnquiryStatus.New
        };
        await _context.Enquiries.AddAsync(enquiry);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(() => _service.CloseAsync(enquiry.Id, Guid.NewGuid()));
    }
}
