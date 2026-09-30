using Microsoft.EntityFrameworkCore;
using VahanX.Application.DTOs.Services;
using VahanX.Application.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;
using VahanX.Infrastructure.Persistence;
using VahanX.Infrastructure.Persistence.Repositories;
using Xunit;

namespace VahanX.Tests.Unit.Services;

/// <summary>
/// Unit tests for ServiceBookingService.
/// </summary>
public class ServiceBookingServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly ServiceBookingService _service;

    public ServiceBookingServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var bookingRepository = new Repository<ServiceBooking>(_context);
        var branchRepository = new Repository<ServiceCenterBranch>(_context);

        _service = new ServiceBookingService(bookingRepository, branchRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_CreatesBookingInRequestedStatus()
    {
        // Arrange
        var branch = new ServiceCenterBranch
        {
            Id = Guid.NewGuid(),
            ServiceCenterId = Guid.NewGuid(),
            Name = "Test Branch",
            Status = ServiceCenterStatus.Active
        };
        await _context.ServiceCenterBranches.AddAsync(branch);
        await _context.SaveChangesAsync();

        var request = new CreateServiceBookingRequest
        {
            ServiceCenterBranchId = branch.Id,
            RequestedDate = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var result = await _service.CreateAsync(request, Guid.NewGuid());

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(ServiceBookingStatus.Requested, result.Status);
    }

    [Fact]
    public async Task CreateAsync_WithInactiveBranch_ThrowsConflictException()
    {
        // Arrange
        var branch = new ServiceCenterBranch
        {
            Id = Guid.NewGuid(),
            ServiceCenterId = Guid.NewGuid(),
            Name = "Test Branch",
            Status = ServiceCenterStatus.Inactive
        };
        await _context.ServiceCenterBranches.AddAsync(branch);
        await _context.SaveChangesAsync();

        var request = new CreateServiceBookingRequest
        {
            ServiceCenterBranchId = branch.Id,
            RequestedDate = DateTime.UtcNow.AddDays(1)
        };

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _service.CreateAsync(request, Guid.NewGuid()));
    }

    [Fact]
    public async Task ConfirmAsync_WithRequestedStatus_ChangesToConfirmed()
    {
        // Arrange
        var branch = new ServiceCenterBranch
        {
            Id = Guid.NewGuid(),
            ServiceCenterId = Guid.NewGuid(),
            Name = "Test Branch",
            Status = ServiceCenterStatus.Active
        };
        await _context.ServiceCenterBranches.AddAsync(branch);
        await _context.SaveChangesAsync();

        var booking = new ServiceBooking
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ServiceCenterBranchId = branch.Id,
            RequestedDate = DateTime.UtcNow.AddDays(1),
            Status = ServiceBookingStatus.Requested,
            BookingReference = "SB-TEST-001"
        };
        await _context.ServiceBookings.AddAsync(booking);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.ConfirmAsync(booking.Id, booking.UserId);

        // Assert
        Assert.Equal(ServiceBookingStatus.Confirmed, result.Status);
        Assert.NotNull(result.ConfirmedAt);
    }

    [Fact]
    public async Task CompleteAsync_WithInProgressStatus_ChangesToCompleted()
    {
        // Arrange
        var branch = new ServiceCenterBranch
        {
            Id = Guid.NewGuid(),
            ServiceCenterId = Guid.NewGuid(),
            Name = "Test Branch",
            Status = ServiceCenterStatus.Active
        };
        await _context.ServiceCenterBranches.AddAsync(branch);
        await _context.SaveChangesAsync();

        var booking = new ServiceBooking
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ServiceCenterBranchId = branch.Id,
            RequestedDate = DateTime.UtcNow.AddDays(1),
            Status = ServiceBookingStatus.InProgress,
            BookingReference = "SB-TEST-002"
        };
        await _context.ServiceBookings.AddAsync(booking);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.CompleteAsync(booking.Id, booking.UserId);

        // Assert
        Assert.Equal(ServiceBookingStatus.Completed, result.Status);
        Assert.NotNull(result.CompletedAt);
    }
}
