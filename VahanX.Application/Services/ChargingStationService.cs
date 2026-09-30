using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of charging station service.
/// </summary>
public class ChargingStationService : IChargingStationService
{
    private readonly IRepository<ChargingStation> _repository;

    public ChargingStationService(IRepository<ChargingStation> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<ChargingStationListDto>> GetAllAsync(int page, int pageSize, string? keyword = null, Guid? locationId = null, ChargingStationStatus? status = null, VerificationStatus? verificationStatus = null, bool? is24Hours = null, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(cs => cs.Name.Contains(keyword) || (cs.Description != null && cs.Description.Contains(keyword)));

        if (status.HasValue)
            query = query.Where(cs => cs.Status == status.Value);

        if (verificationStatus.HasValue)
            query = query.Where(cs => cs.VerificationStatus == verificationStatus.Value);

        if (is24Hours.HasValue)
            query = query.Where(cs => cs.Is24Hours == is24Hours.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(cs => cs.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(cs => new ChargingStationListDto
            {
                Id = cs.Id,
                Name = cs.Name,
                OperatorName = cs.OperatorName,
                Status = cs.Status,
                VerificationStatus = cs.VerificationStatus,
                Is24Hours = cs.Is24Hours,
                Latitude = cs.Latitude,
                Longitude = cs.Longitude
            })
            .ToListAsync(cancellationToken);

        return PagedResult<ChargingStationListDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<ChargingStationDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var station = await _repository.GetByIdAsync(id, cancellationToken);
        if (station is null) return null;

        return new ChargingStationDetailsDto
        {
            Id = station.Id,
            Name = station.Name,
            Description = station.Description,
            OperatorName = station.OperatorName,
            ContactPhone = station.ContactPhone,
            ContactEmail = station.ContactEmail,
            WebsiteUrl = station.WebsiteUrl,
            Status = station.Status,
            VerificationStatus = station.VerificationStatus,
            Is24Hours = station.Is24Hours,
            Latitude = station.Latitude,
            Longitude = station.Longitude
        };
    }

    public async Task<PagedResult<ChargingStationListDto>> GetNearbyAsync(double latitude, double longitude, double radiusKm, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        var allStations = await query
            .Where(cs => cs.Status == ChargingStationStatus.Active)
            .ToListAsync(cancellationToken);

        var nearbyStations = allStations
            .Select(cs => new
            {
                Station = cs,
                DistanceKm = CalculateDistance(latitude, longitude, cs.Latitude, cs.Longitude)
            })
            .Where(x => x.DistanceKm <= radiusKm)
            .OrderBy(x => x.DistanceKm)
            .ToList();

        var totalCount = nearbyStations.Count;
        var items = nearbyStations
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ChargingStationListDto
            {
                Id = x.Station.Id,
                Name = x.Station.Name,
                OperatorName = x.Station.OperatorName,
                Status = x.Station.Status,
                VerificationStatus = x.Station.VerificationStatus,
                Is24Hours = x.Station.Is24Hours,
                Latitude = x.Station.Latitude,
                Longitude = x.Station.Longitude,
                DistanceKm = Math.Round(x.DistanceKm, 2)
            })
            .ToList();

        return PagedResult<ChargingStationListDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<ChargingStationConnectorResponse>> GetConnectorsAsync(Guid stationId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);
        var station = await query.FirstOrDefaultAsync(cs => cs.Id == stationId, cancellationToken);
        if (station is null) throw new Domain.Exceptions.NotFoundException("ChargingStation", stationId);

        var connectors = await _repository.QueryAsync(true, cancellationToken);
        var stationConnectors = await connectors
            .Where(cs => cs.Id == stationId)
            .SelectMany(cs => cs.Connectors)
            .ToListAsync(cancellationToken);

        var totalCount = stationConnectors.Count;
        var items = stationConnectors
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ChargingStationConnectorResponse
            {
                Id = c.Id,
                ChargingStationId = c.ChargingStationId,
                ConnectorType = c.ConnectorType,
                PowerKw = c.PowerKw,
                Voltage = c.Voltage,
                Amperage = c.Amperage,
                Quantity = c.Quantity,
                AvailableQuantity = c.AvailableQuantity,
                ChargingMode = c.ChargingMode,
                Status = c.Status
            })
            .ToList();

        return PagedResult<ChargingStationConnectorResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<ChargingStationAmenityResponse>> GetAmenitiesAsync(Guid stationId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);
        var station = await query.FirstOrDefaultAsync(cs => cs.Id == stationId, cancellationToken);
        if (station is null) throw new Domain.Exceptions.NotFoundException("ChargingStation", stationId);

        var amenities = await _repository.QueryAsync(true, cancellationToken);
        var stationAmenities = await amenities
            .Where(cs => cs.Id == stationId)
            .SelectMany(cs => cs.Amenities)
            .ToListAsync(cancellationToken);

        var totalCount = stationAmenities.Count;
        var items = stationAmenities
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new ChargingStationAmenityResponse
            {
                Id = a.Id,
                ChargingStationId = a.ChargingStationId,
                Name = a.Name,
                Code = a.Code,
                Description = a.Description,
                IsAvailable = a.IsAvailable
            })
            .ToList();

        return PagedResult<ChargingStationAmenityResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<ChargingStationAvailabilityResponse>> GetAvailabilityAsync(Guid stationId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);
        var station = await query.FirstOrDefaultAsync(cs => cs.Id == stationId, cancellationToken);
        if (station is null) throw new Domain.Exceptions.NotFoundException("ChargingStation", stationId);

        var availability = await _repository.QueryAsync(true, cancellationToken);
        var stationAvailability = await availability
            .Where(cs => cs.Id == stationId)
            .SelectMany(cs => cs.Availability)
            .ToListAsync(cancellationToken);

        var totalCount = stationAvailability.Count;
        var items = stationAvailability
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new ChargingStationAvailabilityResponse
            {
                Id = a.Id,
                ChargingStationId = a.ChargingStationId,
                ConnectorId = a.ConnectorId,
                Status = a.Status,
                AvailableFrom = a.AvailableFrom,
                AvailableUntil = a.AvailableUntil,
                UpdatedAt = a.UpdatedAt ?? DateTime.MinValue
            })
            .ToList();

        return PagedResult<ChargingStationAvailabilityResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<ChargingStationPriceResponse>> GetPricesAsync(Guid stationId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);
        var station = await query.FirstOrDefaultAsync(cs => cs.Id == stationId, cancellationToken);
        if (station is null) throw new Domain.Exceptions.NotFoundException("ChargingStation", stationId);

        var prices = await _repository.QueryAsync(true, cancellationToken);
        var stationPrices = await prices
            .Where(cs => cs.Id == stationId)
            .SelectMany(cs => cs.Prices)
            .ToListAsync(cancellationToken);

        var totalCount = stationPrices.Count;
        var items = stationPrices
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ChargingStationPriceResponse
            {
                Id = p.Id,
                ChargingStationId = p.ChargingStationId,
                ConnectorType = p.ConnectorType,
                PricingType = p.PricingType,
                Price = p.Price,
                Currency = p.Currency,
                Unit = p.Unit,
                EffectiveFrom = p.EffectiveFrom,
                EffectiveTo = p.EffectiveTo,
                IsActive = p.IsActive
            })
            .ToList();

        return PagedResult<ChargingStationPriceResponse>.Create(items, totalCount, page, pageSize);
    }

    private static double CalculateDistance(double lat1, double lon1, double? lat2, double? lon2)
    {
        if (!lat2.HasValue || !lon2.HasValue) return double.MaxValue;

        const double R = 6371;
        var dLat = ToRad(lat2.Value - lat1);
        var dLon = ToRad(lon2.Value - lon1);
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2.Value)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private static double ToRad(double degrees) => degrees * Math.PI / 180;
}
