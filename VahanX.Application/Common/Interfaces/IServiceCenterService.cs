using VahanX.Application.Common;
using VahanX.Application.DTOs.Services;
using VahanX.Domain.Enums;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for service center operations.
/// </summary>
public interface IServiceCenterService
{
    Task<PagedResult<ServiceCenterListDto>> GetAllAsync(int page, int pageSize, string? keyword = null, Guid? locationId = null, ServiceCenterStatus? status = null, VerificationStatus? verificationStatus = null, CancellationToken cancellationToken = default);
    Task<ServiceCenterDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceCenterDetailsDto> CreateAsync(CreateServiceCenterRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<ServiceCenterDetailsDto> UpdateAsync(Guid id, UpdateServiceCenterRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<ServiceCenterBranchResponse> CreateBranchAsync(Guid serviceCenterId, CreateServiceCenterBranchRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<ServiceCenterBranchResponse> UpdateBranchAsync(Guid serviceCenterId, Guid branchId, UpdateServiceCenterBranchRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task DeleteBranchAsync(Guid serviceCenterId, Guid branchId, Guid userId, CancellationToken cancellationToken = default);
    Task<PagedResult<ServiceCenterBranchResponse>> GetBranchesAsync(Guid serviceCenterId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<ServiceCenterBranchResponse?> GetBranchByIdAsync(Guid serviceCenterId, Guid branchId, CancellationToken cancellationToken = default);
    Task<PagedResult<ServiceCenterServiceResponse>> GetBranchServicesAsync(Guid branchId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<ServicePackageResponse>> GetBranchPackagesAsync(Guid branchId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<ServiceWorkingHourResponse>> GetBranchWorkingHoursAsync(Guid branchId, CancellationToken cancellationToken = default);
    Task<PagedResult<ServiceCenterListDto>> GetNearbyAsync(double latitude, double longitude, double radiusKm, int page, int pageSize, CancellationToken cancellationToken = default);
}
