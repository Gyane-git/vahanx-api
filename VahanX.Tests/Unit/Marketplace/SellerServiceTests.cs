using Microsoft.EntityFrameworkCore;
using VahanX.Application.DTOs.Marketplace;
using VahanX.Application.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;
using VahanX.Infrastructure.Persistence;
using VahanX.Infrastructure.Persistence.Repositories;
using Xunit;

namespace VahanX.Tests.Unit.Marketplace;

/// <summary>
/// Unit tests for SellerService.
/// </summary>
public class SellerServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly SellerService _service;

    public SellerServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var sellerRepository = new Repository<Seller>(_context);
        var listingRepository = new Repository<VehicleListing>(_context);

        _service = new SellerService(sellerRepository, listingRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_CreatesSeller()
    {
        // Arrange
        var request = new CreateSellerRequest
        {
            UserId = Guid.NewGuid(),
            DisplayName = "Test Seller",
            Phone = "9841234567",
            Email = "test@example.com"
        };

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Test Seller", result.DisplayName);
        Assert.Equal("9841234567", result.Phone);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateUserId_ThrowsConflictException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var seller = new Seller
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            DisplayName = "Existing Seller",
            Phone = "9841234567"
        };
        await _context.Sellers.AddAsync(seller);
        await _context.SaveChangesAsync();

        var request = new CreateSellerRequest
        {
            UserId = userId,
            DisplayName = "New Seller",
            Phone = "9841234567"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _service.CreateAsync(request));
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingSeller_ReturnsSeller()
    {
        // Arrange
        var seller = new Seller
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            DisplayName = "Test Seller",
            Phone = "9841234567"
        };
        await _context.Sellers.AddAsync(seller);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetByIdAsync(seller.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Seller", result.DisplayName);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingSeller_ReturnsNull()
    {
        // Act
        var result = await _service.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }
}
