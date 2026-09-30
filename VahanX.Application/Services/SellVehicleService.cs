using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Business;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of sell vehicle service.
/// </summary>
public class SellVehicleService : ISellVehicleService
{
    private readonly IRepository<SellRequest> _requestRepository;
    private readonly IRepository<SellVehicle> _vehicleRepository;
    private readonly IRepository<SellOffer> _offerRepository;
    private readonly IRepository<SellRequestStatusHistory> _historyRepository;

    public SellVehicleService(
        IRepository<SellRequest> requestRepository,
        IRepository<SellVehicle> vehicleRepository,
        IRepository<SellOffer> offerRepository,
        IRepository<SellRequestStatusHistory> historyRepository)
    {
        _requestRepository = requestRepository;
        _vehicleRepository = vehicleRepository;
        _offerRepository = offerRepository;
        _historyRepository = historyRepository;
    }

    public async Task<PagedResult<SellRequestResponse>> GetRequestsAsync(int page, int pageSize, Guid? userId = null, SellRequestStatus? status = null, CancellationToken cancellationToken = default)
    {
        var query = await _requestRepository.QueryAsync(true, cancellationToken);

        if (userId.HasValue)
            query = query.Where(r => r.UserId == userId.Value);
        if (status.HasValue)
            query = query.Where(r => r.Status == status.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new SellRequestResponse
            {
                Id = r.Id,
                UserId = r.UserId,
                SellVehicleId = r.SellVehicleId,
                VehicleDisplayName = r.SellVehicle != null ? $"{(r.SellVehicle.Brand != null ? r.SellVehicle.Brand.Name : string.Empty)} {(r.SellVehicle.Model != null ? r.SellVehicle.Model.Name : string.Empty)}" : string.Empty,
                PreferredContactMethod = r.PreferredContactMethod,
                PreferredContactTime = r.PreferredContactTime,
                LocationId = r.LocationId,
                LocationName = r.Location != null ? r.Location.Name : null,
                Status = r.Status,
                Notes = r.Notes,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                ClosedAt = r.ClosedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<SellRequestResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<SellRequestResponse?> GetRequestByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var request = await _requestRepository.GetByIdAsync(id, cancellationToken);
        if (request is null) return null;

        if (request.UserId != userId)
            throw new ForbiddenException("You do not have access to this sell request.");

        return new SellRequestResponse
        {
            Id = request.Id,
            UserId = request.UserId,
            SellVehicleId = request.SellVehicleId,
            VehicleDisplayName = request.SellVehicle != null ? $"{request.SellVehicle.Brand?.Name} {request.SellVehicle.Model?.Name}" : string.Empty,
            PreferredContactMethod = request.PreferredContactMethod,
            PreferredContactTime = request.PreferredContactTime,
            LocationId = request.LocationId,
            LocationName = request.Location != null ? request.Location.Name : null,
            Status = request.Status,
            Notes = request.Notes,
            CreatedAt = request.CreatedAt,
            UpdatedAt = request.UpdatedAt,
            ClosedAt = request.ClosedAt
        };
    }

    public async Task<SellRequestResponse> CreateRequestAsync(CreateSellRequestRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var sellRequest = new SellRequest
        {
            UserId = userId,
            PreferredContactMethod = request.PreferredContactMethod,
            PreferredContactTime = request.PreferredContactTime,
            LocationId = request.LocationId,
            Status = SellRequestStatus.Draft,
            Notes = request.Notes
        };

        await _requestRepository.AddAsync(sellRequest, cancellationToken);

        await AddStatusHistoryAsync(sellRequest.Id, null, SellRequestStatus.Draft, "Request created", userId, cancellationToken);

        return new SellRequestResponse
        {
            Id = sellRequest.Id,
            UserId = sellRequest.UserId,
            SellVehicleId = sellRequest.SellVehicleId,
            VehicleDisplayName = string.Empty,
            PreferredContactMethod = sellRequest.PreferredContactMethod,
            PreferredContactTime = sellRequest.PreferredContactTime,
            LocationId = sellRequest.LocationId,
            LocationName = null,
            Status = sellRequest.Status,
            Notes = sellRequest.Notes,
            CreatedAt = sellRequest.CreatedAt,
            UpdatedAt = sellRequest.UpdatedAt,
            ClosedAt = sellRequest.ClosedAt
        };
    }

    public async Task<SellRequestResponse> UpdateRequestAsync(Guid id, UpdateSellRequestRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var sellRequest = await _requestRepository.GetByIdAsync(id, cancellationToken);
        if (sellRequest is null) throw new NotFoundException("SellRequest", id);

        if (sellRequest.UserId != userId)
            throw new ForbiddenException("You can only update your own sell requests.");

        if (sellRequest.Status != SellRequestStatus.Draft && sellRequest.Status != SellRequestStatus.Submitted)
            throw new ConflictException($"Cannot update a sell request with status {sellRequest.Status}.");

        if (request.PreferredContactMethod.HasValue) sellRequest.PreferredContactMethod = request.PreferredContactMethod.Value;
        if (request.PreferredContactTime != null) sellRequest.PreferredContactTime = request.PreferredContactTime;
        if (request.Notes != null) sellRequest.Notes = request.Notes;

        await _requestRepository.UpdateAsync(sellRequest, cancellationToken);

        return new SellRequestResponse
        {
            Id = sellRequest.Id,
            UserId = sellRequest.UserId,
            SellVehicleId = sellRequest.SellVehicleId,
            VehicleDisplayName = string.Empty,
            PreferredContactMethod = sellRequest.PreferredContactMethod,
            PreferredContactTime = sellRequest.PreferredContactTime,
            LocationId = sellRequest.LocationId,
            LocationName = null,
            Status = sellRequest.Status,
            Notes = sellRequest.Notes,
            CreatedAt = sellRequest.CreatedAt,
            UpdatedAt = sellRequest.UpdatedAt,
            ClosedAt = sellRequest.ClosedAt
        };
    }

    public async Task<SellRequestResponse> SubmitRequestAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var sellRequest = await _requestRepository.GetByIdAsync(id, cancellationToken);
        if (sellRequest is null) throw new NotFoundException("SellRequest", id);

        if (sellRequest.UserId != userId)
            throw new ForbiddenException("You can only submit your own sell requests.");

        if (sellRequest.Status != SellRequestStatus.Draft)
            throw new ConflictException($"Cannot submit a sell request with status {sellRequest.Status}.");

        var oldStatus = sellRequest.Status;
        sellRequest.Status = SellRequestStatus.Submitted;
        await _requestRepository.UpdateAsync(sellRequest, cancellationToken);

        await AddStatusHistoryAsync(sellRequest.Id, oldStatus, SellRequestStatus.Submitted, "Request submitted", userId, cancellationToken);

        return new SellRequestResponse
        {
            Id = sellRequest.Id,
            UserId = sellRequest.UserId,
            SellVehicleId = sellRequest.SellVehicleId,
            VehicleDisplayName = string.Empty,
            PreferredContactMethod = sellRequest.PreferredContactMethod,
            PreferredContactTime = sellRequest.PreferredContactTime,
            LocationId = sellRequest.LocationId,
            LocationName = null,
            Status = sellRequest.Status,
            Notes = sellRequest.Notes,
            CreatedAt = sellRequest.CreatedAt,
            UpdatedAt = sellRequest.UpdatedAt,
            ClosedAt = sellRequest.ClosedAt
        };
    }

    public async Task<SellRequestResponse> CancelRequestAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var sellRequest = await _requestRepository.GetByIdAsync(id, cancellationToken);
        if (sellRequest is null) throw new NotFoundException("SellRequest", id);

        if (sellRequest.UserId != userId)
            throw new ForbiddenException("You can only cancel your own sell requests.");

        if (sellRequest.Status == SellRequestStatus.Completed || sellRequest.Status == SellRequestStatus.Cancelled)
            throw new ConflictException($"Cannot cancel a sell request with status {sellRequest.Status}.");

        var oldStatus = sellRequest.Status;
        sellRequest.Status = SellRequestStatus.Cancelled;
        sellRequest.ClosedAt = DateTime.UtcNow;
        await _requestRepository.UpdateAsync(sellRequest, cancellationToken);

        await AddStatusHistoryAsync(sellRequest.Id, oldStatus, SellRequestStatus.Cancelled, "Request cancelled", userId, cancellationToken);

        return new SellRequestResponse
        {
            Id = sellRequest.Id,
            UserId = sellRequest.UserId,
            SellVehicleId = sellRequest.SellVehicleId,
            VehicleDisplayName = string.Empty,
            PreferredContactMethod = sellRequest.PreferredContactMethod,
            PreferredContactTime = sellRequest.PreferredContactTime,
            LocationId = sellRequest.LocationId,
            LocationName = null,
            Status = sellRequest.Status,
            Notes = sellRequest.Notes,
            CreatedAt = sellRequest.CreatedAt,
            UpdatedAt = sellRequest.UpdatedAt,
            ClosedAt = sellRequest.ClosedAt
        };
    }

    public async Task<SellVehicleResponse> CreateSellVehicleAsync(Guid requestId, CreateSellVehicleRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var sellRequest = await _requestRepository.GetByIdAsync(requestId, cancellationToken);
        if (sellRequest is null) throw new NotFoundException("SellRequest", requestId);

        if (sellRequest.UserId != userId)
            throw new ForbiddenException("You can only add vehicles to your own sell requests.");

        if (sellRequest.Status != SellRequestStatus.Draft)
            throw new ConflictException($"Cannot add vehicle to a sell request with status {sellRequest.Status}.");

        var sellVehicle = new SellVehicle
        {
            SellRequestId = requestId,
            VehicleId = request.VehicleId,
            VehicleTypeId = request.VehicleTypeId,
            BrandId = request.BrandId,
            ModelId = request.ModelId,
            VariantId = request.VariantId,
            ManufactureYear = request.ManufactureYear,
            RegistrationYear = request.RegistrationYear,
            Mileage = request.Mileage,
            MileageUnit = request.MileageUnit,
            FuelTypeId = request.FuelTypeId,
            TransmissionTypeId = request.TransmissionTypeId,
            Condition = request.Condition,
            AskingPrice = request.AskingPrice,
            Description = request.Description,
            LocationId = request.LocationId
        };

        await _vehicleRepository.AddAsync(sellVehicle, cancellationToken);

        return new SellVehicleResponse
        {
            Id = sellVehicle.Id,
            SellRequestId = sellVehicle.SellRequestId,
            VehicleId = sellVehicle.VehicleId,
            VehicleTypeId = sellVehicle.VehicleTypeId,
            VehicleTypeName = string.Empty,
            BrandId = sellVehicle.BrandId,
            BrandName = string.Empty,
            ModelId = sellVehicle.ModelId,
            ModelName = string.Empty,
            VariantId = sellVehicle.VariantId,
            VariantName = null,
            ManufactureYear = sellVehicle.ManufactureYear,
            RegistrationYear = sellVehicle.RegistrationYear,
            Mileage = sellVehicle.Mileage,
            MileageUnit = sellVehicle.MileageUnit,
            FuelTypeId = sellVehicle.FuelTypeId,
            FuelTypeName = string.Empty,
            TransmissionTypeId = sellVehicle.TransmissionTypeId,
            TransmissionTypeName = null,
            Condition = sellVehicle.Condition,
            AskingPrice = sellVehicle.AskingPrice,
            Description = sellVehicle.Description,
            LocationId = sellVehicle.LocationId,
            LocationName = null
        };
    }

    public async Task<SellVehicleResponse?> GetSellVehicleAsync(Guid requestId, Guid userId, CancellationToken cancellationToken = default)
    {
        var sellRequest = await _requestRepository.GetByIdAsync(requestId, cancellationToken);
        if (sellRequest is null) return null;

        if (sellRequest.UserId != userId)
            throw new ForbiddenException("You do not have access to this sell request.");

        var sellVehicle = await _vehicleRepository.GetByIdAsync(sellRequest.SellVehicleId, cancellationToken);
        if (sellVehicle is null) return null;

        return new SellVehicleResponse
        {
            Id = sellVehicle.Id,
            SellRequestId = sellVehicle.SellRequestId,
            VehicleId = sellVehicle.VehicleId,
            VehicleTypeId = sellVehicle.VehicleTypeId,
            VehicleTypeName = sellVehicle.VehicleType != null ? sellVehicle.VehicleType.Name : string.Empty,
            BrandId = sellVehicle.BrandId,
            BrandName = sellVehicle.Brand != null ? sellVehicle.Brand.Name : string.Empty,
            ModelId = sellVehicle.ModelId,
            ModelName = sellVehicle.Model != null ? sellVehicle.Model.Name : string.Empty,
            VariantId = sellVehicle.VariantId,
            VariantName = sellVehicle.Variant != null ? sellVehicle.Variant.Name : null,
            ManufactureYear = sellVehicle.ManufactureYear,
            RegistrationYear = sellVehicle.RegistrationYear,
            Mileage = sellVehicle.Mileage,
            MileageUnit = sellVehicle.MileageUnit,
            FuelTypeId = sellVehicle.FuelTypeId,
            FuelTypeName = sellVehicle.FuelType != null ? sellVehicle.FuelType.Name : string.Empty,
            TransmissionTypeId = sellVehicle.TransmissionTypeId,
            TransmissionTypeName = sellVehicle.TransmissionType != null ? sellVehicle.TransmissionType.Name : null,
            Condition = sellVehicle.Condition,
            AskingPrice = sellVehicle.AskingPrice,
            Description = sellVehicle.Description,
            LocationId = sellVehicle.LocationId,
            LocationName = sellVehicle.Location != null ? sellVehicle.Location.Name : null
        };
    }

    public async Task<SellOfferResponse> CreateOfferAsync(Guid requestId, CreateSellOfferRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var sellRequest = await _requestRepository.GetByIdAsync(requestId, cancellationToken);
        if (sellRequest is null) throw new NotFoundException("SellRequest", requestId);

        if (sellRequest.UserId == userId)
            throw new ConflictException("You cannot make an offer on your own sell request.");

        if (sellRequest.Status != SellRequestStatus.Submitted && sellRequest.Status != SellRequestStatus.UnderReview && sellRequest.Status != SellRequestStatus.OfferReceived)
            throw new ConflictException($"Cannot make an offer on a sell request with status {sellRequest.Status}.");

        var offer = new SellOffer
        {
            SellRequestId = requestId,
            OfferedByUserId = userId,
            OfferedAmount = request.OfferedAmount,
            Currency = request.Currency,
            ValidUntil = request.ValidUntil,
            Notes = request.Notes,
            Status = SellOfferStatus.Submitted
        };

        await _offerRepository.AddAsync(offer, cancellationToken);

        return new SellOfferResponse
        {
            Id = offer.Id,
            SellRequestId = offer.SellRequestId,
            OfferedByUserId = offer.OfferedByUserId,
            OfferedAmount = offer.OfferedAmount,
            Currency = offer.Currency,
            ValidUntil = offer.ValidUntil,
            Notes = offer.Notes,
            Status = offer.Status,
            CreatedAt = offer.CreatedAt,
            AcceptedAt = offer.AcceptedAt,
            RejectedAt = offer.RejectedAt
        };
    }

    public async Task<SellOfferResponse> AcceptOfferAsync(Guid offerId, Guid userId, CancellationToken cancellationToken = default)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId, cancellationToken);
        if (offer is null) throw new NotFoundException("SellOffer", offerId);

        var sellRequest = await _requestRepository.GetByIdAsync(offer.SellRequestId, cancellationToken);
        if (sellRequest is null) throw new NotFoundException("SellRequest", offer.SellRequestId);

        if (sellRequest.UserId != userId)
            throw new ForbiddenException("You can only accept offers on your own sell requests.");

        if (offer.Status != SellOfferStatus.Submitted)
            throw new ConflictException($"Cannot accept an offer with status {offer.Status}.");

        offer.Status = SellOfferStatus.Accepted;
        offer.AcceptedAt = DateTime.UtcNow;
        await _offerRepository.UpdateAsync(offer, cancellationToken);

        var oldStatus = sellRequest.Status;
        sellRequest.Status = SellRequestStatus.Accepted;
        await _requestRepository.UpdateAsync(sellRequest, cancellationToken);

        await AddStatusHistoryAsync(sellRequest.Id, oldStatus, SellRequestStatus.Accepted, "Offer accepted", userId, cancellationToken);

        return new SellOfferResponse
        {
            Id = offer.Id,
            SellRequestId = offer.SellRequestId,
            OfferedByUserId = offer.OfferedByUserId,
            OfferedAmount = offer.OfferedAmount,
            Currency = offer.Currency,
            ValidUntil = offer.ValidUntil,
            Notes = offer.Notes,
            Status = offer.Status,
            CreatedAt = offer.CreatedAt,
            AcceptedAt = offer.AcceptedAt,
            RejectedAt = offer.RejectedAt
        };
    }

    public async Task<SellOfferResponse> RejectOfferAsync(Guid offerId, Guid userId, CancellationToken cancellationToken = default)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId, cancellationToken);
        if (offer is null) throw new NotFoundException("SellOffer", offerId);

        var sellRequest = await _requestRepository.GetByIdAsync(offer.SellRequestId, cancellationToken);
        if (sellRequest is null) throw new NotFoundException("SellRequest", offer.SellRequestId);

        if (sellRequest.UserId != userId)
            throw new ForbiddenException("You can only reject offers on your own sell requests.");

        if (offer.Status != SellOfferStatus.Submitted)
            throw new ConflictException($"Cannot reject an offer with status {offer.Status}.");

        offer.Status = SellOfferStatus.Rejected;
        offer.RejectedAt = DateTime.UtcNow;
        await _offerRepository.UpdateAsync(offer, cancellationToken);

        return new SellOfferResponse
        {
            Id = offer.Id,
            SellRequestId = offer.SellRequestId,
            OfferedByUserId = offer.OfferedByUserId,
            OfferedAmount = offer.OfferedAmount,
            Currency = offer.Currency,
            ValidUntil = offer.ValidUntil,
            Notes = offer.Notes,
            Status = offer.Status,
            CreatedAt = offer.CreatedAt,
            AcceptedAt = offer.AcceptedAt,
            RejectedAt = offer.RejectedAt
        };
    }

    public async Task<PagedResult<SellOfferResponse>> GetRequestOffersAsync(Guid requestId, Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var sellRequest = await _requestRepository.GetByIdAsync(requestId, cancellationToken);
        if (sellRequest is null) throw new NotFoundException("SellRequest", requestId);

        if (sellRequest.UserId != userId)
            throw new ForbiddenException("You do not have access to this sell request.");

        var query = await _offerRepository.QueryAsync(true, cancellationToken);
        var filteredQuery = query.Where(o => o.SellRequestId == requestId).OrderByDescending(o => o.CreatedAt);

        var totalCount = await filteredQuery.CountAsync(cancellationToken);
        var items = await filteredQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new SellOfferResponse
            {
                Id = o.Id,
                SellRequestId = o.SellRequestId,
                OfferedByUserId = o.OfferedByUserId,
                OfferedAmount = o.OfferedAmount,
                Currency = o.Currency,
                ValidUntil = o.ValidUntil,
                Notes = o.Notes,
                Status = o.Status,
                CreatedAt = o.CreatedAt,
                AcceptedAt = o.AcceptedAt,
                RejectedAt = o.RejectedAt
            })
            .ToListAsync(cancellationToken);

        return PagedResult<SellOfferResponse>.Create(items, totalCount, page, pageSize);
    }

    private async Task AddStatusHistoryAsync(Guid sellRequestId, SellRequestStatus? oldStatus, SellRequestStatus newStatus, string? reason, Guid? changedByUserId, CancellationToken cancellationToken)
    {
        var history = new SellRequestStatusHistory
        {
            SellRequestId = sellRequestId,
            OldStatus = oldStatus,
            NewStatus = newStatus,
            Reason = reason,
            ChangedByUserId = changedByUserId,
            ChangedAt = DateTime.UtcNow
        };

        await _historyRepository.AddAsync(history, cancellationToken);
    }
}
