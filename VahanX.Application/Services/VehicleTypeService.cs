using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.VehicleCore;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of vehicle type service.
/// </summary>
public class VehicleTypeService : IVehicleTypeService
{
    private readonly IRepository<VehicleType> _repository;

    public VehicleTypeService(IRepository<VehicleType> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<VehicleTypeDto>> GetAllAsync(int page, int pageSize, string? search = null, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(v => v.Name.Contains(search) || v.Code.Contains(search));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(v => v.DisplayOrder)
            .ThenBy(v => v.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(v => new VehicleTypeDto
            {
                Id = v.Id,
                Name = v.Name,
                Code = v.Code,
                Description = v.Description,
                IsActive = v.IsActive,
                DisplayOrder = v.DisplayOrder
            })
            .ToListAsync(cancellationToken);

        return PagedResult<VehicleTypeDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<VehicleTypeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vehicleType = await _repository.GetByIdAsync(id, cancellationToken);

        if (vehicleType is null)
            return null;

        return new VehicleTypeDto
        {
            Id = vehicleType.Id,
            Name = vehicleType.Name,
            Code = vehicleType.Code,
            Description = vehicleType.Description,
            IsActive = vehicleType.IsActive,
            DisplayOrder = vehicleType.DisplayOrder
        };
    }

    public async Task<VehicleTypeDto> CreateAsync(CreateVehicleTypeRequest request, CancellationToken cancellationToken = default)
    {
        var exists = await _repository.AnyAsync(v => v.Code == request.Code, cancellationToken);

        if (exists)
            throw new ConflictException($"Vehicle type with code '{request.Code}' already exists.");

        var vehicleType = new VehicleType
        {
            Name = request.Name,
            Code = request.Code,
            Description = request.Description,
            IsActive = request.IsActive,
            DisplayOrder = request.DisplayOrder
        };

        await _repository.AddAsync(vehicleType, cancellationToken);

        return new VehicleTypeDto
        {
            Id = vehicleType.Id,
            Name = vehicleType.Name,
            Code = vehicleType.Code,
            Description = vehicleType.Description,
            IsActive = vehicleType.IsActive,
            DisplayOrder = vehicleType.DisplayOrder
        };
    }

    public async Task<VehicleTypeDto> UpdateAsync(Guid id, UpdateVehicleTypeRequest request, CancellationToken cancellationToken = default)
    {
        var vehicleType = await _repository.GetByIdAsync(id, cancellationToken);

        if (vehicleType is null)
            throw new NotFoundException("VehicleType", id);

        var exists = await _repository.AnyAsync(v => v.Code == request.Code && v.Id != id, cancellationToken);

        if (exists)
            throw new ConflictException($"Vehicle type with code '{request.Code}' already exists.");

        vehicleType.Name = request.Name;
        vehicleType.Code = request.Code;
        vehicleType.Description = request.Description;
        vehicleType.IsActive = request.IsActive;
        vehicleType.DisplayOrder = request.DisplayOrder;

        await _repository.UpdateAsync(vehicleType, cancellationToken);

        return new VehicleTypeDto
        {
            Id = vehicleType.Id,
            Name = vehicleType.Name,
            Code = vehicleType.Code,
            Description = vehicleType.Description,
            IsActive = vehicleType.IsActive,
            DisplayOrder = vehicleType.DisplayOrder
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vehicleType = await _repository.GetByIdAsync(id, cancellationToken);

        if (vehicleType is null)
            throw new NotFoundException("VehicleType", id);

        await _repository.DeleteAsync(vehicleType, cancellationToken);
    }
}
