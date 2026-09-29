using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.VehicleCore;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of variant service.
/// </summary>
public class VariantService : IVariantService
{
    private readonly IRepository<Variant> _repository;
    private readonly IRepository<Generation> _generationRepository;

    public VariantService(IRepository<Variant> repository, IRepository<Generation> generationRepository)
    {
        _repository = repository;
        _generationRepository = generationRepository;
    }

    public async Task<PagedResult<VariantDto>> GetAllAsync(int page, int pageSize, Guid? generationId = null, string? search = null, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        if (generationId.HasValue)
            query = query.Where(v => v.GenerationId == generationId.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(v => v.Name.Contains(search) || v.Code.Contains(search));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(v => v.DisplayOrder)
            .ThenBy(v => v.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(v => new VariantDto
            {
                Id = v.Id,
                GenerationId = v.GenerationId,
                GenerationName = v.Generation != null ? v.Generation.Name : string.Empty,
                Name = v.Name,
                Code = v.Code,
                Description = v.Description,
                ModelYearFrom = v.ModelYearFrom,
                ModelYearTo = v.ModelYearTo,
                IsActive = v.IsActive,
                DisplayOrder = v.DisplayOrder
            })
            .ToListAsync(cancellationToken);

        return PagedResult<VariantDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<VariantDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var variant = await _repository.GetByIdAsync(id, cancellationToken);

        if (variant is null)
            return null;

        return new VariantDto
        {
            Id = variant.Id,
            GenerationId = variant.GenerationId,
            GenerationName = variant.Generation != null ? variant.Generation.Name : string.Empty,
            Name = variant.Name,
            Code = variant.Code,
            Description = variant.Description,
            ModelYearFrom = variant.ModelYearFrom,
            ModelYearTo = variant.ModelYearTo,
            IsActive = variant.IsActive,
            DisplayOrder = variant.DisplayOrder
        };
    }

    public async Task<VariantDto> CreateAsync(CreateVariantRequest request, CancellationToken cancellationToken = default)
    {
        var generationExists = await _generationRepository.AnyAsync(g => g.Id == request.GenerationId, cancellationToken);

        if (!generationExists)
            throw new NotFoundException("Generation", request.GenerationId);

        var exists = await _repository.AnyAsync(v => v.GenerationId == request.GenerationId && v.Code == request.Code, cancellationToken);

        if (exists)
            throw new ConflictException($"Variant with code '{request.Code}' already exists for this generation.");

        var variant = new Variant
        {
            GenerationId = request.GenerationId,
            Name = request.Name,
            Code = request.Code,
            Description = request.Description,
            ModelYearFrom = request.ModelYearFrom,
            ModelYearTo = request.ModelYearTo,
            IsActive = request.IsActive,
            DisplayOrder = request.DisplayOrder
        };

        await _repository.AddAsync(variant, cancellationToken);

        return new VariantDto
        {
            Id = variant.Id,
            GenerationId = variant.GenerationId,
            GenerationName = string.Empty,
            Name = variant.Name,
            Code = variant.Code,
            Description = variant.Description,
            ModelYearFrom = variant.ModelYearFrom,
            ModelYearTo = variant.ModelYearTo,
            IsActive = variant.IsActive,
            DisplayOrder = variant.DisplayOrder
        };
    }

    public async Task<VariantDto> UpdateAsync(Guid id, UpdateVariantRequest request, CancellationToken cancellationToken = default)
    {
        var variant = await _repository.GetByIdAsync(id, cancellationToken);

        if (variant is null)
            throw new NotFoundException("Variant", id);

        var exists = await _repository.AnyAsync(v => v.GenerationId == variant.GenerationId && v.Code == request.Code && v.Id != id, cancellationToken);

        if (exists)
            throw new ConflictException($"Variant with code '{request.Code}' already exists for this generation.");

        variant.Name = request.Name;
        variant.Code = request.Code;
        variant.Description = request.Description;
        variant.ModelYearFrom = request.ModelYearFrom;
        variant.ModelYearTo = request.ModelYearTo;
        variant.IsActive = request.IsActive;
        variant.DisplayOrder = request.DisplayOrder;

        await _repository.UpdateAsync(variant, cancellationToken);

        return new VariantDto
        {
            Id = variant.Id,
            GenerationId = variant.GenerationId,
            GenerationName = string.Empty,
            Name = variant.Name,
            Code = variant.Code,
            Description = variant.Description,
            ModelYearFrom = variant.ModelYearFrom,
            ModelYearTo = variant.ModelYearTo,
            IsActive = variant.IsActive,
            DisplayOrder = variant.DisplayOrder
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var variant = await _repository.GetByIdAsync(id, cancellationToken);

        if (variant is null)
            throw new NotFoundException("Variant", id);

        await _repository.DeleteAsync(variant, cancellationToken);
    }
}
