using VahanX.Application.Common;
using VahanX.Application.DTOs.Services;
using VahanX.Domain.Enums;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for service booking operations.
/// </summary>
public interface IServiceBookingService
{
    Task<PagedResult<ServiceBookingResponse>> GetAllAsync(int page, int pageSize, Guid? userId = null, Guid? branchId = null, ServiceBookingStatus? status = null, CancellationToken cancellationToken = default);
    Task<ServiceBookingResponse?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<ServiceBookingResponse> CreateAsync(CreateServiceBookingRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<ServiceBookingResponse> UpdateAsync(Guid id, UpdateServiceBookingRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<ServiceBookingResponse> ConfirmAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<ServiceBookingResponse> RescheduleAsync(Guid id, RescheduleServiceBookingRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<ServiceBookingResponse> StartAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<ServiceBookingResponse> CompleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<ServiceBookingResponse> CancelAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<ServiceBookingResponse> RejectAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<ServiceBookingResponse> NoShowAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<PagedResult<ServiceBookingResponse>> GetMyBookingsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<ServiceBookingResponse>> GetBranchBookingsAsync(Guid branchId, int page, int pageSize, CancellationToken cancellationToken = default);
}
