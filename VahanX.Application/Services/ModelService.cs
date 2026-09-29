using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.VehicleCore;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of model service.
/// </summary>
public class ModelService : IModelService
{
    private readonly IRepository<Model> _repository;
    private readonly IRepository<Brand> _brandRepository;

    public ModelService(IRepository<Model> repository, IRepository<Brand> brandRepository)
    {
        _repository = repository;
        _brandRepository = brandRepository;
    }

    public async Task<PagedResult<ModelDto>> GetAllAsync(int page, int pageSize, Guid? brandId = null, string? search = null, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        if (brandId.HasValue)
            query = query.Where(m => m.BrandId == brandId.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(m => m.Name.Contains(search) || m.Code.Contains(search));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(m => m.DisplayOrder)
            .ThenBy(m => m.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new ModelDto
            {
                Id = m.Id,
                BrandId = m.BrandId,
                BrandName = m.Brand != null ? m.Brand.Name : string.Empty,
                Name = m.Name,
                Code = m.Code,
                Description = m.Description,
                IsActive = m.IsActive,
                DisplayOrder = m.DisplayOrder
            })
            .ToListAsync(cancellationToken);

        return PagedResult<ModelDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<ModelDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var model = await _repository.GetByIdAsync(id, cancellationToken);

        if (model is null)
            return null;

        return new ModelDto
        {
            Id = model.Id,
            BrandId = model.BrandId,
            BrandName = model.Brand != null ? model.Brand.Name : string.Empty,
            Name = model.Name,
            Code = model.Code,
            Description = model.Description,
            IsActive = model.IsActive,
            DisplayOrder = model.DisplayOrder
        };
    }

    public async Task<ModelDto> CreateAsync(CreateModelRequest request, CancellationToken cancellationToken = default)
    {
        var brandExists = await _brandRepository.AnyAsync(b => b.Id == request.BrandId, cancellationToken);

        if (!brandExists)
            throw new NotFoundException("Brand", request.BrandId);

        var exists = await _repository.AnyAsync(m => m.BrandId == request.BrandId && m.Code == request.Code, cancellationToken);

        if (exists)
            throw new ConflictException($"Model with code '{request.Code}' already exists for this brand.");

        var model = new Model
        {
            BrandId = request.BrandId,
            Name = request.Name,
            Code = request.Code,
            Description = request.Description,
            IsActive = request.IsActive,
            DisplayOrder = request.DisplayOrder
        };

        await _repository.AddAsync(model, cancellationToken);

        return new ModelDto
        {
            Id = model.Id,
            BrandId = model.BrandId,
            BrandName = string.Empty,
            Name = model.Name,
            Code = model.Code,
            Description = model.Description,
            IsActive = model.IsActive,
            DisplayOrder = model.DisplayOrder
        };
    }

    public async Task<ModelDto> UpdateAsync(Guid id, UpdateModelRequest request, CancellationToken cancellationToken = default)
    {
        var model = await _repository.GetByIdAsync(id, cancellationToken);

        if (model is null)
            throw new NotFoundException("Model", id);

        var exists = await _repository.AnyAsync(m => m.BrandId == model.BrandId && m.Code == request.Code && m.Id != id, cancellationToken);

        if (exists)
            throw new ConflictException($"Model with code '{request.Code}' already exists for this brand.");

        model.Name = request.Name;
        model.Code = request.Code;
        model.Description = request.Description;
        model.IsActive = request.IsActive;
        model.DisplayOrder = request.DisplayOrder;

        await _repository.UpdateAsync(model, cancellationToken);

        return new ModelDto
        {
            Id = model.Id,
            BrandId = model.BrandId,
            BrandName = string.Empty,
            Name = model.Name,
            Code = model.Code,
            Description = model.Description,
            IsActive = model.IsActive,
            DisplayOrder = model.DisplayOrder
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var model = await _repository.GetByIdAsync(id, cancellationToken);

        if (model is null)
            throw new NotFoundException("Model", id);

        await _repository.DeleteAsync(model, cancellationToken);
    }
}
