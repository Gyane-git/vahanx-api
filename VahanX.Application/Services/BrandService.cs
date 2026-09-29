using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.VehicleCore;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of brand service.
/// </summary>
public class BrandService : IBrandService
{
    private readonly IRepository<Brand> _repository;

    public BrandService(IRepository<Brand> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<BrandDto>> GetAllAsync(int page, int pageSize, string? search = null, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(b => b.Name.Contains(search) || b.Code.Contains(search));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(b => b.DisplayOrder)
            .ThenBy(b => b.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BrandDto
            {
                Id = b.Id,
                Name = b.Name,
                Code = b.Code,
                Description = b.Description,
                LogoMediaId = b.LogoMediaId,
                CountryOfOrigin = b.CountryOfOrigin,
                IsActive = b.IsActive,
                DisplayOrder = b.DisplayOrder
            })
            .ToListAsync(cancellationToken);

        return PagedResult<BrandDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<BrandDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var brand = await _repository.GetByIdAsync(id, cancellationToken);

        if (brand is null)
            return null;

        return new BrandDto
        {
            Id = brand.Id,
            Name = brand.Name,
            Code = brand.Code,
            Description = brand.Description,
            LogoMediaId = brand.LogoMediaId,
            CountryOfOrigin = brand.CountryOfOrigin,
            IsActive = brand.IsActive,
            DisplayOrder = brand.DisplayOrder
        };
    }

    public async Task<BrandDto> CreateAsync(CreateBrandRequest request, CancellationToken cancellationToken = default)
    {
        var exists = await _repository.AnyAsync(b => b.Code == request.Code, cancellationToken);

        if (exists)
            throw new ConflictException($"Brand with code '{request.Code}' already exists.");

        var brand = new Brand
        {
            Name = request.Name,
            Code = request.Code,
            Description = request.Description,
            LogoMediaId = request.LogoMediaId,
            CountryOfOrigin = request.CountryOfOrigin,
            IsActive = request.IsActive,
            DisplayOrder = request.DisplayOrder
        };

        await _repository.AddAsync(brand, cancellationToken);

        return new BrandDto
        {
            Id = brand.Id,
            Name = brand.Name,
            Code = brand.Code,
            Description = brand.Description,
            LogoMediaId = brand.LogoMediaId,
            CountryOfOrigin = brand.CountryOfOrigin,
            IsActive = brand.IsActive,
            DisplayOrder = brand.DisplayOrder
        };
    }

    public async Task<BrandDto> UpdateAsync(Guid id, UpdateBrandRequest request, CancellationToken cancellationToken = default)
    {
        var brand = await _repository.GetByIdAsync(id, cancellationToken);

        if (brand is null)
            throw new NotFoundException("Brand", id);

        var exists = await _repository.AnyAsync(b => b.Code == request.Code && b.Id != id, cancellationToken);

        if (exists)
            throw new ConflictException($"Brand with code '{request.Code}' already exists.");

        brand.Name = request.Name;
        brand.Code = request.Code;
        brand.Description = request.Description;
        brand.LogoMediaId = request.LogoMediaId;
        brand.CountryOfOrigin = request.CountryOfOrigin;
        brand.IsActive = request.IsActive;
        brand.DisplayOrder = request.DisplayOrder;

        await _repository.UpdateAsync(brand, cancellationToken);

        return new BrandDto
        {
            Id = brand.Id,
            Name = brand.Name,
            Code = brand.Code,
            Description = brand.Description,
            LogoMediaId = brand.LogoMediaId,
            CountryOfOrigin = brand.CountryOfOrigin,
            IsActive = brand.IsActive,
            DisplayOrder = brand.DisplayOrder
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var brand = await _repository.GetByIdAsync(id, cancellationToken);

        if (brand is null)
            throw new NotFoundException("Brand", id);

        await _repository.DeleteAsync(brand, cancellationToken);
    }
}
