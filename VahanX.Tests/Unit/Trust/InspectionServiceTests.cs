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
/// Unit tests for InspectionService.
/// </summary>
public class InspectionServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly InspectionService _service;

    public InspectionServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var inspectionRepository = new Repository<Inspection>(_context);
        var itemRepository = new Repository<InspectionItem>(_context);
        var vehicleRepository = new Repository<Vehicle>(_context);
        var listingRepository = new Repository<VehicleListing>(_context);

        _service = new InspectionService(inspectionRepository, itemRepository, vehicleRepository, listingRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_CreatesInspectionInScheduledStatus()
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

        var request = new CreateInspectionRequest
        {
            VehicleId = vehicle.Id,
            InspectionType = InspectionType.PreSale
        };

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(InspectionStatus.Scheduled, result.Status);
    }

    [Fact]
    public async Task StartAsync_WithScheduledStatus_ChangesToInProgress()
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

        var inspection = new Inspection
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Status = InspectionStatus.Scheduled,
            InspectionType = InspectionType.PreSale
        };
        await _context.Inspections.AddAsync(inspection);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.StartAsync(inspection.Id);

        // Assert
        Assert.Equal(InspectionStatus.InProgress, result.Status);
    }

    [Fact]
    public async Task CompleteAsync_WithInProgressStatus_ChangesToCompleted()
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

        var inspection = new Inspection
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Status = InspectionStatus.InProgress,
            InspectionType = InspectionType.PreSale
        };
        await _context.Inspections.AddAsync(inspection);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.CompleteAsync(inspection.Id);

        // Assert
        Assert.Equal(InspectionStatus.Completed, result.Status);
        Assert.NotNull(result.CompletedAt);
    }

    [Fact]
    public async Task AddItemAsync_WithValidRequest_AddsItem()
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

        var inspection = new Inspection
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Status = InspectionStatus.InProgress,
            InspectionType = InspectionType.PreSale
        };
        await _context.Inspections.AddAsync(inspection);
        await _context.SaveChangesAsync();

        var request = new CreateInspectionItemRequest
        {
            Category = "Engine",
            ItemName = "Oil Level",
            Condition = InspectionItemCondition.Good,
            Score = 85
        };

        // Act
        var result = await _service.AddItemAsync(inspection.Id, request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Engine", result.Category);
        Assert.Equal("Oil Level", result.ItemName);
    }
}
