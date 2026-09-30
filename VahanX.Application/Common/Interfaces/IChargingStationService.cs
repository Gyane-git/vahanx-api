using VahanX.Application.Common;
using VahanX.Application.DTOs.Services;
using VahanX.Domain.Enums;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for EV charging station operations.
/// </summary>
public interface IChargingStationService
{
    Task<PagedResult<ChargingStationListDto>> GetAllAsync(int page, int pageSize, string? keyword = null, Guid? locationId = null, ChargingStationStatus? status = null, VerificationStatus? verificationStatus = null, bool? is24Hours = null, CancellationToken cancellationToken = default);
    Task<ChargingStationDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<ChargingStationListDto>> GetNearbyAsync(double latitude, double longitude, double radiusKm, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<ChargingStationConnectorResponse>> GetConnectorsAsync(Guid stationId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<ChargingStationAmenityResponse>> GetAmenitiesAsync(Guid stationId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<ChargingStationAvailabilityResponse>> GetAvailabilityAsync(Guid stationId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<ChargingStationPriceResponse>> GetPricesAsync(Guid stationId, int page, int pageSize, CancellationToken cancellationToken = default);
}
