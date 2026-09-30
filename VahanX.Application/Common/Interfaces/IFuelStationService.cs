using VahanX.Application.Common;
using VahanX.Application.DTOs.Services;
using VahanX.Domain.Enums;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for fuel station operations.
/// </summary>
public interface IFuelStationService
{
    Task<PagedResult<FuelStationListDto>> GetAllAsync(int page, int pageSize, string? keyword = null, Guid? locationId = null, FuelStationStatus? status = null, VerificationStatus? verificationStatus = null, bool? is24Hours = null, CancellationToken cancellationToken = default);
    Task<FuelStationDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<FuelStationListDto>> GetNearbyAsync(double latitude, double longitude, double radiusKm, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<FuelStationFuelTypeResponse>> GetFuelTypesAsync(Guid stationId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<FuelStationAvailabilityResponse>> GetAvailabilityAsync(Guid stationId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<FuelStationPriceResponse>> GetPricesAsync(Guid stationId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<FuelStationAmenityResponse>> GetAmenitiesAsync(Guid stationId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<FuelStationWorkingHourResponse>> GetWorkingHoursAsync(Guid stationId, CancellationToken cancellationToken = default);
}
