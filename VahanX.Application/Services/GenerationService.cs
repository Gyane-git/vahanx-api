using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.VehicleCore;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of generation service.
/// </summary>
public class GenerationService : IGenerationService
{
    private readonly IRepository<Generation> _repository;
    private readonly IRepository<Model> _modelRepository;

    public GenerationService(IRepository<Generation> repository, IRepository<Model> modelRepository)
    {
        _repository = repository;
        _modelRepository = modelRepository;
    }

    public async Task<PagedResult<GenerationDto>> GetAllAsync(int page, int pageSize, Guid? modelId = null, string? search = null, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        if (modelId.HasValue)
            query = query.Where(g => g.ModelId == modelId.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(g => g.Name.Contains(search) || g.Code.Contains(search));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(g => g.DisplayOrder)
            .ThenBy(g => g.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(g => new GenerationDto
            {
                Id = g.Id,
                ModelId = g.ModelId,
                ModelName = g.Model != null ? g.Model.Name : string.Empty,
                Name = g.Name,
                Code = g.Code,
                StartYear = g.StartYear,
                EndYear = g.EndYear,
                Description = g.Description,
                IsActive = g.IsActive,
                DisplayOrder = g.DisplayOrder
            })
            .ToListAsync(cancellationToken);

        return PagedResult<GenerationDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<GenerationDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var generation = await _repository.GetByIdAsync(id, cancellationToken);

        if (generation is null)
            return null;

        return new GenerationDto
        {
            Id = generation.Id,
            ModelId = generation.ModelId,
            ModelName = generation.Model != null ? generation.Model.Name : string.Empty,
            Name = generation.Name,
            Code = generation.Code,
            StartYear = generation.StartYear,
            EndYear = generation.EndYear,
            Description = generation.Description,
            IsActive = generation.IsActive,
            DisplayOrder = generation.DisplayOrder
        };
    }

    public async Task<GenerationDto> CreateAsync(CreateGenerationRequest request, CancellationToken cancellationToken = default)
    {
        var modelExists = await _modelRepository.AnyAsync(m => m.Id == request.ModelId, cancellationToken);

        if (!modelExists)
            throw new NotFoundException("Model", request.ModelId);

        var exists = await _repository.AnyAsync(g => g.ModelId == request.ModelId && g.Code == request.Code, cancellationToken);

        if (exists)
            throw new ConflictException($"Generation with code '{request.Code}' already exists for this model.");

        var generation = new Generation
        {
            ModelId = request.ModelId,
            Name = request.Name,
            Code = request.Code,
            StartYear = request.StartYear,
            EndYear = request.EndYear,
            Description = request.Description,
            IsActive = request.IsActive,
            DisplayOrder = request.DisplayOrder
        };

        await _repository.AddAsync(generation, cancellationToken);

        return new GenerationDto
        {
            Id = generation.Id,
            ModelId = generation.ModelId,
            ModelName = string.Empty,
            Name = generation.Name,
            Code = generation.Code,
            StartYear = generation.StartYear,
            EndYear = generation.EndYear,
            Description = generation.Description,
            IsActive = generation.IsActive,
            DisplayOrder = generation.DisplayOrder
        };
    }

    public async Task<GenerationDto> UpdateAsync(Guid id, UpdateGenerationRequest request, CancellationToken cancellationToken = default)
    {
        var generation = await _repository.GetByIdAsync(id, cancellationToken);

        if (generation is null)
            throw new NotFoundException("Generation", id);

        var exists = await _repository.AnyAsync(g => g.ModelId == generation.ModelId && g.Code == request.Code && g.Id != id, cancellationToken);

        if (exists)
            throw new ConflictException($"Generation with code '{request.Code}' already exists for this model.");

        generation.Name = request.Name;
        generation.Code = request.Code;
        generation.StartYear = request.StartYear;
        generation.EndYear = request.EndYear;
        generation.Description = request.Description;
        generation.IsActive = request.IsActive;
        generation.DisplayOrder = request.DisplayOrder;

        await _repository.UpdateAsync(generation, cancellationToken);

        return new GenerationDto
        {
            Id = generation.Id,
            ModelId = generation.ModelId,
            ModelName = string.Empty,
            Name = generation.Name,
            Code = generation.Code,
            StartYear = generation.StartYear,
            EndYear = generation.EndYear,
            Description = generation.Description,
            IsActive = generation.IsActive,
            DisplayOrder = generation.DisplayOrder
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var generation = await _repository.GetByIdAsync(id, cancellationToken);

        if (generation is null)
            throw new NotFoundException("Generation", id);

        await _repository.DeleteAsync(generation, cancellationToken);
    }
}
