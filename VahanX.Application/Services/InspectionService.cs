using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Trust;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of inspection service.
/// </summary>
public class InspectionService : IInspectionService
{
    private readonly IRepository<Inspection> _repository;
    private readonly IRepository<InspectionItem> _itemRepository;
    private readonly IRepository<Vehicle> _vehicleRepository;
    private readonly IRepository<VehicleListing> _listingRepository;

    public InspectionService(
        IRepository<Inspection> repository,
        IRepository<InspectionItem> itemRepository,
        IRepository<Vehicle> vehicleRepository,
        IRepository<VehicleListing> listingRepository)
    {
        _repository = repository;
        _itemRepository = itemRepository;
        _vehicleRepository = vehicleRepository;
        _listingRepository = listingRepository;
    }

    public async Task<PagedResult<InspectionResponse>> GetAllAsync(int page, int pageSize, Guid? vehicleId = null, Guid? listingId = null, InspectionStatus? status = null, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        if (vehicleId.HasValue)
            query = query.Where(i => i.VehicleId == vehicleId.Value);

        if (listingId.HasValue)
            query = query.Where(i => i.ListingId == listingId.Value);

        if (status.HasValue)
            query = query.Where(i => i.Status == status.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(i => new InspectionResponse
            {
                Id = i.Id,
                VehicleId = i.VehicleId,
                ListingId = i.ListingId,
                InspectionType = i.InspectionType,
                Status = i.Status,
                InspectorUserId = i.InspectorUserId,
                InspectionDate = i.InspectionDate,
                CompletedAt = i.CompletedAt,
                OverallScore = i.OverallScore,
                Summary = i.Summary,
                Notes = i.Notes,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<InspectionResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<InspectionResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var inspection = await _repository.GetByIdAsync(id, cancellationToken);

        if (inspection is null)
            return null;

        return new InspectionResponse
        {
            Id = inspection.Id,
            VehicleId = inspection.VehicleId,
            ListingId = inspection.ListingId,
            InspectionType = inspection.InspectionType,
            Status = inspection.Status,
            InspectorUserId = inspection.InspectorUserId,
            InspectionDate = inspection.InspectionDate,
            CompletedAt = inspection.CompletedAt,
            OverallScore = inspection.OverallScore,
            Summary = inspection.Summary,
            Notes = inspection.Notes,
            CreatedAt = inspection.CreatedAt,
            UpdatedAt = inspection.UpdatedAt
        };
    }

    public async Task<InspectionResponse> CreateAsync(CreateInspectionRequest request, CancellationToken cancellationToken = default)
    {
        var vehicleExists = await _vehicleRepository.AnyAsync(v => v.Id == request.VehicleId, cancellationToken);
        if (!vehicleExists) throw new NotFoundException("Vehicle", request.VehicleId);

        if (request.ListingId.HasValue)
        {
            var listingExists = await _listingRepository.AnyAsync(l => l.Id == request.ListingId.Value, cancellationToken);
            if (!listingExists) throw new NotFoundException("Listing", request.ListingId.Value);
        }

        var inspection = new Inspection
        {
            VehicleId = request.VehicleId,
            ListingId = request.ListingId,
            InspectionType = request.InspectionType,
            Status = InspectionStatus.Scheduled,
            InspectionDate = request.InspectionDate
        };

        await _repository.AddAsync(inspection, cancellationToken);

        return new InspectionResponse
        {
            Id = inspection.Id,
            VehicleId = inspection.VehicleId,
            ListingId = inspection.ListingId,
            InspectionType = inspection.InspectionType,
            Status = inspection.Status,
            InspectorUserId = inspection.InspectorUserId,
            InspectionDate = inspection.InspectionDate,
            CompletedAt = inspection.CompletedAt,
            OverallScore = inspection.OverallScore,
            Summary = inspection.Summary,
            Notes = inspection.Notes,
            CreatedAt = inspection.CreatedAt,
            UpdatedAt = inspection.UpdatedAt
        };
    }

    public async Task<InspectionResponse> UpdateAsync(Guid id, UpdateInspectionRequest request, CancellationToken cancellationToken = default)
    {
        var inspection = await _repository.GetByIdAsync(id, cancellationToken);
        if (inspection is null) throw new NotFoundException("Inspection", id);

        if (inspection.Status == InspectionStatus.Completed || inspection.Status == InspectionStatus.Cancelled)
            throw new ConflictException($"Cannot update an inspection with status {inspection.Status}.");

        if (request.InspectionType.HasValue)
            inspection.InspectionType = request.InspectionType.Value;
        if (request.Summary != null)
            inspection.Summary = request.Summary;
        if (request.Notes != null)
            inspection.Notes = request.Notes;

        await _repository.UpdateAsync(inspection, cancellationToken);

        return new InspectionResponse
        {
            Id = inspection.Id,
            VehicleId = inspection.VehicleId,
            ListingId = inspection.ListingId,
            InspectionType = inspection.InspectionType,
            Status = inspection.Status,
            InspectorUserId = inspection.InspectorUserId,
            InspectionDate = inspection.InspectionDate,
            CompletedAt = inspection.CompletedAt,
            OverallScore = inspection.OverallScore,
            Summary = inspection.Summary,
            Notes = inspection.Notes,
            CreatedAt = inspection.CreatedAt,
            UpdatedAt = inspection.UpdatedAt
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var inspection = await _repository.GetByIdAsync(id, cancellationToken);
        if (inspection is null) throw new NotFoundException("Inspection", id);

        await _repository.DeleteAsync(inspection, cancellationToken);
    }

    public async Task<InspectionResponse> StartAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var inspection = await _repository.GetByIdAsync(id, cancellationToken);
        if (inspection is null) throw new NotFoundException("Inspection", id);

        if (inspection.Status != InspectionStatus.Scheduled)
            throw new ConflictException($"Cannot start an inspection with status {inspection.Status}.");

        inspection.Status = InspectionStatus.InProgress;
        await _repository.UpdateAsync(inspection, cancellationToken);

        return new InspectionResponse
        {
            Id = inspection.Id,
            VehicleId = inspection.VehicleId,
            ListingId = inspection.ListingId,
            InspectionType = inspection.InspectionType,
            Status = inspection.Status,
            InspectorUserId = inspection.InspectorUserId,
            InspectionDate = inspection.InspectionDate,
            CompletedAt = inspection.CompletedAt,
            OverallScore = inspection.OverallScore,
            Summary = inspection.Summary,
            Notes = inspection.Notes,
            CreatedAt = inspection.CreatedAt,
            UpdatedAt = inspection.UpdatedAt
        };
    }

    public async Task<InspectionResponse> CompleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var inspection = await _repository.GetByIdAsync(id, cancellationToken);
        if (inspection is null) throw new NotFoundException("Inspection", id);

        if (inspection.Status != InspectionStatus.InProgress)
            throw new ConflictException($"Cannot complete an inspection with status {inspection.Status}.");

        inspection.Status = InspectionStatus.Completed;
        inspection.CompletedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(inspection, cancellationToken);

        return new InspectionResponse
        {
            Id = inspection.Id,
            VehicleId = inspection.VehicleId,
            ListingId = inspection.ListingId,
            InspectionType = inspection.InspectionType,
            Status = inspection.Status,
            InspectorUserId = inspection.InspectorUserId,
            InspectionDate = inspection.InspectionDate,
            CompletedAt = inspection.CompletedAt,
            OverallScore = inspection.OverallScore,
            Summary = inspection.Summary,
            Notes = inspection.Notes,
            CreatedAt = inspection.CreatedAt,
            UpdatedAt = inspection.UpdatedAt
        };
    }

    public async Task<InspectionResponse> CancelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var inspection = await _repository.GetByIdAsync(id, cancellationToken);
        if (inspection is null) throw new NotFoundException("Inspection", id);

        if (inspection.Status == InspectionStatus.Completed || inspection.Status == InspectionStatus.Cancelled)
            throw new ConflictException($"Cannot cancel an inspection with status {inspection.Status}.");

        inspection.Status = InspectionStatus.Cancelled;
        await _repository.UpdateAsync(inspection, cancellationToken);

        return new InspectionResponse
        {
            Id = inspection.Id,
            VehicleId = inspection.VehicleId,
            ListingId = inspection.ListingId,
            InspectionType = inspection.InspectionType,
            Status = inspection.Status,
            InspectorUserId = inspection.InspectorUserId,
            InspectionDate = inspection.InspectionDate,
            CompletedAt = inspection.CompletedAt,
            OverallScore = inspection.OverallScore,
            Summary = inspection.Summary,
            Notes = inspection.Notes,
            CreatedAt = inspection.CreatedAt,
            UpdatedAt = inspection.UpdatedAt
        };
    }

    public async Task<InspectionItemResponse> AddItemAsync(Guid inspectionId, CreateInspectionItemRequest request, CancellationToken cancellationToken = default)
    {
        var inspectionExists = await _repository.AnyAsync(i => i.Id == inspectionId, cancellationToken);
        if (!inspectionExists) throw new NotFoundException("Inspection", inspectionId);

        var item = new InspectionItem
        {
            InspectionId = inspectionId,
            Category = request.Category,
            ItemName = request.ItemName,
            Condition = request.Condition,
            Score = request.Score,
            Notes = request.Notes
        };

        await _itemRepository.AddAsync(item, cancellationToken);

        return new InspectionItemResponse
        {
            Id = item.Id,
            InspectionId = item.InspectionId,
            Category = item.Category,
            ItemName = item.ItemName,
            Condition = item.Condition,
            Score = item.Score,
            Notes = item.Notes
        };
    }

    public async Task<InspectionItemResponse> UpdateItemAsync(Guid inspectionId, Guid itemId, UpdateInspectionItemRequest request, CancellationToken cancellationToken = default)
    {
        var item = await _itemRepository.GetByIdAsync(itemId, cancellationToken);
        if (item is null) throw new NotFoundException("InspectionItem", itemId);

        if (item.InspectionId != inspectionId)
            throw new NotFoundException("InspectionItem", itemId);

        if (request.Condition.HasValue)
            item.Condition = request.Condition.Value;
        if (request.Score.HasValue)
            item.Score = request.Score;
        if (request.Notes != null)
            item.Notes = request.Notes;

        await _itemRepository.UpdateAsync(item, cancellationToken);

        return new InspectionItemResponse
        {
            Id = item.Id,
            InspectionId = item.InspectionId,
            Category = item.Category,
            ItemName = item.ItemName,
            Condition = item.Condition,
            Score = item.Score,
            Notes = item.Notes
        };
    }

    public async Task DeleteItemAsync(Guid inspectionId, Guid itemId, CancellationToken cancellationToken = default)
    {
        var item = await _itemRepository.GetByIdAsync(itemId, cancellationToken);
        if (item is null) throw new NotFoundException("InspectionItem", itemId);

        if (item.InspectionId != inspectionId)
            throw new NotFoundException("InspectionItem", itemId);

        await _itemRepository.DeleteAsync(item, cancellationToken);
    }

    public async Task<PagedResult<InspectionItemResponse>> GetItemsAsync(Guid inspectionId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _itemRepository.QueryAsync(true, cancellationToken);

        var itemsQuery = query
            .Where(i => i.InspectionId == inspectionId)
            .OrderBy(i => i.Category);

        var totalCount = await itemsQuery.CountAsync(cancellationToken);
        var items = await itemsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(i => new InspectionItemResponse
            {
                Id = i.Id,
                InspectionId = i.InspectionId,
                Category = i.Category,
                ItemName = i.ItemName,
                Condition = i.Condition,
                Score = i.Score,
                Notes = i.Notes
            })
            .ToListAsync(cancellationToken);

        return PagedResult<InspectionItemResponse>.Create(items, totalCount, page, pageSize);
    }
}
