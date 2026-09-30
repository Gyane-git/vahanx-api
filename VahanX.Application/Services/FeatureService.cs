using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Business;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of feature service.
/// </summary>
public class FeatureService : IFeatureService
{
    private readonly IRepository<Feature> _repository;

    public FeatureService(IRepository<Feature> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<FeatureResponse>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(f => f.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(f => new FeatureResponse
            {
                Id = f.Id,
                Code = f.Code,
                Name = f.Name,
                Description = f.Description,
                FeatureType = f.FeatureType,
                Unit = f.Unit,
                IsActive = f.IsActive
            })
            .ToListAsync(cancellationToken);

        return PagedResult<FeatureResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<FeatureResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var feature = await _repository.GetByIdAsync(id, cancellationToken);
        if (feature is null) return null;

        return new FeatureResponse
        {
            Id = feature.Id,
            Code = feature.Code,
            Name = feature.Name,
            Description = feature.Description,
            FeatureType = feature.FeatureType,
            Unit = feature.Unit,
            IsActive = feature.IsActive
        };
    }

    public async Task<FeatureResponse> CreateAsync(CreateFeatureRequest request, CancellationToken cancellationToken = default)
    {
        var feature = new Feature
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            FeatureType = request.FeatureType,
            Unit = request.Unit
        };

        await _repository.AddAsync(feature, cancellationToken);

        return new FeatureResponse
        {
            Id = feature.Id,
            Code = feature.Code,
            Name = feature.Name,
            Description = feature.Description,
            FeatureType = feature.FeatureType,
            Unit = feature.Unit,
            IsActive = feature.IsActive
        };
    }

    public async Task<FeatureResponse> UpdateAsync(Guid id, UpdateFeatureRequest request, CancellationToken cancellationToken = default)
    {
        var feature = await _repository.GetByIdAsync(id, cancellationToken);
        if (feature is null) throw new NotFoundException("Feature", id);

        if (request.Name != null) feature.Name = request.Name;
        if (request.Description != null) feature.Description = request.Description;
        if (request.FeatureType.HasValue) feature.FeatureType = request.FeatureType.Value;
        if (request.Unit != null) feature.Unit = request.Unit;
        if (request.IsActive.HasValue) feature.IsActive = request.IsActive.Value;

        await _repository.UpdateAsync(feature, cancellationToken);

        return new FeatureResponse
        {
            Id = feature.Id,
            Code = feature.Code,
            Name = feature.Name,
            Description = feature.Description,
            FeatureType = feature.FeatureType,
            Unit = feature.Unit,
            IsActive = feature.IsActive
        };
    }
}
