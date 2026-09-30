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
/// Unit tests for DealerService.
/// </summary>
public class DealerServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly DealerService _service;

    public DealerServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var dealerRepository = new Repository<Dealer>(_context);
        var branchRepository = new Repository<DealerBranch>(_context);
        var listingRepository = new Repository<VehicleListing>(_context);

        _service = new DealerService(dealerRepository, branchRepository, listingRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_CreatesDealer()
    {
        // Arrange
        var request = new CreateDealerRequest
        {
            BusinessName = "Test Dealer",
            Phone = "9841234567",
            Email = "dealer@example.com"
        };

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Test Dealer", result.BusinessName);
    }

    [Fact]
    public async Task CreateBranchAsync_WithValidRequest_CreatesBranch()
    {
        // Arrange
        var dealer = new Dealer
        {
            Id = Guid.NewGuid(),
            BusinessName = "Test Dealer",
            Phone = "9841234567"
        };
        await _context.Dealers.AddAsync(dealer);
        await _context.SaveChangesAsync();

        var request = new CreateDealerBranchRequest
        {
            Name = "Main Branch",
            Phone = "9841234567"
        };

        // Act
        var result = await _service.CreateBranchAsync(dealer.Id, request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Main Branch", result.Name);
        Assert.Equal(dealer.Id, result.DealerId);
    }

    [Fact]
    public async Task CreateBranchAsync_WithInvalidDealerId_ThrowsNotFoundException()
    {
        // Arrange
        var request = new CreateDealerBranchRequest
        {
            Name = "Main Branch"
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateBranchAsync(Guid.NewGuid(), request));
    }
}
