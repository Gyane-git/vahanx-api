using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.VehicleCore;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of vehicle category service.
/// </summary>
public class VehicleCategoryService : IVehicleCategoryService
{
    private readonly IRepository<VehicleCategory> _repository;
    private readonly IRepository<VehicleType> _vehicleTypeRepository;

    public VehicleCategoryService(IRepository<VehicleCategory> repository, IRepository<VehicleType> vehicleTypeRepository)
    {
        _repository = repository;
        _vehicleTypeRepository = vehicleTypeRepository;
    }

    public async Task<PagedResult<VehicleCategoryDto>> GetAllAsync(int page, int pageSize, Guid? vehicleTypeId = null, string? search = null, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        if (vehicleTypeId.HasValue)
            query = query.Where(c => c.VehicleTypeId == vehicleTypeId.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(c => c.Name.Contains(search) || c.Code.Contains(search));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(c => c.DisplayOrder)
            .ThenBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new VehicleCategoryDto
            {
                Id = c.Id,
                VehicleTypeId = c.VehicleTypeId,
                VehicleTypeName = c.VehicleType != null ? c.VehicleType.Name : string.Empty,
                Name = c.Name,
                Code = c.Code,
                Description = c.Description,
                IsActive = c.IsActive,
                DisplayOrder = c.DisplayOrder
            })
            .ToListAsync(cancellationToken);

        return PagedResult<VehicleCategoryDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<VehicleCategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _repository.GetByIdAsync(id, cancellationToken);

        if (category is null)
            return null;

        return new VehicleCategoryDto
        {
            Id = category.Id,
            VehicleTypeId = category.VehicleTypeId,
            VehicleTypeName = category.VehicleType != null ? category.VehicleType.Name : string.Empty,
            Name = category.Name,
            Code = category.Code,
            Description = category.Description,
            IsActive = category.IsActive,
            DisplayOrder = category.DisplayOrder
        };
    }

    public async Task<VehicleCategoryDto> CreateAsync(CreateVehicleCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var vehicleTypeExists = await _vehicleTypeRepository.AnyAsync(v => v.Id == request.VehicleTypeId, cancellationToken);

        if (!vehicleTypeExists)
            throw new NotFoundException("VehicleType", request.VehicleTypeId);

        var exists = await _repository.AnyAsync(c => c.VehicleTypeId == request.VehicleTypeId && c.Code == request.Code, cancellationToken);

        if (exists)
            throw new ConflictException($"Vehicle category with code '{request.Code}' already exists for this vehicle type.");

        var category = new VehicleCategory
        {
            VehicleTypeId = request.VehicleTypeId,
            Name = request.Name,
            Code = request.Code,
            Description = request.Description,
            IsActive = request.IsActive,
            DisplayOrder = request.DisplayOrder
        };

        await _repository.AddAsync(category, cancellationToken);

        return new VehicleCategoryDto
        {
            Id = category.Id,
            VehicleTypeId = category.VehicleTypeId,
            VehicleTypeName = string.Empty,
            Name = category.Name,
            Code = category.Code,
            Description = category.Description,
            IsActive = category.IsActive,
            DisplayOrder = category.DisplayOrder
        };
    }

    public async Task<VehicleCategoryDto> UpdateAsync(Guid id, UpdateVehicleCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _repository.GetByIdAsync(id, cancellationToken);

        if (category is null)
            throw new NotFoundException("VehicleCategory", id);

        var exists = await _repository.AnyAsync(c => c.VehicleTypeId == category.VehicleTypeId && c.Code == request.Code && c.Id != id, cancellationToken);

        if (exists)
            throw new ConflictException($"Vehicle category with code '{request.Code}' already exists for this vehicle type.");

        category.Name = request.Name;
        category.Code = request.Code;
        category.Description = request.Description;
        category.IsActive = request.IsActive;
        category.DisplayOrder = request.DisplayOrder;

        await _repository.UpdateAsync(category, cancellationToken);

        return new VehicleCategoryDto
        {
            Id = category.Id,
            VehicleTypeId = category.VehicleTypeId,
            VehicleTypeName = string.Empty,
            Name = category.Name,
            Code = category.Code,
            Description = category.Description,
            IsActive = category.IsActive,
            DisplayOrder = category.DisplayOrder
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _repository.GetByIdAsync(id, cancellationToken);

        if (category is null)
            throw new NotFoundException("VehicleCategory", id);

        await _repository.DeleteAsync(category, cancellationToken);
    }
}
