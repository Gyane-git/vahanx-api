using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of fuel station service.
/// </summary>
public class FuelStationService : IFuelStationService
{
    private readonly IRepository<FuelStation> _repository;

    public FuelStationService(IRepository<FuelStation> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<FuelStationListDto>> GetAllAsync(int page, int pageSize, string? keyword = null, Guid? locationId = null, FuelStationStatus? status = null, VerificationStatus? verificationStatus = null, bool? is24Hours = null, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(fs => fs.Name.Contains(keyword) || (fs.Description != null && fs.Description.Contains(keyword)));

        if (status.HasValue)
            query = query.Where(fs => fs.Status == status.Value);

        if (verificationStatus.HasValue)
            query = query.Where(fs => fs.VerificationStatus == verificationStatus.Value);

        if (is24Hours.HasValue)
            query = query.Where(fs => fs.Is24Hours == is24Hours.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(fs => fs.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(fs => new FuelStationListDto
            {
                Id = fs.Id,
                Name = fs.Name,
                OperatorName = fs.OperatorName,
                Status = fs.Status,
                VerificationStatus = fs.VerificationStatus,
                Is24Hours = fs.Is24Hours,
                Latitude = fs.Latitude,
                Longitude = fs.Longitude
            })
            .ToListAsync(cancellationToken);

        return PagedResult<FuelStationListDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<FuelStationDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var station = await _repository.GetByIdAsync(id, cancellationToken);
        if (station is null) return null;

        return new FuelStationDetailsDto
        {
            Id = station.Id,
            Name = station.Name,
            OperatorName = station.OperatorName,
            Description = station.Description,
            ContactPhone = station.ContactPhone,
            ContactEmail = station.ContactEmail,
            WebsiteUrl = station.WebsiteUrl,
            Is24Hours = station.Is24Hours,
            Status = station.Status,
            VerificationStatus = station.VerificationStatus,
            Latitude = station.Latitude,
            Longitude = station.Longitude
        };
    }

    public async Task<PagedResult<FuelStationListDto>> GetNearbyAsync(double latitude, double longitude, double radiusKm, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);

        var allStations = await query
            .Where(fs => fs.Status == FuelStationStatus.Active)
            .ToListAsync(cancellationToken);

        var nearbyStations = allStations
            .Select(fs => new
            {
                Station = fs,
                DistanceKm = CalculateDistance(latitude, longitude, fs.Latitude, fs.Longitude)
            })
            .Where(x => x.DistanceKm <= radiusKm)
            .OrderBy(x => x.DistanceKm)
            .ToList();

        var totalCount = nearbyStations.Count;
        var items = nearbyStations
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new FuelStationListDto
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

        return PagedResult<FuelStationListDto>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<FuelStationFuelTypeResponse>> GetFuelTypesAsync(Guid stationId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);
        var station = await query.FirstOrDefaultAsync(fs => fs.Id == stationId, cancellationToken);
        if (station is null) throw new Domain.Exceptions.NotFoundException("FuelStation", stationId);

        var fuelTypes = await _repository.QueryAsync(true, cancellationToken);
        var stationFuelTypes = await fuelTypes
            .Where(fs => fs.Id == stationId)
            .SelectMany(fs => fs.FuelTypes)
            .ToListAsync(cancellationToken);

        var totalCount = stationFuelTypes.Count;
        var items = stationFuelTypes
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ft => new FuelStationFuelTypeResponse
            {
                Id = ft.Id,
                FuelStationId = ft.FuelStationId,
                FuelType = ft.FuelType,
                IsAvailable = ft.IsAvailable
            })
            .ToList();

        return PagedResult<FuelStationFuelTypeResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<FuelStationAvailabilityResponse>> GetAvailabilityAsync(Guid stationId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);
        var station = await query.FirstOrDefaultAsync(fs => fs.Id == stationId, cancellationToken);
        if (station is null) throw new Domain.Exceptions.NotFoundException("FuelStation", stationId);

        var availability = await _repository.QueryAsync(true, cancellationToken);
        var stationAvailability = await availability
            .Where(fs => fs.Id == stationId)
            .SelectMany(fs => fs.Availability)
            .ToListAsync(cancellationToken);

        var totalCount = stationAvailability.Count;
        var items = stationAvailability
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new FuelStationAvailabilityResponse
            {
                Id = a.Id,
                FuelStationId = a.FuelStationId,
                FuelStationFuelTypeId = a.FuelStationFuelTypeId,
                FuelType = a.FuelType != null ? a.FuelType.FuelType : null,
                Status = a.Status,
                UpdatedAt = a.UpdatedAt ?? DateTime.MinValue
            })
            .ToList();

        return PagedResult<FuelStationAvailabilityResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<FuelStationPriceResponse>> GetPricesAsync(Guid stationId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);
        var station = await query.FirstOrDefaultAsync(fs => fs.Id == stationId, cancellationToken);
        if (station is null) throw new Domain.Exceptions.NotFoundException("FuelStation", stationId);

        var prices = await _repository.QueryAsync(true, cancellationToken);
        var stationPrices = await prices
            .Where(fs => fs.Id == stationId)
            .SelectMany(fs => fs.Prices)
            .ToListAsync(cancellationToken);

        var totalCount = stationPrices.Count;
        var items = stationPrices
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new FuelStationPriceResponse
            {
                Id = p.Id,
                FuelStationId = p.FuelStationId,
                FuelStationFuelTypeId = p.FuelStationFuelTypeId,
                FuelType = p.FuelType != null ? p.FuelType.FuelType : null,
                Price = p.Price,
                Currency = p.Currency,
                Unit = p.Unit,
                EffectiveFrom = p.EffectiveFrom,
                EffectiveTo = p.EffectiveTo,
                IsCurrent = p.IsCurrent
            })
            .ToList();

        return PagedResult<FuelStationPriceResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<FuelStationAmenityResponse>> GetAmenitiesAsync(Guid stationId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);
        var station = await query.FirstOrDefaultAsync(fs => fs.Id == stationId, cancellationToken);
        if (station is null) throw new Domain.Exceptions.NotFoundException("FuelStation", stationId);

        var amenities = await _repository.QueryAsync(true, cancellationToken);
        var stationAmenities = await amenities
            .Where(fs => fs.Id == stationId)
            .SelectMany(fs => fs.Amenities)
            .ToListAsync(cancellationToken);

        var totalCount = stationAmenities.Count;
        var items = stationAmenities
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new FuelStationAmenityResponse
            {
                Id = a.Id,
                FuelStationId = a.FuelStationId,
                Name = a.Name,
                Code = a.Code,
                Description = a.Description,
                IsAvailable = a.IsAvailable
            })
            .ToList();

        return PagedResult<FuelStationAmenityResponse>.Create(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<FuelStationWorkingHourResponse>> GetWorkingHoursAsync(Guid stationId, CancellationToken cancellationToken = default)
    {
        var query = await _repository.QueryAsync(true, cancellationToken);
        var station = await query.FirstOrDefaultAsync(fs => fs.Id == stationId, cancellationToken);
        if (station is null) throw new Domain.Exceptions.NotFoundException("FuelStation", stationId);

        var workingHours = await _repository.QueryAsync(true, cancellationToken);
        var stationHours = await workingHours
            .Where(fs => fs.Id == stationId)
            .SelectMany(fs => fs.WorkingHours)
            .ToListAsync(cancellationToken);

        var items = stationHours
            .Select(w => new FuelStationWorkingHourResponse
            {
                Id = w.Id,
                FuelStationId = w.FuelStationId,
                DayOfWeek = w.DayOfWeek,
                OpeningTime = w.OpeningTime,
                ClosingTime = w.ClosingTime,
                IsClosed = w.IsClosed
            })
            .ToList();

        return PagedResult<FuelStationWorkingHourResponse>.Create(items, items.Count, 1, items.Count);
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
