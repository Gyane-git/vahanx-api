using Microsoft.EntityFrameworkCore;
using VahanX.Application.DTOs.Business;
using VahanX.Application.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;
using VahanX.Infrastructure.Persistence;
using VahanX.Infrastructure.Persistence.Repositories;
using Xunit;

namespace VahanX.Tests.Unit.Business;

/// <summary>
/// Unit tests for SellVehicleService.
/// </summary>
public class SellVehicleServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly SellVehicleService _service;

    public SellVehicleServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var requestRepository = new Repository<SellRequest>(_context);
        var vehicleRepository = new Repository<SellVehicle>(_context);
        var offerRepository = new Repository<SellOffer>(_context);
        var historyRepository = new Repository<SellRequestStatusHistory>(_context);

        _service = new SellVehicleService(requestRepository, vehicleRepository, offerRepository, historyRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreateRequestAsync_WithValidRequest_CreatesDraftRequest()
    {
        // Arrange
        var request = new CreateSellRequestRequest
        {
            PreferredContactMethod = ContactPreference.Phone
        };

        // Act
        var result = await _service.CreateRequestAsync(request, Guid.NewGuid());

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(SellRequestStatus.Draft, result.Status);
    }

    [Fact]
    public async Task SubmitRequestAsync_WithDraftStatus_ChangesToSubmitted()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var sellRequest = new SellRequest
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Status = SellRequestStatus.Draft
        };
        await _context.SellRequests.AddAsync(sellRequest);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.SubmitRequestAsync(sellRequest.Id, userId);

        // Assert
        Assert.Equal(SellRequestStatus.Submitted, result.Status);
    }

    [Fact]
    public async Task SubmitRequestAsync_WithDifferentUser_ThrowsForbiddenException()
    {
        // Arrange
        var sellRequest = new SellRequest
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Status = SellRequestStatus.Draft
        };
        await _context.SellRequests.AddAsync(sellRequest);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(() => _service.SubmitRequestAsync(sellRequest.Id, Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateOfferAsync_WithValidRequest_CreatesOffer()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var buyerId = Guid.NewGuid();
        var sellRequest = new SellRequest
        {
            Id = Guid.NewGuid(),
            UserId = ownerId,
            Status = SellRequestStatus.Submitted
        };
        await _context.SellRequests.AddAsync(sellRequest);
        await _context.SaveChangesAsync();

        var request = new CreateSellOfferRequest
        {
            OfferedAmount = 5000000
        };

        // Act
        var result = await _service.CreateOfferAsync(sellRequest.Id, request, buyerId);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(5000000m, result.OfferedAmount);
        Assert.Equal(SellOfferStatus.Submitted, result.Status);
    }

    [Fact]
    public async Task CreateOfferAsync_WithOwnRequest_ThrowsConflictException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var sellRequest = new SellRequest
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Status = SellRequestStatus.Submitted
        };
        await _context.SellRequests.AddAsync(sellRequest);
        await _context.SaveChangesAsync();

        var request = new CreateSellOfferRequest
        {
            OfferedAmount = 5000000
        };

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _service.CreateOfferAsync(sellRequest.Id, request, userId));
    }
}
