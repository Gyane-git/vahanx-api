using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.VehicleCore;
using VahanX.Domain.Entities;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of vehicle service.
/// </summary>
public class VehicleService : IVehicleService
{
    private readonly IRepository<Vehicle> _repository;
    private readonly IRepository<Variant> _variantRepository;
    private readonly IRepository<BodyType> _bodyTypeRepository;
    private readonly IRepository<FuelType> _fuelTypeRepository;
    private readonly IRepository<TransmissionType> _transmissionTypeRepository;
    private readonly IRepository<VahanX.Domain.Entities.DriveType> _driveTypeRepository;
    private readonly IRepository<EngineType> _engineTypeRepository;

    public VehicleService(
        IRepository<Vehicle> repository,
        IRepository<Variant> variantRepository,
        IRepository<BodyType> bodyTypeRepository,
        IRepository<FuelType> fuelTypeRepository,
        IRepository<TransmissionType> transmissionTypeRepository,
        IRepository<VahanX.Domain.Entities.DriveType> driveTypeRepository,
        IRepository<EngineType> engineTypeRepository)
    {
        _repository = repository;
        _variantRepository = variantRepository;
        _bodyTypeRepository = bodyTypeRepository;
        _fuelTypeRepository = fuelTypeRepository;
        _transmissionTypeRepository = transmissionTypeRepository;
        _driveTypeRepository = driveTypeRepository;
        _engineTypeRepository = engineTypeRepository;
    }

    public async Task<PagedResult<VehicleDto>> GetAllAsync(int page, int pageSize, Guid? variantId = null, Guid? fuelTypeId = null, Guid? bodyTypeId = null, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        if (variantId.HasValue)
            query = query.Where(v => v.VariantId == variantId.Value);

        if (fuelTypeId.HasValue)
            query = query.Where(v => v.FuelTypeId == fuelTypeId.Value);

        if (bodyTypeId.HasValue)
            query = query.Where(v => v.BodyTypeId == bodyTypeId.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(v => v.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(v => new VehicleDto
            {
                Id = v.Id,
                VariantId = v.VariantId,
                VariantName = v.Variant != null ? v.Variant.Name : string.Empty,
                BodyTypeId = v.BodyTypeId,
                BodyTypeName = v.BodyType != null ? v.BodyType.Name : string.Empty,
                FuelTypeId = v.FuelTypeId,
                FuelTypeName = v.FuelType != null ? v.FuelType.Name : string.Empty,
                TransmissionTypeId = v.TransmissionTypeId,
                TransmissionTypeName = v.TransmissionType != null ? v.TransmissionType.Name : string.Empty,
                DriveTypeId = v.DriveTypeId,
                DriveTypeName = v.DriveType != null ? v.DriveType.Name : string.Empty,
                EngineTypeId = v.EngineTypeId,
                EngineTypeName = v.EngineType != null ? v.EngineType.Name : string.Empty,
                Description = v.Description,
                IsActive = v.IsActive
            })
            .ToListAsync(cancellationToken);

        return PagedResult<VehicleDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<VehicleDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vehicle = await _repository.GetByIdAsync(id, cancellationToken);

        if (vehicle is null)
            return null;

        return new VehicleDto
        {
            Id = vehicle.Id,
            VariantId = vehicle.VariantId,
            VariantName = vehicle.Variant != null ? vehicle.Variant.Name : string.Empty,
            BodyTypeId = vehicle.BodyTypeId,
            BodyTypeName = vehicle.BodyType != null ? vehicle.BodyType.Name : string.Empty,
            FuelTypeId = vehicle.FuelTypeId,
            FuelTypeName = vehicle.FuelType != null ? vehicle.FuelType.Name : string.Empty,
            TransmissionTypeId = vehicle.TransmissionTypeId,
            TransmissionTypeName = vehicle.TransmissionType != null ? vehicle.TransmissionType.Name : string.Empty,
            DriveTypeId = vehicle.DriveTypeId,
            DriveTypeName = vehicle.DriveType != null ? vehicle.DriveType.Name : string.Empty,
            EngineTypeId = vehicle.EngineTypeId,
            EngineTypeName = vehicle.EngineType != null ? vehicle.EngineType.Name : string.Empty,
            Description = vehicle.Description,
            IsActive = vehicle.IsActive
        };
    }

    public async Task<VehicleDto> CreateAsync(CreateVehicleRequest request, CancellationToken cancellationToken = default)
    {
        var variantExists = await _variantRepository.AnyAsync(v => v.Id == request.VariantId, cancellationToken);
        if (!variantExists) throw new NotFoundException("Variant", request.VariantId);

        var bodyTypeExists = await _bodyTypeRepository.AnyAsync(b => b.Id == request.BodyTypeId, cancellationToken);
        if (!bodyTypeExists) throw new NotFoundException("BodyType", request.BodyTypeId);

        var fuelTypeExists = await _fuelTypeRepository.AnyAsync(f => f.Id == request.FuelTypeId, cancellationToken);
        if (!fuelTypeExists) throw new NotFoundException("FuelType", request.FuelTypeId);

        var transmissionTypeExists = await _transmissionTypeRepository.AnyAsync(t => t.Id == request.TransmissionTypeId, cancellationToken);
        if (!transmissionTypeExists) throw new NotFoundException("TransmissionType", request.TransmissionTypeId);

        var driveTypeExists = await _driveTypeRepository.AnyAsync(d => d.Id == request.DriveTypeId, cancellationToken);
        if (!driveTypeExists) throw new NotFoundException("DriveType", request.DriveTypeId);

        var engineTypeExists = await _engineTypeRepository.AnyAsync(e => e.Id == request.EngineTypeId, cancellationToken);
        if (!engineTypeExists) throw new NotFoundException("EngineType", request.EngineTypeId);

        var vehicle = new Vehicle
        {
            VariantId = request.VariantId,
            BodyTypeId = request.BodyTypeId,
            FuelTypeId = request.FuelTypeId,
            TransmissionTypeId = request.TransmissionTypeId,
            DriveTypeId = request.DriveTypeId,
            EngineTypeId = request.EngineTypeId,
            Description = request.Description,
            IsActive = request.IsActive
        };

        await _repository.AddAsync(vehicle, cancellationToken);

        return new VehicleDto
        {
            Id = vehicle.Id,
            VariantId = vehicle.VariantId,
            VariantName = string.Empty,
            BodyTypeId = vehicle.BodyTypeId,
            BodyTypeName = string.Empty,
            FuelTypeId = vehicle.FuelTypeId,
            FuelTypeName = string.Empty,
            TransmissionTypeId = vehicle.TransmissionTypeId,
            TransmissionTypeName = string.Empty,
            DriveTypeId = vehicle.DriveTypeId,
            DriveTypeName = string.Empty,
            EngineTypeId = vehicle.EngineTypeId,
            EngineTypeName = string.Empty,
            Description = vehicle.Description,
            IsActive = vehicle.IsActive
        };
    }

    public async Task<VehicleDto> UpdateAsync(Guid id, UpdateVehicleRequest request, CancellationToken cancellationToken = default)
    {
        var vehicle = await _repository.GetByIdAsync(id, cancellationToken);
        if (vehicle is null) throw new NotFoundException("Vehicle", id);

        var bodyTypeExists = await _bodyTypeRepository.AnyAsync(b => b.Id == request.BodyTypeId, cancellationToken);
        if (!bodyTypeExists) throw new NotFoundException("BodyType", request.BodyTypeId);

        var fuelTypeExists = await _fuelTypeRepository.AnyAsync(f => f.Id == request.FuelTypeId, cancellationToken);
        if (!fuelTypeExists) throw new NotFoundException("FuelType", request.FuelTypeId);

        var transmissionTypeExists = await _transmissionTypeRepository.AnyAsync(t => t.Id == request.TransmissionTypeId, cancellationToken);
        if (!transmissionTypeExists) throw new NotFoundException("TransmissionType", request.TransmissionTypeId);

        var driveTypeExists = await _driveTypeRepository.AnyAsync(d => d.Id == request.DriveTypeId, cancellationToken);
        if (!driveTypeExists) throw new NotFoundException("DriveType", request.DriveTypeId);

        var engineTypeExists = await _engineTypeRepository.AnyAsync(e => e.Id == request.EngineTypeId, cancellationToken);
        if (!engineTypeExists) throw new NotFoundException("EngineType", request.EngineTypeId);

        vehicle.BodyTypeId = request.BodyTypeId;
        vehicle.FuelTypeId = request.FuelTypeId;
        vehicle.TransmissionTypeId = request.TransmissionTypeId;
        vehicle.DriveTypeId = request.DriveTypeId;
        vehicle.EngineTypeId = request.EngineTypeId;
        vehicle.Description = request.Description;
        vehicle.IsActive = request.IsActive;

        await _repository.UpdateAsync(vehicle, cancellationToken);

        return new VehicleDto
        {
            Id = vehicle.Id,
            VariantId = vehicle.VariantId,
            VariantName = string.Empty,
            BodyTypeId = vehicle.BodyTypeId,
            BodyTypeName = string.Empty,
            FuelTypeId = vehicle.FuelTypeId,
            FuelTypeName = string.Empty,
            TransmissionTypeId = vehicle.TransmissionTypeId,
            TransmissionTypeName = string.Empty,
            DriveTypeId = vehicle.DriveTypeId,
            DriveTypeName = string.Empty,
            EngineTypeId = vehicle.EngineTypeId,
            EngineTypeName = string.Empty,
            Description = vehicle.Description,
            IsActive = vehicle.IsActive
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vehicle = await _repository.GetByIdAsync(id, cancellationToken);
        if (vehicle is null) throw new NotFoundException("Vehicle", id);

        await _repository.DeleteAsync(vehicle, cancellationToken);
    }
}
