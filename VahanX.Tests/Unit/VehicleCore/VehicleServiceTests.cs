using Microsoft.EntityFrameworkCore;
using VahanX.Application.DTOs.VehicleCore;
using VahanX.Domain.Exceptions;
using VahanX.Infrastructure.Persistence;

namespace VahanX.Tests.Unit.VehicleCore;

/// <summary>
/// Unit tests for VehicleService.
/// </summary>
public class VehicleServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly VahanX.Application.Services.VehicleService _service;

    public VehicleServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);
        _service = new VahanX.Application.Services.VehicleService(
            new VahanX.Infrastructure.Persistence.Repositories.Repository<VahanX.Domain.Entities.Vehicle>(_context),
            new VahanX.Infrastructure.Persistence.Repositories.Repository<VahanX.Domain.Entities.Variant>(_context),
            new VahanX.Infrastructure.Persistence.Repositories.Repository<VahanX.Domain.Entities.BodyType>(_context),
            new VahanX.Infrastructure.Persistence.Repositories.Repository<VahanX.Domain.Entities.FuelType>(_context),
            new VahanX.Infrastructure.Persistence.Repositories.Repository<VahanX.Domain.Entities.TransmissionType>(_context),
            new VahanX.Infrastructure.Persistence.Repositories.Repository<VahanX.Domain.Entities.DriveType>(_context),
            new VahanX.Infrastructure.Persistence.Repositories.Repository<VahanX.Domain.Entities.EngineType>(_context));
    }

    [Fact]
    public async Task CreateVehicle_WithInvalidVariantId_ShouldThrowNotFoundException()
    {
        var request = new CreateVehicleRequest
        {
            VariantId = Guid.NewGuid(),
            BodyTypeId = Guid.NewGuid(),
            FuelTypeId = Guid.NewGuid(),
            TransmissionTypeId = Guid.NewGuid(),
            DriveTypeId = Guid.NewGuid(),
            EngineTypeId = Guid.NewGuid()
        };

        await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateAsync(request));
    }

    [Fact]
    public async Task CreateVehicle_WithInvalidBodyTypeId_ShouldThrowNotFoundException()
    {
        var variant = new VahanX.Domain.Entities.Variant
        {
            GenerationId = Guid.NewGuid(),
            Name = "Test Variant",
            Code = "TEST_VARIANT",
            ModelYearFrom = 2020
        };
        await _context.Variants.AddAsync(variant);
        await _context.SaveChangesAsync();

        var request = new CreateVehicleRequest
        {
            VariantId = variant.Id,
            BodyTypeId = Guid.NewGuid(),
            FuelTypeId = Guid.NewGuid(),
            TransmissionTypeId = Guid.NewGuid(),
            DriveTypeId = Guid.NewGuid(),
            EngineTypeId = Guid.NewGuid()
        };

        await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateAsync(request));
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
