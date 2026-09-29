using Microsoft.EntityFrameworkCore;
using VahanX.Application.DTOs.VehicleCore;
using VahanX.Domain.Exceptions;
using VahanX.Infrastructure.Persistence;

namespace VahanX.Tests.Unit.VehicleCore;

/// <summary>
/// Unit tests for GenerationService.
/// </summary>
public class GenerationServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly VahanX.Application.Services.GenerationService _service;
    private readonly VahanX.Infrastructure.Persistence.Repositories.Repository<VahanX.Domain.Entities.Model> _modelRepository;

    public GenerationServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);
        _service = new VahanX.Application.Services.GenerationService(
            new VahanX.Infrastructure.Persistence.Repositories.Repository<VahanX.Domain.Entities.Generation>(_context),
            new VahanX.Infrastructure.Persistence.Repositories.Repository<VahanX.Domain.Entities.Model>(_context));
        _modelRepository = new VahanX.Infrastructure.Persistence.Repositories.Repository<VahanX.Domain.Entities.Model>(_context);
    }

    [Fact]
    public async Task CreateGeneration_WithValidData_ShouldReturnGeneration()
    {
        var model = new VahanX.Domain.Entities.Model
        {
            BrandId = Guid.NewGuid(),
            Name = "Corolla",
            Code = "COROLLA"
        };
        await _modelRepository.AddAsync(model);

        var request = new CreateGenerationRequest
        {
            ModelId = model.Id,
            Name = "12th Generation",
            Code = "12TH_GEN",
            StartYear = 2018,
            EndYear = null
        };

        var result = await _service.CreateAsync(request);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("12th Generation", result.Name);
        Assert.Equal(2018, result.StartYear);
    }

    [Fact]
    public async Task CreateGeneration_WithInvalidModelId_ShouldThrowNotFoundException()
    {
        var request = new CreateGenerationRequest
        {
            ModelId = Guid.NewGuid(),
            Name = "12th Generation",
            Code = "12TH_GEN",
            StartYear = 2018
        };

        await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateAsync(request));
    }

    [Fact]
    public async Task CreateGeneration_WithDuplicateCode_ShouldThrowConflictException()
    {
        var model = new VahanX.Domain.Entities.Model
        {
            BrandId = Guid.NewGuid(),
            Name = "Corolla",
            Code = "COROLLA"
        };
        await _modelRepository.AddAsync(model);

        var request = new CreateGenerationRequest
        {
            ModelId = model.Id,
            Name = "12th Generation",
            Code = "12TH_GEN",
            StartYear = 2018
        };

        await _service.CreateAsync(request);

        var duplicateRequest = new CreateGenerationRequest
        {
            ModelId = model.Id,
            Name = "12th Generation Duplicate",
            Code = "12TH_GEN",
            StartYear = 2018
        };

        await Assert.ThrowsAsync<ConflictException>(() => _service.CreateAsync(duplicateRequest));
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
