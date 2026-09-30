using VahanX.Application.Common;
using VahanX.Application.DTOs.Trust;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for vehicle history operations.
/// </summary>
public interface IVehicleHistoryService
{
    Task<VehicleHistoryResponse> GetVehicleHistoryAsync(Guid vehicleId, CancellationToken cancellationToken = default);
    Task<PagedResult<OwnershipHistoryResponse>> GetOwnershipHistoryAsync(Guid vehicleId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<MileageHistoryResponse>> GetMileageHistoryAsync(Guid vehicleId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<ServiceHistoryResponse>> GetServiceHistoryAsync(Guid vehicleId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<AccidentHistoryResponse>> GetAccidentHistoryAsync(Guid vehicleId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<InsuranceHistoryResponse>> GetInsuranceHistoryAsync(Guid vehicleId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<RegistrationHistoryResponse>> GetRegistrationHistoryAsync(Guid vehicleId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<PriceHistoryResponse>> GetPriceHistoryAsync(Guid listingId, int page, int pageSize, CancellationToken cancellationToken = default);
}
