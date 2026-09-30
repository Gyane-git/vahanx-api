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
/// Unit tests for ServiceCenterService.
/// </summary>
public class ServiceCenterServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly VahanX.Application.Services.ServiceCenterService _service;

    public ServiceCenterServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var serviceCenterRepository = new Repository<ServiceCenter>(_context);
        var branchRepository = new Repository<ServiceCenterBranch>(_context);

        _service = new VahanX.Application.Services.ServiceCenterService(serviceCenterRepository, branchRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_CreatesServiceCenter()
    {
        // Arrange
        var request = new CreateServiceCenterRequest
        {
            Name = "Test Service Center",
            ContactPhone = "9841234567",
            Description = "Test description"
        };

        // Act
        var result = await _service.CreateAsync(request, Guid.NewGuid());

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Test Service Center", result.Name);
        Assert.Equal(ServiceCenterStatus.PendingApproval, result.Status);
    }

    [Fact]
    public async Task CreateBranchAsync_WithValidRequest_CreatesBranch()
    {
        // Arrange
        var serviceCenter = new ServiceCenter
        {
            Id = Guid.NewGuid(),
            Name = "Test Service Center",
            ContactPhone = "9841234567",
            OwnerUserId = Guid.NewGuid(),
            Status = ServiceCenterStatus.Active
        };
        await _context.ServiceCenters.AddAsync(serviceCenter);
        await _context.SaveChangesAsync();

        var request = new CreateServiceCenterBranchRequest
        {
            Name = "Main Branch",
            ContactPhone = "9841234567"
        };

        // Act
        var result = await _service.CreateBranchAsync(serviceCenter.Id, request, serviceCenter.OwnerUserId.Value);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Main Branch", result.Name);
        Assert.Equal(serviceCenter.Id, result.ServiceCenterId);
    }

    [Fact]
    public async Task CreateBranchAsync_WithDifferentUser_ThrowsForbiddenException()
    {
        // Arrange
        var serviceCenter = new ServiceCenter
        {
            Id = Guid.NewGuid(),
            Name = "Test Service Center",
            ContactPhone = "9841234567",
            OwnerUserId = Guid.NewGuid(),
            Status = ServiceCenterStatus.Active
        };
        await _context.ServiceCenters.AddAsync(serviceCenter);
        await _context.SaveChangesAsync();

        var request = new CreateServiceCenterBranchRequest
        {
            Name = "Main Branch"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(() => _service.CreateBranchAsync(serviceCenter.Id, request, Guid.NewGuid()));
    }

    [Fact]
    public async Task GetNearbyAsync_WithValidCoordinates_ReturnsNearbyCenters()
    {
        // Arrange
        var branch = new ServiceCenterBranch
        {
            Id = Guid.NewGuid(),
            ServiceCenterId = Guid.NewGuid(),
            Name = "Test Branch",
            Latitude = 27.7172,
            Longitude = 85.3240,
            Status = ServiceCenterStatus.Active
        };
        await _context.ServiceCenterBranches.AddAsync(branch);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetNearbyAsync(27.7172, 85.3240, 10, 1, 20);

        // Assert
        Assert.NotNull(result);
    }
}
