using Microsoft.EntityFrameworkCore;
using VahanX.Application.DTOs.Trust;
using VahanX.Application.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;
using VahanX.Infrastructure.Persistence;
using VahanX.Infrastructure.Persistence.Repositories;
using Xunit;

namespace VahanX.Tests.Unit.Trust;

/// <summary>
/// Unit tests for ReviewService.
/// </summary>
public class ReviewServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly ReviewService _service;

    public ReviewServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var vehicleReviewRepository = new Repository<VehicleReview>(_context);
        var sellerReviewRepository = new Repository<SellerReview>(_context);
        var dealerReviewRepository = new Repository<DealerReview>(_context);
        var reviewReportRepository = new Repository<ReviewReport>(_context);
        var vehicleRepository = new Repository<Vehicle>(_context);
        var sellerRepository = new Repository<Seller>(_context);
        var dealerRepository = new Repository<Dealer>(_context);

        _service = new ReviewService(vehicleReviewRepository, sellerReviewRepository, dealerReviewRepository, reviewReportRepository, vehicleRepository, sellerRepository, dealerRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreateVehicleReviewAsync_WithValidRequest_CreatesReviewInPendingStatus()
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

        var request = new CreateReviewRequest
        {
            Rating = 5,
            Title = "Great car!",
            Comment = "Very satisfied with the purchase."
        };

        // Act
        var result = await _service.CreateVehicleReviewAsync(vehicle.Id, Guid.NewGuid(), request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(5, result.Rating);
        Assert.Equal(ReviewStatus.Pending, result.Status);
    }

    [Fact]
    public async Task CreateVehicleReviewAsync_WithDuplicateReview_ThrowsConflictException()
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

        var userId = Guid.NewGuid();
        var request = new CreateReviewRequest
        {
            Rating = 5,
            Title = "Great car!"
        };

        await _service.CreateVehicleReviewAsync(vehicle.Id, userId, request);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _service.CreateVehicleReviewAsync(vehicle.Id, userId, request));
    }

    [Fact]
    public async Task CreateSellerReviewAsync_WithOwnProfile_ThrowsConflictException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var seller = new Seller
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            DisplayName = "Test Seller",
            Phone = "9841234567"
        };
        await _context.Sellers.AddAsync(seller);
        await _context.SaveChangesAsync();

        var request = new CreateReviewRequest
        {
            Rating = 5,
            Title = "Great seller!"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _service.CreateSellerReviewAsync(seller.Id, userId, request));
    }

    [Fact]
    public async Task GetVehicleRatingSummaryAsync_WithPublishedReviews_ReturnsCorrectSummary()
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

        var review1 = new VehicleReview
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Rating = 5,
            Status = ReviewStatus.Published
        };

        var review2 = new VehicleReview
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Rating = 3,
            Status = ReviewStatus.Published
        };

        await _context.VehicleReviews.AddRangeAsync(review1, review2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetVehicleRatingSummaryAsync(vehicle.Id);

        // Assert
        Assert.Equal(2, result.TotalReviews);
        Assert.Equal(4.0m, result.AverageRating);
        Assert.Equal(0, result.Rating1Count);
        Assert.Equal(0, result.Rating2Count);
        Assert.Equal(1, result.Rating3Count);
        Assert.Equal(0, result.Rating4Count);
        Assert.Equal(1, result.Rating5Count);
    }
}
