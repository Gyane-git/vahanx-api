using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.VehicleCore;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of master data service.
/// </summary>
public class MasterDataService : IMasterDataService
{
    private readonly IRepository<BodyType> _bodyTypeRepository;
    private readonly IRepository<FuelType> _fuelTypeRepository;
    private readonly IRepository<TransmissionType> _transmissionTypeRepository;
    private readonly IRepository<VahanX.Domain.Entities.DriveType> _driveTypeRepository;
    private readonly IRepository<EngineType> _engineTypeRepository;
    private readonly IRepository<VehicleFeature> _vehicleFeatureRepository;
    private readonly IRepository<VehicleSpecification> _vehicleSpecificationRepository;

    public MasterDataService(
        IRepository<BodyType> bodyTypeRepository,
        IRepository<FuelType> fuelTypeRepository,
        IRepository<TransmissionType> transmissionTypeRepository,
        IRepository<VahanX.Domain.Entities.DriveType> driveTypeRepository,
        IRepository<EngineType> engineTypeRepository,
        IRepository<VehicleFeature> vehicleFeatureRepository,
        IRepository<VehicleSpecification> vehicleSpecificationRepository)
    {
        _bodyTypeRepository = bodyTypeRepository;
        _fuelTypeRepository = fuelTypeRepository;
        _transmissionTypeRepository = transmissionTypeRepository;
        _driveTypeRepository = driveTypeRepository;
        _engineTypeRepository = engineTypeRepository;
        _vehicleFeatureRepository = vehicleFeatureRepository;
        _vehicleSpecificationRepository = vehicleSpecificationRepository;
    }

    // BodyType
    public async Task<PagedResult<BodyTypeDto>> GetBodyTypesAsync(int page, int pageSize, string? search = null, CancellationToken cancellationToken = default)
    {
        var query = await _bodyTypeRepository.QueryAsync(true, cancellationToken);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(b => b.Name.Contains(search) || b.Code.Contains(search));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(b => b.DisplayOrder).ThenBy(b => b.Name)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(b => new BodyTypeDto { Id = b.Id, Name = b.Name, Code = b.Code, Description = b.Description, IsActive = b.IsActive, DisplayOrder = b.DisplayOrder })
            .ToListAsync(cancellationToken);
        return PagedResult<BodyTypeDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<BodyTypeDto?> GetBodyTypeByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _bodyTypeRepository.GetByIdAsync(id, cancellationToken);
        return item is null ? null : new BodyTypeDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task<BodyTypeDto> CreateBodyTypeAsync(CreateBodyTypeRequest request, CancellationToken cancellationToken = default)
    {
        if (await _bodyTypeRepository.AnyAsync(b => b.Code == request.Code, cancellationToken))
            throw new ConflictException($"Body type with code '{request.Code}' already exists.");

        var item = new BodyType { Name = request.Name, Code = request.Code, Description = request.Description, IsActive = request.IsActive, DisplayOrder = request.DisplayOrder };
        await _bodyTypeRepository.AddAsync(item, cancellationToken);
        return new BodyTypeDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task<BodyTypeDto> UpdateBodyTypeAsync(Guid id, UpdateBodyTypeRequest request, CancellationToken cancellationToken = default)
    {
        var item = await _bodyTypeRepository.GetByIdAsync(id, cancellationToken);
        if (item is null) throw new NotFoundException("BodyType", id);
        if (await _bodyTypeRepository.AnyAsync(b => b.Code == request.Code && b.Id != id, cancellationToken))
            throw new ConflictException($"Body type with code '{request.Code}' already exists.");

        item.Name = request.Name; item.Code = request.Code; item.Description = request.Description; item.IsActive = request.IsActive; item.DisplayOrder = request.DisplayOrder;
        await _bodyTypeRepository.UpdateAsync(item, cancellationToken);
        return new BodyTypeDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task DeleteBodyTypeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _bodyTypeRepository.GetByIdAsync(id, cancellationToken);
        if (item is null) throw new NotFoundException("BodyType", id);
        await _bodyTypeRepository.DeleteAsync(item, cancellationToken);
    }

    // FuelType
    public async Task<PagedResult<FuelTypeDto>> GetFuelTypesAsync(int page, int pageSize, string? search = null, CancellationToken cancellationToken = default)
    {
        var query = await _fuelTypeRepository.QueryAsync(true, cancellationToken);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(f => f.Name.Contains(search) || f.Code.Contains(search));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(f => f.DisplayOrder).ThenBy(f => f.Name)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(f => new FuelTypeDto { Id = f.Id, Name = f.Name, Code = f.Code, Description = f.Description, IsActive = f.IsActive, DisplayOrder = f.DisplayOrder })
            .ToListAsync(cancellationToken);
        return PagedResult<FuelTypeDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<FuelTypeDto?> GetFuelTypeByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _fuelTypeRepository.GetByIdAsync(id, cancellationToken);
        return item is null ? null : new FuelTypeDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task<FuelTypeDto> CreateFuelTypeAsync(CreateFuelTypeRequest request, CancellationToken cancellationToken = default)
    {
        if (await _fuelTypeRepository.AnyAsync(f => f.Code == request.Code, cancellationToken))
            throw new ConflictException($"Fuel type with code '{request.Code}' already exists.");

        var item = new FuelType { Name = request.Name, Code = request.Code, Description = request.Description, IsActive = request.IsActive, DisplayOrder = request.DisplayOrder };
        await _fuelTypeRepository.AddAsync(item, cancellationToken);
        return new FuelTypeDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task<FuelTypeDto> UpdateFuelTypeAsync(Guid id, UpdateFuelTypeRequest request, CancellationToken cancellationToken = default)
    {
        var item = await _fuelTypeRepository.GetByIdAsync(id, cancellationToken);
        if (item is null) throw new NotFoundException("FuelType", id);
        if (await _fuelTypeRepository.AnyAsync(f => f.Code == request.Code && f.Id != id, cancellationToken))
            throw new ConflictException($"Fuel type with code '{request.Code}' already exists.");

        item.Name = request.Name; item.Code = request.Code; item.Description = request.Description; item.IsActive = request.IsActive; item.DisplayOrder = request.DisplayOrder;
        await _fuelTypeRepository.UpdateAsync(item, cancellationToken);
        return new FuelTypeDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task DeleteFuelTypeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _fuelTypeRepository.GetByIdAsync(id, cancellationToken);
        if (item is null) throw new NotFoundException("FuelType", id);
        await _fuelTypeRepository.DeleteAsync(item, cancellationToken);
    }

    // TransmissionType
    public async Task<PagedResult<TransmissionTypeDto>> GetTransmissionTypesAsync(int page, int pageSize, string? search = null, CancellationToken cancellationToken = default)
    {
        var query = await _transmissionTypeRepository.QueryAsync(true, cancellationToken);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(t => t.Name.Contains(search) || t.Code.Contains(search));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(t => t.DisplayOrder).ThenBy(t => t.Name)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(t => new TransmissionTypeDto { Id = t.Id, Name = t.Name, Code = t.Code, Description = t.Description, IsActive = t.IsActive, DisplayOrder = t.DisplayOrder })
            .ToListAsync(cancellationToken);
        return PagedResult<TransmissionTypeDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<TransmissionTypeDto?> GetTransmissionTypeByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _transmissionTypeRepository.GetByIdAsync(id, cancellationToken);
        return item is null ? null : new TransmissionTypeDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task<TransmissionTypeDto> CreateTransmissionTypeAsync(CreateTransmissionTypeRequest request, CancellationToken cancellationToken = default)
    {
        if (await _transmissionTypeRepository.AnyAsync(t => t.Code == request.Code, cancellationToken))
            throw new ConflictException($"Transmission type with code '{request.Code}' already exists.");

        var item = new TransmissionType { Name = request.Name, Code = request.Code, Description = request.Description, IsActive = request.IsActive, DisplayOrder = request.DisplayOrder };
        await _transmissionTypeRepository.AddAsync(item, cancellationToken);
        return new TransmissionTypeDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task<TransmissionTypeDto> UpdateTransmissionTypeAsync(Guid id, UpdateTransmissionTypeRequest request, CancellationToken cancellationToken = default)
    {
        var item = await _transmissionTypeRepository.GetByIdAsync(id, cancellationToken);
        if (item is null) throw new NotFoundException("TransmissionType", id);
        if (await _transmissionTypeRepository.AnyAsync(t => t.Code == request.Code && t.Id != id, cancellationToken))
            throw new ConflictException($"Transmission type with code '{request.Code}' already exists.");

        item.Name = request.Name; item.Code = request.Code; item.Description = request.Description; item.IsActive = request.IsActive; item.DisplayOrder = request.DisplayOrder;
        await _transmissionTypeRepository.UpdateAsync(item, cancellationToken);
        return new TransmissionTypeDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task DeleteTransmissionTypeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _transmissionTypeRepository.GetByIdAsync(id, cancellationToken);
        if (item is null) throw new NotFoundException("TransmissionType", id);
        await _transmissionTypeRepository.DeleteAsync(item, cancellationToken);
    }

    // DriveType
    public async Task<PagedResult<DriveTypeDto>> GetDriveTypesAsync(int page, int pageSize, string? search = null, CancellationToken cancellationToken = default)
    {
        var query = await _driveTypeRepository.QueryAsync(true, cancellationToken);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(d => d.Name.Contains(search) || d.Code.Contains(search));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(d => d.DisplayOrder).ThenBy(d => d.Name)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(d => new DriveTypeDto { Id = d.Id, Name = d.Name, Code = d.Code, Description = d.Description, IsActive = d.IsActive, DisplayOrder = d.DisplayOrder })
            .ToListAsync(cancellationToken);
        return PagedResult<DriveTypeDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<DriveTypeDto?> GetDriveTypeByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _driveTypeRepository.GetByIdAsync(id, cancellationToken);
        return item is null ? null : new DriveTypeDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task<DriveTypeDto> CreateDriveTypeAsync(CreateDriveTypeRequest request, CancellationToken cancellationToken = default)
    {
        if (await _driveTypeRepository.AnyAsync(d => d.Code == request.Code, cancellationToken))
            throw new ConflictException($"Drive type with code '{request.Code}' already exists.");

        var item = new VahanX.Domain.Entities.DriveType { Name = request.Name, Code = request.Code, Description = request.Description, IsActive = request.IsActive, DisplayOrder = request.DisplayOrder };
        await _driveTypeRepository.AddAsync(item, cancellationToken);
        return new DriveTypeDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task<DriveTypeDto> UpdateDriveTypeAsync(Guid id, UpdateDriveTypeRequest request, CancellationToken cancellationToken = default)
    {
        var item = await _driveTypeRepository.GetByIdAsync(id, cancellationToken);
        if (item is null) throw new NotFoundException("DriveType", id);
        if (await _driveTypeRepository.AnyAsync(d => d.Code == request.Code && d.Id != id, cancellationToken))
            throw new ConflictException($"Drive type with code '{request.Code}' already exists.");

        item.Name = request.Name; item.Code = request.Code; item.Description = request.Description; item.IsActive = request.IsActive; item.DisplayOrder = request.DisplayOrder;
        await _driveTypeRepository.UpdateAsync(item, cancellationToken);
        return new DriveTypeDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task DeleteDriveTypeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _driveTypeRepository.GetByIdAsync(id, cancellationToken);
        if (item is null) throw new NotFoundException("DriveType", id);
        await _driveTypeRepository.DeleteAsync(item, cancellationToken);
    }

    // EngineType
    public async Task<PagedResult<EngineTypeDto>> GetEngineTypesAsync(int page, int pageSize, string? search = null, CancellationToken cancellationToken = default)
    {
        var query = await _engineTypeRepository.QueryAsync(true, cancellationToken);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(e => e.DisplayOrder).ThenBy(e => e.Name)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(e => new EngineTypeDto { Id = e.Id, Name = e.Name, Code = e.Code, Description = e.Description, IsActive = e.IsActive, DisplayOrder = e.DisplayOrder })
            .ToListAsync(cancellationToken);
        return PagedResult<EngineTypeDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<EngineTypeDto?> GetEngineTypeByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _engineTypeRepository.GetByIdAsync(id, cancellationToken);
        return item is null ? null : new EngineTypeDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task<EngineTypeDto> CreateEngineTypeAsync(CreateEngineTypeRequest request, CancellationToken cancellationToken = default)
    {
        if (await _engineTypeRepository.AnyAsync(e => e.Code == request.Code, cancellationToken))
            throw new ConflictException($"Engine type with code '{request.Code}' already exists.");

        var item = new EngineType { Name = request.Name, Code = request.Code, Description = request.Description, IsActive = request.IsActive, DisplayOrder = request.DisplayOrder };
        await _engineTypeRepository.AddAsync(item, cancellationToken);
        return new EngineTypeDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task<EngineTypeDto> UpdateEngineTypeAsync(Guid id, UpdateEngineTypeRequest request, CancellationToken cancellationToken = default)
    {
        var item = await _engineTypeRepository.GetByIdAsync(id, cancellationToken);
        if (item is null) throw new NotFoundException("EngineType", id);
        if (await _engineTypeRepository.AnyAsync(e => e.Code == request.Code && e.Id != id, cancellationToken))
            throw new ConflictException($"Engine type with code '{request.Code}' already exists.");

        item.Name = request.Name; item.Code = request.Code; item.Description = request.Description; item.IsActive = request.IsActive; item.DisplayOrder = request.DisplayOrder;
        await _engineTypeRepository.UpdateAsync(item, cancellationToken);
        return new EngineTypeDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task DeleteEngineTypeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _engineTypeRepository.GetByIdAsync(id, cancellationToken);
        if (item is null) throw new NotFoundException("EngineType", id);
        await _engineTypeRepository.DeleteAsync(item, cancellationToken);
    }

    // VehicleFeature
    public async Task<PagedResult<VehicleFeatureDto>> GetVehicleFeaturesAsync(int page, int pageSize, string? search = null, string? featureGroup = null, CancellationToken cancellationToken = default)
    {
        var query = await _vehicleFeatureRepository.QueryAsync(true, cancellationToken);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(f => f.Name.Contains(search) || f.Code.Contains(search));
        if (!string.IsNullOrWhiteSpace(featureGroup))
            query = query.Where(f => f.FeatureGroup == featureGroup);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(f => f.DisplayOrder).ThenBy(f => f.Name)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(f => new VehicleFeatureDto { Id = f.Id, Name = f.Name, Code = f.Code, Description = f.Description, FeatureGroup = f.FeatureGroup, IsActive = f.IsActive, DisplayOrder = f.DisplayOrder })
            .ToListAsync(cancellationToken);
        return PagedResult<VehicleFeatureDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<VehicleFeatureDto?> GetVehicleFeatureByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _vehicleFeatureRepository.GetByIdAsync(id, cancellationToken);
        return item is null ? null : new VehicleFeatureDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, FeatureGroup = item.FeatureGroup, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task<VehicleFeatureDto> CreateVehicleFeatureAsync(CreateVehicleFeatureRequest request, CancellationToken cancellationToken = default)
    {
        if (await _vehicleFeatureRepository.AnyAsync(f => f.Code == request.Code, cancellationToken))
            throw new ConflictException($"Vehicle feature with code '{request.Code}' already exists.");

        var item = new VehicleFeature { Name = request.Name, Code = request.Code, Description = request.Description, FeatureGroup = request.FeatureGroup, IsActive = request.IsActive, DisplayOrder = request.DisplayOrder };
        await _vehicleFeatureRepository.AddAsync(item, cancellationToken);
        return new VehicleFeatureDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, FeatureGroup = item.FeatureGroup, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task<VehicleFeatureDto> UpdateVehicleFeatureAsync(Guid id, UpdateVehicleFeatureRequest request, CancellationToken cancellationToken = default)
    {
        var item = await _vehicleFeatureRepository.GetByIdAsync(id, cancellationToken);
        if (item is null) throw new NotFoundException("VehicleFeature", id);
        if (await _vehicleFeatureRepository.AnyAsync(f => f.Code == request.Code && f.Id != id, cancellationToken))
            throw new ConflictException($"Vehicle feature with code '{request.Code}' already exists.");

        item.Name = request.Name; item.Code = request.Code; item.Description = request.Description; item.FeatureGroup = request.FeatureGroup; item.IsActive = request.IsActive; item.DisplayOrder = request.DisplayOrder;
        await _vehicleFeatureRepository.UpdateAsync(item, cancellationToken);
        return new VehicleFeatureDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, FeatureGroup = item.FeatureGroup, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task DeleteVehicleFeatureAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _vehicleFeatureRepository.GetByIdAsync(id, cancellationToken);
        if (item is null) throw new NotFoundException("VehicleFeature", id);
        await _vehicleFeatureRepository.DeleteAsync(item, cancellationToken);
    }

    // VehicleSpecification
    public async Task<PagedResult<VehicleSpecificationDto>> GetVehicleSpecificationsAsync(int page, int pageSize, string? search = null, string? specGroup = null, CancellationToken cancellationToken = default)
    {
        var query = await _vehicleSpecificationRepository.QueryAsync(true, cancellationToken);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(s => s.Name.Contains(search) || s.Code.Contains(search));
        if (!string.IsNullOrWhiteSpace(specGroup))
            query = query.Where(s => s.SpecGroup == specGroup);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(s => new VehicleSpecificationDto { Id = s.Id, Name = s.Name, Code = s.Code, Description = s.Description, Unit = s.Unit, DataType = s.DataType, SpecGroup = s.SpecGroup, IsActive = s.IsActive, DisplayOrder = s.DisplayOrder })
            .ToListAsync(cancellationToken);
        return PagedResult<VehicleSpecificationDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<VehicleSpecificationDto?> GetVehicleSpecificationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _vehicleSpecificationRepository.GetByIdAsync(id, cancellationToken);
        return item is null ? null : new VehicleSpecificationDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, Unit = item.Unit, DataType = item.DataType, SpecGroup = item.SpecGroup, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task<VehicleSpecificationDto> CreateVehicleSpecificationAsync(CreateVehicleSpecificationRequest request, CancellationToken cancellationToken = default)
    {
        if (await _vehicleSpecificationRepository.AnyAsync(s => s.Code == request.Code, cancellationToken))
            throw new ConflictException($"Vehicle specification with code '{request.Code}' already exists.");

        var item = new VehicleSpecification { Name = request.Name, Code = request.Code, Description = request.Description, Unit = request.Unit, DataType = request.DataType, SpecGroup = request.SpecGroup, IsActive = request.IsActive, DisplayOrder = request.DisplayOrder };
        await _vehicleSpecificationRepository.AddAsync(item, cancellationToken);
        return new VehicleSpecificationDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, Unit = item.Unit, DataType = item.DataType, SpecGroup = item.SpecGroup, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task<VehicleSpecificationDto> UpdateVehicleSpecificationAsync(Guid id, UpdateVehicleSpecificationRequest request, CancellationToken cancellationToken = default)
    {
        var item = await _vehicleSpecificationRepository.GetByIdAsync(id, cancellationToken);
        if (item is null) throw new NotFoundException("VehicleSpecification", id);
        if (await _vehicleSpecificationRepository.AnyAsync(s => s.Code == request.Code && s.Id != id, cancellationToken))
            throw new ConflictException($"Vehicle specification with code '{request.Code}' already exists.");

        item.Name = request.Name; item.Code = request.Code; item.Description = request.Description; item.Unit = request.Unit; item.DataType = request.DataType; item.SpecGroup = request.SpecGroup; item.IsActive = request.IsActive; item.DisplayOrder = request.DisplayOrder;
        await _vehicleSpecificationRepository.UpdateAsync(item, cancellationToken);
        return new VehicleSpecificationDto { Id = item.Id, Name = item.Name, Code = item.Code, Description = item.Description, Unit = item.Unit, DataType = item.DataType, SpecGroup = item.SpecGroup, IsActive = item.IsActive, DisplayOrder = item.DisplayOrder };
    }

    public async Task DeleteVehicleSpecificationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _vehicleSpecificationRepository.GetByIdAsync(id, cancellationToken);
        if (item is null) throw new NotFoundException("VehicleSpecification", id);
        await _vehicleSpecificationRepository.DeleteAsync(item, cancellationToken);
    }
}
