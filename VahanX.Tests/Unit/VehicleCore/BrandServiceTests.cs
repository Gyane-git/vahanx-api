using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.DTOs.VehicleCore;
using VahanX.Domain.Exceptions;
using VahanX.Infrastructure.Persistence;

namespace VahanX.Tests.Unit.VehicleCore;

/// <summary>
/// Unit tests for BrandService.
/// </summary>
public class BrandServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly VahanX.Application.Services.BrandService _service;

    public BrandServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);
        _service = new VahanX.Application.Services.BrandService(
            new VahanX.Infrastructure.Persistence.Repositories.Repository<VahanX.Domain.Entities.Brand>(_context));
    }

    [Fact]
    public async Task CreateBrand_WithValidData_ShouldReturnBrand()
    {
        var request = new CreateBrandRequest
        {
            Name = "Toyota",
            Code = "TOYOTA",
            Description = "Japanese automaker",
            CountryOfOrigin = "Japan"
        };

        var result = await _service.CreateAsync(request);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Toyota", result.Name);
        Assert.Equal("TOYOTA", result.Code);
    }

    [Fact]
    public async Task CreateBrand_WithDuplicateCode_ShouldThrowConflictException()
    {
        var request = new CreateBrandRequest
        {
            Name = "Toyota",
            Code = "TOYOTA"
        };

        await _service.CreateAsync(request);

        var duplicateRequest = new CreateBrandRequest
        {
            Name = "Toyota Duplicate",
            Code = "TOYOTA"
        };

        await Assert.ThrowsAsync<ConflictException>(() => _service.CreateAsync(duplicateRequest));
    }

    [Fact]
    public async Task GetById_WithExistingId_ShouldReturnBrand()
    {
        var createRequest = new CreateBrandRequest
        {
            Name = "Honda",
            Code = "HONDA"
        };

        var created = await _service.CreateAsync(createRequest);
        var result = await _service.GetByIdAsync(created.Id);

        Assert.NotNull(result);
        Assert.Equal("Honda", result.Name);
    }

    [Fact]
    public async Task GetById_WithNonExistingId_ShouldReturnNull()
    {
        var result = await _service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateBrand_WithValidData_ShouldReturnUpdatedBrand()
    {
        var createRequest = new CreateBrandRequest
        {
            Name = "Hyundai",
            Code = "HYUNDAI"
        };

        var created = await _service.CreateAsync(createRequest);

        var updateRequest = new UpdateBrandRequest
        {
            Name = "Hyundai Updated",
            Code = "HYUNDAI_UPDATED"
        };

        var result = await _service.UpdateAsync(created.Id, updateRequest);

        Assert.Equal("Hyundai Updated", result.Name);
        Assert.Equal("HYUNDAI_UPDATED", result.Code);
    }

    [Fact]
    public async Task DeleteBrand_WithExistingId_ShouldRemoveBrand()
    {
        var createRequest = new CreateBrandRequest
        {
            Name = "Kia",
            Code = "KIA"
        };

        var created = await _service.CreateAsync(createRequest);
        await _service.DeleteAsync(created.Id);

        var result = await _service.GetByIdAsync(created.Id);
        Assert.Null(result);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
